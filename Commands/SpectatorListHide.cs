using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using NorthwoodLib.Pools;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoahsNationUtils.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SpectatorListHide : ICommand
    {
        public string Command => "spectatorlist";

        public string[] Aliases => new string[0];

        public string Description => "Hides the spectator list when playing.";

        public DateTime lastUsed { get; set; } = DateTime.MinValue;

        public float Cooldown { get; set; } = 30f;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string userId = Player.Get(sender).UserId;

            if (!NoahsNationUtils.SpectatorListHidden.Remove(userId))
            {
                NoahsNationUtils.SpectatorListHidden.Add(userId);

                response = "<color=red>Spectator List has been hidden!</color>";
                return true;
            }

            response = "<color=green>Spectator List has been shown!</color>";
            return true;
        }
    }
}
