using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistica_Review.Models
{
    public class UserModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<ProjectModel> AssignedProjects { get; set; }
        public ICollection<ProjectModel> ManagedProjects { get; set; }
    }
}