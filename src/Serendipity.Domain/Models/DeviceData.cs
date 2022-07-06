using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Serendipity.Domain.Constants;

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


