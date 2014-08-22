#region

using System;
using System.Linq;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

#endregion

namespace TeamCityTest
{
    internal class CurrentBuildStatus
    {
        public string Projekt { get; set; }
        public string BuildConfigName { get; set; }
        public string Status { get; set; }
        public string BuildNumber { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new TeamCityClient("maui:90");
            client.Connect("restUser", "restUser");

            for (var i = 0; i < 1000; i++)
                GetValue(client);


            Console.ReadLine();
        }

        private static void GetValue(ITeamCityClient client)
        {
            var runningBuilds = client.Builds.ByBuildLocator(BuildLocator.RunningBuilds());

            var listOfBuilds =
                client.BuildConfigs.All()
                    .Select(
                        delegate(BuildConfig x)
                        {
                            var lastBuildByBuildConfigId = client.Builds.LastBuildByBuildConfigId(x.Id);
                            var returnObject = new CurrentBuildStatus
                            {
                                Projekt = x.ProjectName,
                                BuildConfigName = x.Name,
                                Status = lastBuildByBuildConfigId.Status,
                                BuildNumber = lastBuildByBuildConfigId.Number,
                            };
                            if (runningBuilds.Any(z => z.BuildTypeId == lastBuildByBuildConfigId.BuildTypeId))
                                returnObject.Status = "RUNNING";
                            return returnObject;
                        })
                    .ToList();


            foreach (var y in listOfBuilds)
                Console.WriteLine(y.Projekt + ": " + y.BuildConfigName + ": " + y.Status + " (" + y.BuildNumber + ")");
        }
    }
}