using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class UserDevicePermission
{
    public int PermissionId { get; set; }

    public int? UserId { get; set; }

    public int? DeviceId { get; set; }

    public string? PermissionType { get; set; }

    public virtual Device? Device { get; set; }

    public virtual User? User { get; set; }
}
