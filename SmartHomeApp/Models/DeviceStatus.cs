using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class DeviceStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
