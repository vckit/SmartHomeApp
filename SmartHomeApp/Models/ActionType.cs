using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class ActionType
{
    public int ActionTypeId { get; set; }

    public string? ActionTypeName { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();
}
