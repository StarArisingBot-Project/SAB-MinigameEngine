using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StarArisingBot.MinigameEngine
{
    /// <summary>
    /// Manager responsible for controlling the flow of minigames.
    /// </summary>
    public static class MinigameInstanceClient
    {
        /// <summary>
        /// Represents the instance of each loaded minigame module.
        /// </summary>
        public static List<MinigameInstance> MinigameInstances { get; private set; }

        private static DiscordClient Client { get; set; }

        //======================================================//

        /// <summary>
        /// Launch the minigame client.
        /// </summary>
        /// <param name="client">The current Discord Client connected.</param>
        public static async Task StartAsync(DiscordClient client)
        {
            Client = client;
            MinigameInstances = new List<MinigameInstance>();

            await Task.CompletedTask;
        }

        //======================================================//
        /// <summary>
        /// Get the active instance of a minigame.
        /// </summary>
        /// <param name="minigameModuleType">The name of the desired instance.</param>
        /// <returns>The required instance.</returns>
        public static async Task<MinigameInstance> GetInstanceAsync(Type minigameModuleType)
        {
            return await Task.FromResult(MinigameInstances.Where(x => x.MinigameModuleType == minigameModuleType).FirstOrDefault());
        }

        /// <summary>
        /// Register a minigame module on the instances.
        /// </summary>
        /// <typeparam name="T">A class that is a minigame module.</typeparam>
        public static void RegisterMinigameModule<T>() where T : MinigameModule
        {
            Type minigameModuleType = typeof(T);

            if (minigameModuleType.Name.EndsWith("Minigame"))
            {
                MinigameInstances.Add(new MinigameInstance(minigameModuleType));
            }
        }
    }
}
