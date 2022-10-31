using LinkNet.Infrastructure.Data;
using LinkNet.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Infrastructure.Seeders
{
    public class Seeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<LinkNetAPIContext>();

                context.Database.EnsureCreated();

                AddData<ApplicationUser>(context, DataConstants.Users);
                AddData<Post>(context, DataConstants.Posts);
                AddData<Occupation>(context, DataConstants.Occupations);
                AddData<Story>(context, DataConstants.Stories);
            }
        }

        private static void AddData<T>(LinkNetAPIContext context, string fileName)
            where T : class
        {
            if (!context.Set<T>().Any())
            {
                var data = new List<T>();

                using (var reader = new StreamReader(string.Format(DataConstants.Path, fileName)))
                {
                    data = JsonConvert.DeserializeObject<List<T>>(reader.ReadToEnd());
                }

                context.Set<T>().AddRange(data);
                context.SaveChanges();
            }
        }
    }
}
