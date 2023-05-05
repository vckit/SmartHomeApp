using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class Action
{
    public int ActionId { get; set; }

    public int? DeviceId { get; set; }

    public int? UserId { get; set; }

    public int? ActionTypeId { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual ActionType? ActionType { get; set; }

    public virtual Device? Device { get; set; }

    public virtual User? User { get; set; }
}
