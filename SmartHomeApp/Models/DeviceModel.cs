using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class DeviceModel
{
    public int ModelId { get; set; }

    public int? ManufacturerId { get; set; }

    public string? ModelName { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual Manufacturer? Manufacturer { get; set; }
}
