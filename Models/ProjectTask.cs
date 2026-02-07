using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class ProjectTask
{
    public int Id { get; set; }

    public string TaskName { get; set; } = null!;

    public string TaskDescription { get; set; } = null!;

    public int ProjectId { get; set; }

    public int TaskIssuerId { get; set; }

    public int TaskAssigneeId { get; set; }

    public string TaskStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime DeadLine { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User TaskAssignee { get; set; } = null!;

    public virtual User TaskIssuer { get; set; } = null!;
}
