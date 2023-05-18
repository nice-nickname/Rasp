using Incoding.Web.MvcContrib;

namespace UI.Controllers;

public class Dispatcher : DispatcherControllerBase
{
    public Dispatcher(IServiceProvider serviceProvider)
            : base(serviceProvider)
    {
    }
}
