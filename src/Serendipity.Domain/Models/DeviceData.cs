using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace Serendipity.Domain.Models;
public class DeviceData
{
    public int Serendipity { get; set; }
    public int StepsWalked { get; set; }
    public int Heartbeat { get; set; }
    public int NumberOfFalls { get; set; }
    public int Battery { get; set; }
    public int Standings { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public States State { get; set; }
}


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