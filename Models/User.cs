using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<PermissionsForUser> PermissionsForUsers { get; set; } = new List<PermissionsForUser>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<ProjectTask> ProjectTaskTaskAssignees { get; set; } = new List<ProjectTask>();

    public virtual ICollection<ProjectTask> ProjectTaskTaskIssuers { get; set; } = new List<ProjectTask>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
