using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serendipity.Domain.Models;

namespace Serendipity.WebApi.ModelBinders;

public class AlarmModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var bodyStream = bindingContext.ActionContext.HttpContext.Request.Body;
        var jObject = await JsonSerializer.DeserializeAsync<JsonObject>(bodyStream);

        if (jObject is null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }
        
        
        string? type = jObject.Root["type"]?.GetValue<string>();

        if (type is null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        Alarm? obj = type switch
        {
            Alarm.FallType => JsonSerializer.Deserialize<FallAlarm>(bodyStream),
            Alarm.BatteryType => JsonSerializer.Deserialize<LowBatteryAlarm>(bodyStream),
            Alarm.HeartBeatType => JsonSerializer.Deserialize<HeartBeatAlarm>(bodyStream),
            _ => null
        };

        if (obj is null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }
        
        bindingContext.Result = ModelBindingResult.Success(obj);
    }
}