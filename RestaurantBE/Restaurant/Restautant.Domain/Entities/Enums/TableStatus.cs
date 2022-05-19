using System.Runtime.Serialization;

namespace Restaurant.Domain.Entities.Enums
{
    public enum TableStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Free")]
        Free,
    }
}
