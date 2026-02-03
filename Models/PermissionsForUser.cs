using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class PermissionsForUser
{
    public Guid? Id { get; set; }

    public int UserId { get; set; }

    public int PermissionId { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
