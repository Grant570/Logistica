using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Logistica_Review.Models;

namespace Logistica_Review.Database
{
    public class DatabaseManager
    {
        /*
         * Responsible for all calls to the database.
        */
        private SqlConnection sqlCon;

        public DatabaseManager() {
            this.sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            this.sqlCon.Open();
        }

        public DataSet executeQuery(string query, string table)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(query, sqlCon);
            sqlAdapter.Fill(dataSet, table);

            return dataSet;
        }

        public List<ProjectModel> getManagingProjects(string adminId) {
            //Returns a list of projects the user is currently managing.
            DataTable table = executeQuery("SELECT * FROM Projects WHERE Admin='" + adminId + "'", "Projects").Tables["Projects"];

            List<ProjectModel> projects = new List<ProjectModel>();
            foreach(DataRow row in table.Rows) {
                ProjectModel project = new ProjectModel();
                project.ID = Convert.ToInt32(row.ItemArray[0]);
                project.Name = row.ItemArray[1].ToString();
                project.Description = row.ItemArray[2].ToString();
                project.DueDate = row.ItemArray[6].ToString();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(row.ItemArray[4].ToString());
                XmlNode node = xmlDoc.FirstChild;
                foreach (XmlNode userNode in node.SelectNodes("user"))
                {
                    UserModel user = new UserModel();
                    user.ID = userNode.SelectSingleNode("ID").InnerText;
                    user.FirstName = userNode.SelectSingleNode("FirstName").InnerText;
                    user.LastName = userNode.SelectSingleNode("LastName").InnerText;
                    project.Users.Add(user);
                }
                projects.Add(project);
            }
            return projects;
        }

        public void addProject(string adminId, string projectName, string projectDescription, string dueDate, List<string> users, List<string> questions)
        {
            //Adds a project to the database.
            List<string> userIds = new List<string>();
            string usersXml = "<users>";
            foreach (string email in users)
            {
                DataTable dataTable = executeQuery("SELECT * FROM AspNetUsers WHERE Email='" + email + "'", "AspNetUsers").Tables["AspNetUsers"];
                foreach (DataRow row in dataTable.Rows)
                {
                    usersXml += "<user>";
                    usersXml += "<ID>" + row.ItemArray[0].ToString() + "</ID>";
                    usersXml += "<FirstName>" + row.ItemArray[12].ToString() + "</FirstName>";
                    usersXml += "<LastName>" + row.ItemArray[13].ToString() + "</LastName>";
                    usersXml += "</user>";
                    userIds.Add(row.ItemArray[0].ToString());
                }
            }
            usersXml += "</users>";

            string questionsXml = "<questions>";
            foreach (string question in questions)
            {
                questionsXml += "<question>";
                questionsXml += question;
                questionsXml += "</question>";
            }
            questionsXml += "</questions>";

            SqlCommand command = new SqlCommand("INSERT INTO Projects (Name, Description, Admin, DueDate, Users, Evaluations) VALUES ('" + projectName + "', '" + projectDescription + "', '" + adminId + "', '" + dueDate + "', '" + usersXml + "', '" + questionsXml + "')", sqlCon);
            command.ExecuteNonQuery();

            //Insert assciated evaluations into the database
            string projectID = executeQuery("SELECT TOP 1 ID FROM Projects ORDER BY ID DESC", "Projects").Tables["Projects"].Rows[0].ItemArray[0].ToString();
            string answersXml = "<answers></answers>";
            foreach (string userA in userIds) {
                foreach(string userB in userIds) {
                    if(userA != userB) {
                        command = new SqlCommand("INSERT INTO Evaluations (Project, Questions, Answers, ForUser, SubmittedBy, Submitted) VALUES ('" + projectID.ToString() + "', '" + questionsXml + "', '" + answersXml + "', '" + userA + "', '" + userB + "', '0')", sqlCon);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void removeProject(int projectID)
        {
            SqlCommand command = new SqlCommand("DELETE FROM Projects WHERE ID='" + projectID + "'", sqlCon);
            command.ExecuteNonQuery();

            command = new SqlCommand("DELETE FROM Evaluations WHERE Project='" + projectID + "'", sqlCon);
            command.ExecuteNonQuery();
        }

        public List<ProjectModel> getAssignedProjects(string userId)
        {
            //Get projects the user is currently assigned to.
            DataTable table = executeQuery("SELECT * FROM Projects", "Projects").Tables["Projects"];

            List<ProjectModel> projects = new List<ProjectModel>();
            foreach (DataRow row in table.Rows)
            {
                int month = Convert.ToInt32(row.ItemArray[6].ToString().Substring(0, 2));
                int day = Convert.ToInt32(row.ItemArray[6].ToString().Substring(3, 2));
                int year = Convert.ToInt32(row.ItemArray[6].ToString().Substring(6, 4));
                DateTime dueDate = new DateTime(year, month, day);
                TimeSpan timespan = dueDate - DateTime.Now;
                if (row.ItemArray[4].ToString().Contains(userId) && timespan.TotalDays > 0)
                {
                    ProjectModel project = new ProjectModel();
                    project.ID = Convert.ToInt32(row.ItemArray[0]);
                    project.Name = row.ItemArray[1].ToString();
                    project.Description = row.ItemArray[2].ToString();
                    project.DueDate = row.ItemArray[6].ToString();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(row.ItemArray[4].ToString());
                    XmlNode node = xmlDoc.FirstChild;
                    foreach (XmlNode userNode in node.SelectNodes("user"))
                    {
                        UserModel user = new UserModel();
                        user.ID = userNode.SelectSingleNode("ID").InnerText;
                        if(user.ID != userId) {
                            user.FirstName = userNode.SelectSingleNode("FirstName").InnerText;
                            user.LastName = userNode.SelectSingleNode("LastName").InnerText;
                            project.Users.Add(user);
                        }
                    }

                    projects.Add(project);
                }
            }
            return projects;
        }

        public List<ProjectModel> getReviews(string userId)
        {
            //Gets list of submitted reviews about the user.
            List<ProjectModel> projects = getAssignedProjects(userId);
            List<ProjectModel> returnProjects = new List<ProjectModel>();
            foreach(ProjectModel project in projects) {
                DataTable table = executeQuery("SELECT * FROM Evaluations WHERE ForUser='" + userId + "' AND Project='" + project.ID + "'", "Evaluations").Tables["Evaluations"];
                foreach (DataRow row in table.Rows)
                {
                    EvaluationModel evaluation = new EvaluationModel();
                    evaluation.ID = Convert.ToInt32(row[0]);
                    evaluation.ProjectID = project.ID;
                    evaluation.ProjectName = project.Name;
                    evaluation.ForUserID = row[4].ToString();

                    DataRow user = executeQuery("SELECT * FROM AspNetUsers WHERE ID='" + userId + "'", "AspNetUsers").Tables["AspNetUsers"].Rows[0];
                    evaluation.ForUserName = user.ItemArray[12].ToString() + " " + user.ItemArray[13].ToString();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(row[2].ToString());
                    XmlNode node = xmlDoc.FirstChild;
                    foreach(XmlNode question in node.SelectNodes("question")) {
                        evaluation.Questions.Add(question.InnerText);
                    }

                    xmlDoc.LoadXml(row[3].ToString());
                    node = xmlDoc.FirstChild;
                    foreach (XmlNode answer in node.SelectNodes("answer"))
                    {
                        evaluation.Answers.Add(answer.InnerText);
                    }

                    evaluation.SubmittedByID = row[5].ToString();
                    user = executeQuery("SELECT * FROM AspNetUsers WHERE ID='" + evaluation.SubmittedByID + "'", "AspNetUsers").Tables["AspNetUsers"].Rows[0];
                    evaluation.SubmittedByName = user.ItemArray[12].ToString() + " " + user.ItemArray[13].ToString();
                    evaluation.AdditionalComments = row.ItemArray[7].ToString();

                    if(row.ItemArray[6].ToString() == "True") {
                        project.Evaluations.Add(evaluation);
                    }
                }
                if(project.Evaluations.Count > 0) {
                    returnProjects.Add(project);
                }
            }

            return returnProjects;
        }

        public EvaluationModel getEvaluation(int projectId, string forUser, string submittedBy) {
            //Given the project ID and associated users, returns the right evaluation object.
            EvaluationModel evaluation = new EvaluationModel();
            DataRow evaluationRow = executeQuery("SELECT * FROM Evaluations WHERE Project='" + projectId + "' AND ForUser='" + forUser + "' AND SubmittedBy='" + submittedBy + "'", "Evaluations").Tables["Evaluations"].Rows[0];
            evaluation.ID = Convert.ToInt32(evaluationRow.ItemArray[0]);
            evaluation.ProjectID = projectId;
            evaluation.ForUserID = forUser;
            evaluation.SubmittedByID = submittedBy;

            DataRow project = executeQuery("SELECT Name FROM Projects WHERE ID='" + projectId + "'", "Projects").Tables["Projects"].Rows[0];
            evaluation.ProjectName =project.ItemArray[0].ToString();

            DataRow user = executeQuery("SELECT FirstName, LastName FROM AspNetUsers WHERE ID='" + forUser + "'", "AspNetUsers").Tables["AspNetUsers"].Rows[0];
            evaluation.ForUserName = user.ItemArray[0] + " " + user.ItemArray[1];

            user = executeQuery("SELECT FirstName, LastName FROM AspNetUsers WHERE ID='" + submittedBy + "'", "AspNetUsers").Tables["AspNetUsers"].Rows[0];
            evaluation.SubmittedByName = user.ItemArray[0] + " " + user.ItemArray[1];

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(evaluationRow[2].ToString());
            XmlNode node = xmlDoc.FirstChild;
            foreach(XmlNode question in node.SelectNodes("question")) {
                evaluation.Questions.Add(question.InnerText);
            }

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(evaluationRow[3].ToString());
            node = xmlDoc.FirstChild;
            foreach (XmlNode answer in node.SelectNodes("answer"))
            {
                evaluation.Answers.Add(answer.InnerText);
            }

            evaluation.AdditionalComments = evaluationRow[7].ToString();

            return evaluation;
        }

        public List<EvaluationModel> getEvaluationsForUser(int projectId, string userId)
        {
            //Returns all evaluations for a user and project.
            List<EvaluationModel> evaluations = new List<EvaluationModel>();
            DataTable evaluationsTable = executeQuery("SELECT * FROM Evaluations WHERE Project='" + projectId + "' AND ForUser='" + userId + "'", "Evaluations").Tables["Evaluations"];
            foreach(DataRow row in evaluationsTable.Rows) {
                EvaluationModel evaluation = new EvaluationModel();
                evaluation.ID = Convert.ToInt32(row[0]);
                evaluation.ProjectID = projectId;
                evaluation.ForUserID = userId;
                evaluation.SubmittedByID = row[5].ToString();
                evaluation.Submitted = row[6].ToString() == "True";

                DataRow project = executeQuery("SELECT Name FROM Projects WHERE ID='" + projectId + "'", "Projects").Tables["Projects"].Rows[0];
                evaluation.ProjectName = project.ItemArray[0].ToString();

                DataRow user = executeQuery("SELECT FirstName, LastName FROM AspNetUsers WHERE ID='" + userId + "'", "AspNetUsers").Tables["AspNetUsers"].Rows[0];
                evaluation.ForUserName = user.ItemArray[0] + " " + user.ItemArray[1];

                user = executeQuery("SELECT FirstName, LastName FROM AspNetUsers WHERE ID='" + evaluation.SubmittedByID + "'", "AspNetUsers").Tables["AspNetUsers"].Rows[0];
                evaluation.SubmittedByName = user.ItemArray[0] + " " + user.ItemArray[1];

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(row[2].ToString());
                XmlNode node = xmlDoc.FirstChild;
                foreach (XmlNode question in node.SelectNodes("question"))
                {
                    evaluation.Questions.Add(question.InnerText);
                }

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(row[3].ToString());
                node = xmlDoc.FirstChild;
                foreach (XmlNode answer in node.SelectNodes("answer"))
                {
                    evaluation.Answers.Add(answer.InnerText);
                }

                evaluation.AdditionalComments = row[7].ToString();

                evaluations.Add(evaluation);
            }
            return evaluations;
        }

        public void submitReview(int reviewId, List<string> Answers, string AdditionalComments)
        {
            //Submits data pertaining to a review.
            string answersXml = "<answers>";
            foreach (string answer in Answers)
            {
                answersXml += "<answer>";
                answersXml += answer;
                answersXml += "</answer>";
            }
            answersXml += "</answers>";

            SqlCommand command = new SqlCommand("UPDATE Evaluations SET Answers='" + answersXml +"', AdditionalComments='" + AdditionalComments + "', Submitted='1' WHERE ID='" + reviewId + "'", sqlCon);
            command.ExecuteNonQuery();
        }
    }
}
