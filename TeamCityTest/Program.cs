using System;
using System.Linq;
using TeamCitySharp;

namespace TeamCityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TeamCityClient("maui:90");
            client.Connect("restUser", "restUser");

            GetValue(client);

            Console.ReadLine();
        }

        private static void GetValue(ITeamCityClient client)
        {
            var listOfBuilds =
                client.BuildConfigs.All()
                    .Select(
                        x =>
                            new
                            {
                                Projekt = x.ProjectName,
                                BuildConfigName = x.Name,
                                Status = client.Builds.LastBuildByBuildConfigId(x.Id).Status
                            })
                    .ToList();

            foreach (var y in listOfBuilds)
                Console.WriteLine(y.Projekt + ": " + y.BuildConfigName + ": " + y.Status);
        }
    }
}
