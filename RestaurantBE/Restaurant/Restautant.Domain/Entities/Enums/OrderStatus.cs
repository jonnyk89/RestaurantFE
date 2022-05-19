using System.Runtime.Serialization;

namespace Restaurant.Domain.Entities.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Complete")]
        Complete,
    }
}
