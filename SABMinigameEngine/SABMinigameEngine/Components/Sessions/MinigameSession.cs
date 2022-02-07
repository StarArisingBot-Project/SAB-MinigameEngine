using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarArisingBot.MinigameEngine
{
    /// <summary>
    /// Represents a minigame that is running.
    /// </summary>
    public sealed class MinigameSession
    {
        /// <summary>
        /// The ID of the current session.
        /// </summary>
        public ulong SectionID { get; internal set; }

        /// <summary>
        /// The context in which the session is running.
        /// </summary>
        public CommandContext Context { get; private set; }

        /// <summary>
        /// The date this session was created.
        /// </summary>
        public DateTime CreationTimestamp { get; private set; }

        /// <summary>
        /// Current session information.
        /// </summary>
        public MinigameSessionInfos SessionInfos { get; private set; }

        internal MinigameModule MinigameModule { get; private set; }
        internal MinigameInstance CurrentInstance { get; private set; }
        //======================================//

        public delegate void SessionDisconnected();

        /// <summary>
        /// Invoked if the session is disconnected.
        /// </summary>
        public event SessionDisconnected OnSessionDisconnected;

        //======================================//

        internal MinigameSession(CommandContext context, MinigameInstance currentInstance, MinigameModule minigameModule, MinigameSessionBuilder sessionBuilder = null)
        {
            Context = context;
            CurrentInstance = currentInstance;
            MinigameModule = minigameModule;
            MinigameModule.Session = this;

            CreationTimestamp = DateTime.Now;

            if(sessionBuilder == null)
            {
                SessionInfos = new MinigameSessionInfos();
            }
            else
            {
                SessionInfos = sessionBuilder.ToSessionInfos();
            }
        }
        internal async Task DisconnectAsync()
        {
            MinigameModule.StartCancelProcess();
            await CurrentInstance.RemoveSessionAsync(SectionID);
        }
    }
}
