using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class ProjectMember
{
    public Guid? Id { get; set; }

    public int ProjectId { get; set; }

    public int MemberId { get; set; }

    public virtual User Member { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
