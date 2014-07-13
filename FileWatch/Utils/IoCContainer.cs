using System.Reflection;
using Autofac;

namespace FileWatch.Utils
{
    public interface IObjectFactory
    {
        T GetInstance<T>();
    }

    public class IoCContainer : IObjectFactory
    {
        private static IContainer Container { get; set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            Container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public T GetInstance<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
