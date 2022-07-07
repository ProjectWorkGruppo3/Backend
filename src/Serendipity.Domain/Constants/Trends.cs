using System.Runtime.Serialization;

namespace Serendipity.Domain.Constants;

public enum Trends
{
    [EnumMember(Value = nameof(Up))]
    Up,
    [EnumMember(Value = nameof(Down))]
    Down,
    [EnumMember(Value = nameof(Equal))]
    Equal
}