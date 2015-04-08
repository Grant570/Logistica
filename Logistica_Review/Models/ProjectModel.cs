using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistica_Review.Models
{
    public class ProjectModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Admin { get; set; }
        public List<UserModel> Users { get; set; }
        public List<EvaluationModel> Evaluations { get; set; }

        public ProjectModel()
        {
            Users = new List<UserModel>();
            Evaluations = new List<EvaluationModel>();
        }
    }

}