using Autofac;
using System;

namespace Taylor.ConsoleApp
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
        }

        public static void RunApp()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                //var writer = scope.Resolve<IDateWriter>();
                //writer.WriteDate();
            }
        }

        public void InitContainer()
        {
            var builder = new ContainerBuilder();

            //// Register individual components
            //builder.RegisterInstance(new TaskRepository())
            //       .As<ITaskRepository>();
            //builder.RegisterType<TaskController>();
            //builder.Register(c => new LogManager(DateTime.Now))
            //       .As<ILogger>();

            //// Scan an assembly for components
            //builder.RegisterAssemblyTypes(myAssembly)
            //       .Where(t => t.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces();

            Container = builder.Build();
        }
    }
}
