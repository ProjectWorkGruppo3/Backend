using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Providers;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class AlarmsService : IAlarmsService
{
    private readonly IAlarmsRepository _alarmsRepository;
    private readonly IEmailProvider _emailProvider;
    private readonly IUserRepository _userRepository;
    private readonly IDeviceRepository _deviceRepository;

    public AlarmsService(IAlarmsRepository alarmsRepository, IEmailProvider emailProvider, IUserRepository userRepository, IDeviceRepository deviceRepository)
    {
        _alarmsRepository = alarmsRepository;
        _emailProvider = emailProvider;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
    }

    public async Task<Result<IEnumerable<Alarm>>> GetLatest(string userEmail)
    {
        return await _alarmsRepository.GetLatest(userEmail);
    }

    public async Task<IResult> Insert(Alarm alarm)
    {
        try
        {
            await _alarmsRepository.Insert(alarm);


            var toRecipients = await _userRepository.GetUserEmergencyContactsFromDeviceId(Guid.Parse(alarm.DeviceId));
            var destinations = toRecipients.ToList();

            var device = await _deviceRepository.GetDevice(Guid.Parse(alarm.DeviceId));
            
            if (destinations.Count != 0)
            {
                return await _emailProvider.SendAlarmEmail(
                    destinations,
                    GetTitleFromAlarm(alarm.Type),
                    GetMessageFromAlarm(alarm, device.Name), 
                    alarm.DeviceId,
                    alarm.Timestamp
                );
            }
            

            return new SuccessResult();
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }

    private string GetTitleFromAlarm(string alarmType)
    {
        switch (alarmType)
        {
            case Alarm.BatteryType:
                return $"Low Battery";
            case Alarm.FallType:
                return "Device dropped";
            case Alarm.HeartBeatType:
                return "Danger Heart Rate";
            default:
                return "General Alarm";
        }
    }
    private string GetMessageFromAlarm(Alarm alarm, string deviceName)
    {
        switch (alarm.Type)
        {
            case Alarm.BatteryType:
                var battery = ((LowBatteryAlarm) alarm).BatteryCharge;
                return $"{deviceName} {battery}% of battery remaining";
            case Alarm.FallType:
                return $"{deviceName} dropped to the ground";
            case Alarm.HeartBeatType:
                var heartbeatValue = ((HeartBeatAlarm) alarm).HeartBeat;
                return $"{deviceName} registered {heartbeatValue} BPM";
            default:
                return $"{deviceName} generated an unknown error";
        }
    }
}