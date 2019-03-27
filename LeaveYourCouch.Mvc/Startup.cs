using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using LeaveYourCouch.Mvc.Business;
using LeaveYourCouch.Mvc.Business.Services;
using LeaveYourCouch.Mvc.Business.Services.Events;
using LeaveYourCouch.Mvc.Business.Services.Users;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(LeaveYourCouch.Mvc.Startup))]
namespace LeaveYourCouch.Mvc
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Options.DefaultLifestyle = Lifestyle.Scoped;
            container.RegisterInstance<IAppBuilder>(app);

            //container.Register<ApplicationUserManager>(Lifestyle.Scoped);
            //container.Register<ApplicationSignInManager>(Lifestyle.Scoped);

            container.Register<IApiHelper, ApiHelper>();
            container.Register<IRelationsManager, RelationsManager>();
            container.Register<IEventsBuilder, EventsBuilder>();
            container.Register<IImageHelper, ImageHelper>();
            container.Register<IViewBagMessageFactory, ViewBagMessageFactory>();
            container.Register<IDbContextFactory<ApplicationDbContext>>(() => new MigrationsContextFactory(container));

            container.Register<ApplicationDbContext>(() => new ApplicationDbContext("DefaultConnection"));
            container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(container.GetInstance<ApplicationDbContext>()));
            container.RegisterInitializer<ApplicationUserManager>(manager => InitializeUserManager(manager, app));
            container.Register<SignInManager<ApplicationUser, string>, ApplicationSignInManager>();

            container.Register<IAuthenticationManager>(() => container.IsVerifying ? new OwinContext(new Dictionary<string, object>()).Authentication : HttpContext.Current.GetOwinContext().Authentication);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify(VerificationOption.VerifyAndDiagnose);

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            
            ConfigureAuth(app, container);


        }

        private void InitializeUserManager(ApplicationUserManager manager, IAppBuilder app)
        {
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = app.GetDataProtectionProvider();
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}
