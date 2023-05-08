using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UI.Infrastructure.ModelBinders;

public class LocalizableDateTimeModelBinder : IModelBinder
{
    private readonly bool _isNullable;

    public LocalizableDateTimeModelBinder(bool isNullable)
    {
        this._isNullable = isNullable;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        var metadata = bindingContext.ModelMetadata;
        var type = metadata.UnderlyingOrModelType;

        if (!this._isNullable && valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var modelState = bindingContext.ModelState;
        modelState.SetModelValue(modelName, valueProviderResult);

        try
        {
            var value = valueProviderResult.FirstValue;

            object? model;
            if (string.IsNullOrWhiteSpace(value))
            {
                model = null;
            }
            else
            {
                model = DateTime.Parse(value, CultureInfo.CurrentCulture);
            }
            bindingContext.Result = ModelBindingResult.Success(model);
        }
        catch (Exception exception)
        {
            modelState.TryAddModelError(modelName, exception, metadata);
        }

        return Task.CompletedTask;
    }
}
