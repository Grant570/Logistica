using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistica_Review.Models
{
    public class EvaluationModel
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }
        public string ForUserID { get; set; }
        public string ForUserName { get; set; }
        public string SubmittedByID { get; set; }
        public string SubmittedByName { get; set; }
        public string AdditionalComments { get; set; }
        public bool Submitted { get; set; }

        public EvaluationModel()
        {
            Questions = new List<string>();
            Answers = new List<string>();
        }
    }
}