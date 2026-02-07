using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class Project
{
    public int Id { get; set; }

    public string ProjectName { get; set; } = null!;

    public int ProjectManagerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User ProjectManager { get; set; } = null!;

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
}
