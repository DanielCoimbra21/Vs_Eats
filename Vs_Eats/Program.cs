using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Vs_Eats
{
    public class Program
    {

        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();


        public static void Main(string[] args)
        {
           
        }
    }
}
