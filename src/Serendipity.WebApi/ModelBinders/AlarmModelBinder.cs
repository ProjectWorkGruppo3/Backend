using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serendipity.Domain.Models;

namespace Serendipity.WebApi.ModelBinders;

public class AlarmModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));
        
        try
        {

            var bodyStream = bindingContext.ActionContext.HttpContext.Request.Body;
            var jObject = await JsonSerializer.DeserializeAsync<JsonObject>(bodyStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (jObject is null)
            {
                throw new Exception();
            }
        
        
            string? type = jObject.Root["type"]?.GetValue<string>();

            if (type is null)
            {
                throw new Exception();
            }

        
            Alarm? obj = type switch
            {
                Alarm.FallType => new FallAlarm
                {
                    Type = type, 
                    Timestamp = DateTimeOffset.Parse(jObject["Timestamp"].ToString()),
                    DeviceId = jObject["DeviceId"].ToString()
                },
                Alarm.BatteryType => new LowBatteryAlarm
                {
                    Type = type, 
                    Timestamp = DateTimeOffset.Parse(jObject["Timestamp"].ToString()),
                    DeviceId = jObject["DeviceId"].ToString(),
                    BatteryCharge = int.Parse(jObject["BatteryCharge"].ToString())
                },
                Alarm.HeartBeatType => new HeartBeatAlarm
                {
                    Type = type, 
                    Timestamp = DateTimeOffset.Parse(jObject["Timestamp"].ToString()),
                    DeviceId = jObject["DeviceId"].ToString(),
                    HeartBeat = int.Parse(jObject["HeartBeat"].ToString())
                },
                _ => null
            };

            if (obj is null)
            {
                throw new Exception();
            }
            
            bindingContext.Result = ModelBindingResult.Success(obj);
        }
        catch (Exception)
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
        
        
    }
}