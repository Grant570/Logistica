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

            SqlCommand command = new SqlCommand("INSERT INTO Projects (Name, Description, Admin, DueDate, Users, Evaluations) VALUES ('" + projectName + "', '" + projectDescription + 
                                                                                                                             "', '" + adminId + "', '" + dueDate + "', '" + usersXml +
                                                                                                                             "', '" + questionsXml + "')", sqlCon);
            command.ExecuteNonQuery();
        }

        public List<ProjectModel> getAssignedProjects(string userId)
        {
            //Get projects the user is currently assigned to.
            DataTable table = executeQuery("SELECT * FROM Projects", "Projects").Tables["Projects"];

            List<ProjectModel> projects = new List<ProjectModel>();
            foreach (DataRow row in table.Rows)
            {
                if (row.ItemArray[4].ToString().Contains(userId))
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

        public void addUser(string userEmail, int projectId) {
            //Adds the specified user to a project.
        }

        public void removeUser(string userEmail, int projectId)
        {
            //Removes the specified user from a project.
        }

        public void getReviews(int userId)
        {
            //Gets list of reviews about the user.
        }

        public void submitReview(int reviewId, List<String> Answers)
        {
            //Submits data pertaining to a review.
        }
    }
}
