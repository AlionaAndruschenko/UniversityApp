using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApp.Models
{
    public class StudentGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
    }
}
