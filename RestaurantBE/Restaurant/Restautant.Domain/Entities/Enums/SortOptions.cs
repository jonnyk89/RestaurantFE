using System.Runtime.Serialization;

namespace Restaurant.Domain.Entities.Enums
{
    public enum SortOptions
    {
        [EnumMember(Value = "Ascending")]
        Ascending,
        [EnumMember(Value = "Descending")]
        Descending,
    }
}
