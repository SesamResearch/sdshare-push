using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace SdShare.Service.AspNetWebApi
{
    public class SdShareWebApiAssemblyResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            //var baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //var controllersAssembly = typeof(DataSinkController).Assembly;
            //baseAssemblies.Add(controllersAssembly);
            //return baseAssemblies;

            return new List<Assembly> {typeof (DataSinkController).Assembly};
        }
    }
}
