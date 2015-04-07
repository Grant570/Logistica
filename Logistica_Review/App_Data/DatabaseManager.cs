using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Logistica_Review.Classes;

namespace Logistica_Review.Database
{
    public class DatabaseManager
    {
        private SqlConnection sqlCon;

        public DatabaseManager() {
            SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            sqlCon.Open();
        }

        public List<Project> getProjects(long userId) {
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter("SELECT * FROM Projects", sqlCon);
            sqlAdapter.Fill(dataSet, "Projects");
            DataTable table = dataSet.Tables["Projects"];

            List<Project> projects = new List<Project>();
            foreach(DataRow row in table.Rows) {
                Project project = new Project();
                project.Id = Convert.ToInt32(row.ItemArray[0]);
                project.Name = row.ItemArray[1].ToString();
            }

            return projects;
        }
    }
}
