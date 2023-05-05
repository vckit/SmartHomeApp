using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class SecurityEvent
{
    public int EventId { get; set; }

    public int? DeviceId { get; set; }

    public int? EventTypeId { get; set; }

    public DateTime? EventDate { get; set; }

    public virtual Device? Device { get; set; }

    public virtual EventType? EventType { get; set; }
}
