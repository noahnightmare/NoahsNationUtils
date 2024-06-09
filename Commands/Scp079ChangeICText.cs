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
    public class Scp079ChangeICText : ICommand
    {
        public string Command => "intercomtext";

        public string[] Aliases => ["ictext", "icomtext"];

        public string Description => "Changes the text on Intercom as 079.";

        public DateTime lastUsed { get; set; } = DateTime.MinValue;

        public float Cooldown { get; set; } = 30f;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player.Role.Type != RoleTypeId.Scp079)
            {
                response = "You are not currently Scp 079.";
                return false;
            }

            if (arguments.Count() < 1) 
            {
                response = "Please enter some text to display on the intercom.";
                return false;
            }

            if (DateTime.Now < lastUsed + TimeSpan.FromSeconds(Cooldown)) 
            {
                response = $"You are on cooldown! Please wait {(int)(Cooldown - (DateTime.Now - lastUsed).TotalSeconds)} seconds to use the command.";
                return false;
            }

            Intercom.DisplayText = $"SCP-079 : {FormatArguments(arguments, 0)}"; // sets intercom text to whatever is used after the command
            lastUsed = DateTime.Now;
            response = "Text successfully set.";
            return true;
        }

        private static string FormatArguments(ArraySegment<string> sentence, int index)
        {
            StringBuilder SB = StringBuilderPool.Shared.Rent();
            foreach (string word in sentence.Segment(index))
            {
                SB.Append(word);
                SB.Append(" ");
            }
            string msg = SB.ToString();
            StringBuilderPool.Shared.Return(SB);
            return msg;
        }
    }
}
