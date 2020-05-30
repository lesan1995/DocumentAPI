using Autofac;
using DocumentAPI.Abstract;
using DocumentAPI.Concrete;

namespace DocumentAPI.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        private string ConnectionString { get; }

        public ApplicationModule(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryRepo>()
                .As<ICategoryRepo>()
                .WithParameter("connectionString", ConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<DocumentRepo>()
                .As<IDocumentRepo>()
                .WithParameter("connectionString", ConnectionString)
                .InstancePerLifetimeScope();
        }
    }
}
