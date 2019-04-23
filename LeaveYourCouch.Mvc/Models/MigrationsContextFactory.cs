using System.Data.Entity.Infrastructure;
using SimpleInjector;

namespace LeaveYourCouch.Mvc.Models
{
    public class MigrationsContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        private readonly Container _container;

        public MigrationsContextFactory(Container container)
        {
            _container = container;
        }
        public ApplicationDbContext Create()
        {
            return _container.GetInstance<ApplicationDbContext>();
        }
    }
}