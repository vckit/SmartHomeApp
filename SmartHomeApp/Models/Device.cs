using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class Device
{
    public int DeviceId { get; set; }

    public int? ModelId { get; set; }

    public string? Location { get; set; }

    public DateTime? InstallationDate { get; set; }

    public int? StatusId { get; set; }

    public DateTime? LastMaintenance { get; set; }

    public decimal? SubscriptionPrice { get; set; }

    public int? SubscriptionPeriod { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<DeviceDatum> DeviceData { get; set; } = new List<DeviceDatum>();

    public virtual DeviceModel? Model { get; set; }

    public virtual ICollection<SecurityEvent> SecurityEvents { get; set; } = new List<SecurityEvent>();

    public virtual DeviceStatus? Status { get; set; }

    public virtual ICollection<UserDevicePermission> UserDevicePermissions { get; set; } = new List<UserDevicePermission>();
    
}
