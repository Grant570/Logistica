using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistica_Review.Models
{
    public class EvaluationModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectModel Project { get; set; }
        public ICollection<string> Questions { get; set; }
        public ICollection<int> Answers { get; set; }
    }
}