using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;


namespace Application.Quota.Common.Utils
{
    public static class GetEnvVariable
    {
        public static string Get(IHostingEnvironment env, string variable, IConfiguration Configuration = null)
        {
            string AWSConfigFile = @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration";
            if (File.Exists(AWSConfigFile))
            {
                var config = new ConfigurationBuilder()
                   .AddJsonFile(AWSConfigFile, optional: true, reloadOnChange: true).Build();

                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (IConfigurationSection pair in config.GetSection("iis:env").GetChildren())
                {
                    string[] keypair = pair.Value.Split(new[] { '=' }, 2);
                    dict.Add(keypair[0], keypair[1]);
                }
                return dict.ContainsKey(variable)
                        ? dict[variable]
                        : env.EnvironmentName;
            }
            else if (Configuration != null)
            {
                return Configuration.GetSection(variable).Value;
            }
            else
            {
                return env.EnvironmentName;
            }
        }
    }
}
