using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Mvc;

namespace Rasp.Controllers
{
    public class Dispatcher : DispatcherControllerBase
    {
        public Dispatcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
