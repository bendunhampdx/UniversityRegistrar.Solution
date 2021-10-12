using System.Collections.Generic;
using System;

namespace University.Models
{
  public class Department
  {
    public Department ()
    {
      this.StudentJoinEntities = new HashSet<DepartmentStudent>();
      this.CourseJoinEntities = new HashSet<CourseDepartment>();
    }

    public int DepartmentId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<DepartmentStudent> StudentJoinEntities { get; set; }
    public virtual ICollection<CourseDepartment> CourseJoinEntities { get; set; }

  }
}