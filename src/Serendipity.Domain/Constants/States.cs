using System.Runtime.Serialization;

namespace Serendipity.Domain.Constants;

public enum States
{
    [EnumMember(Value = nameof(Running))]
    Running,
    [EnumMember(Value = nameof(Sleeping))]
    Sleeping,
    [EnumMember(Value = nameof(Walking))]
    Walking,
    [EnumMember(Value = "Sitting")]
    Sitting
}