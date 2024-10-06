using Exiled.API.Interfaces;
using System.ComponentModel;

namespace NoahsNationUtils
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Last Player Alive On Your Team Configs")]
        public bool EnableLastPlayerOnTeamMessage { get; set; } = true;
        public float LastPlayerMessageDuration { get; set; } = 4f;
        public string LastPlayerMessage{ get; set; } = "<b><color=red>You are the last player alive of your team!</color></b>";

        [Description("Target of Scp 096 hint Configs")]
        public bool EnableTargetMessage { get; set; } = true;
        public float TargetMessageDuration { get; set; } = 4f;
        public string TargetMessage { get; set; } = "<b><color=red>You became a target of 096!</color></b>";

        [Description("Micro Vaporize Configs")]
        public bool EnableMicroVaporizing {  get; set; } = true;

        [Description("Spectator List Configs")]
        public float RefreshRate { get; set; } = 2;

        public string FullText { get; set; } = "<size=23><align=right>%display%</size><voffset=900> </voffset></align>";
        public string PlayerDisplay { get; set; } = "%name%";
        public string NoSpectators { get; set; } = "👥 Spectators (0)";
        public string Spectators { get; set; } = "👥 Spectators (%amount%)";
    }
}