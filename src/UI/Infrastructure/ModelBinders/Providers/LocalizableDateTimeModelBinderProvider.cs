using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UI.Infrastructure.ModelBinders;

public class LocalizableDateTimeModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var modelType = context.Metadata.UnderlyingOrModelType;
        if (modelType == typeof(DateTime))
        {
            return new LocalizableDateTimeModelBinder(context.Metadata.IsReferenceOrNullableType);
        }

        return null;
    }
}
