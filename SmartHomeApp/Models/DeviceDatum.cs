using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class DeviceDatum
{
    public int DataId { get; set; }

    public int? DeviceId { get; set; }

    public DateTime? DateRecorded { get; set; }

    public string? Data { get; set; }

    public virtual Device? Device { get; set; }
}
