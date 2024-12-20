using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Auradent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider? Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Register the database context
            services.AddDbContext<db_context>();

            // Register EF classes
            services.AddScoped<IdataHelper<Appointment>, AppointmentEF>();
            services.AddScoped<IdataHelper<DoctorandNurse>, DoctorandNurseEF>();





            Services = services.BuildServiceProvider();
        }
    }

}