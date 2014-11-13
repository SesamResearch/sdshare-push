using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace SdShare.Service.AspNetWebApi
{
    public class SdShareWebApiAssemblyResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly> {typeof (DataSinkController).Assembly};
        }
    }
}
