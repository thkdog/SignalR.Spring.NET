using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Context;
using Spring.Context.Support;
using SignalR;

namespace SignalR.Spring
{
    public class SpringDependencyResolver : DefaultDependencyResolver
    {
        public SpringDependencyResolver(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
                throw new ArgumentNullException("applicationContext");
            _applicationContext = applicationContext;
        }

        public override object GetService(Type serviceType)
        {
            var springServicesDic = _applicationContext.GetObjectsOfType(serviceType);
            foreach (var p in springServicesDic.Values)
                return p;
            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var baseServices = base.GetServices(serviceType);
            var springServicesDic = _applicationContext.GetObjectsOfType(serviceType);
            IList<object> springServices = new List<object>(springServicesDic.Count);
            foreach (var p in springServicesDic.Values)
                springServices.Add(p);
            return springServices.Concat(base.GetServices(serviceType));
        }

        private readonly IApplicationContext _applicationContext = ContextRegistry.GetContext();
    }
}
