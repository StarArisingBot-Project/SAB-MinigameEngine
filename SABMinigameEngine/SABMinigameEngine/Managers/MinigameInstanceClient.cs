using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SABMinigamesEngine
{
    /// <summary>
    /// Manager responsible for controlling the flow of minigames.
    /// </summary>
    public static class MinigameInstanceClient
    {
        /// <summary>
        /// Represents the instance of each loaded minigame module.
        /// </summary>
        public static MinigameInstance[] MinigameInstances { get; private set; }

        private static Assembly CurrentAssembly { get; set; }
        private static DiscordClient Client { get; set; }

        //======================================================//

        /// <summary>
        /// Launch the minigame client.
        /// </summary>
        /// <param name="client">The current Discord Client connected.</param>
        public static async Task StartAsync(DiscordClient client)
        {
            Client = client;
            CurrentAssembly = typeof(MinigameInstanceClient).Assembly;

            await GetMinigamesInstances();
            await StartMinigamesModules();

            //==========================//
            MinigameConsoleDebug();
        }

        private static void MinigameConsoleDebug()
        {
            string minigames = "";
            foreach (MinigameInstance minigame in MinigameInstances)
            {
                minigames += $"> {minigame.MinigameModule.Name} \n";
            }

            Console.WriteLine($"\n=================\n" +

                              $"INSTANCES LOADED: \n\n" +

                              $"{minigames}" +

                              $"\n=================\n");
        }
        private static Task GetMinigamesInstances()
        {
            List<MinigameInstance> instances = new();
            foreach (Type minigame in CurrentAssembly.GetTypes().Where(x => x.IsSubclassOf(typeof(MinigameModule))))
            {
                if (minigame.Name.EndsWith("Minigame"))
                {
                    instances.Add(new MinigameInstance(minigame));
                }
            }

            MinigameInstances = instances.ToArray();
            return Task.CompletedTask;
        }
        private static Task StartMinigamesModules()
        {
            return Task.CompletedTask;
        }

        //======================================================//
        /// <summary>
        /// Get the active instance of a minigame.
        /// </summary>
        /// <param name="minigameModuleType">The name of the desired instance.</param>
        /// <returns>The required instance.</returns>
        public static async Task<MinigameInstance> GetInstanceAsync(Type minigameModuleType)
        {
            return await Task.FromResult(MinigameInstances.Where(x => x.MinigameModule == minigameModuleType).FirstOrDefault());
        }
    }
}
