using System.Runtime.Serialization;

namespace Restaurant.Domain.Entities.Enums
{
    public enum StatusOptions
    {
        [EnumMember(Value = "All")]
        All,
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Complete")]
        Complete,
    }
}
