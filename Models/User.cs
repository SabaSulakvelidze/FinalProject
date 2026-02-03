using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<PermissionsForUser> PermissionsForUsers { get; set; } = new List<PermissionsForUser>();
}
