using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Initialize
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider service)
        {

            using (var scope = service.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TreeContext>();
                await InitializeNodes.InitializeData(context);

            }
        }
    }
}
