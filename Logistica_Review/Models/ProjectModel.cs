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
        public UserModel Admin { get; set; }
        public ICollection<UserModel> Users { get; set; }
        public ICollection<EvaluationModel> Evaluations { get; set; }
    }
}