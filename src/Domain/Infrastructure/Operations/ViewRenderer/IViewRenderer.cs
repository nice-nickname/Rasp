namespace Domain.Infrastructure;

public interface IViewRenderer : IDisposable
{
    Task<string> Render<TModel>(string partialName, TModel model);
}
