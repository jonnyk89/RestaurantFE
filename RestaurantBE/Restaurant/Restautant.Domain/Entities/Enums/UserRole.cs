using System.Runtime.Serialization;

namespace Restaurant.Domain.Entities.Enums
{
    public enum UserRole
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "Waiter")]
        Waiter
    }
}
