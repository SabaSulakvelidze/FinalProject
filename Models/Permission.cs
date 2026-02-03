using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public virtual ICollection<PermissionsForUser> PermissionsForUsers { get; set; } = new List<PermissionsForUser>();
}
