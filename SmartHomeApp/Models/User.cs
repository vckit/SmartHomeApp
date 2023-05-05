using System;
using System.Collections.Generic;

namespace SmartHomeApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<UserDevicePermission> UserDevicePermissions { get; set; } = new List<UserDevicePermission>();
}
