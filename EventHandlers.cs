using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp3114;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Intercom = PlayerRoles.Voice.Intercom;

namespace NoahsNationUtils
{
    public class EventHandlers
    {
       // Handles showing a hint to the player when they look at 096
        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (NoahsNationUtils.Instance.Config.EnableTargetMessage)
            {
                ev.Target.ShowHint(NoahsNationUtils.Instance.Config.TargetMessage, NoahsNationUtils.Instance.Config.TargetMessageDuration);
            }
        }

        // Handles showing a hint to the player when they are the last alive on their team
        public void OnPlayerDying(DyingEventArgs ev)
        {
            if (ev.Player == null || ev.Attacker == null) return;

            if (NoahsNationUtils.Instance.Config.EnableLastPlayerOnTeamMessage)
            {
                List<Player> team = Player.Get(ev.Player.Role.Team).ToList();
                if (team.Count - 1 == 1)
                {
                    if (team[0] == ev.Player)
                    {
                        team[1].ShowHint(NoahsNationUtils.Instance.Config.LastPlayerMessage, NoahsNationUtils.Instance.Config.LastPlayerMessageDuration);
                    }
                    else
                    {
                        team[0].ShowHint(NoahsNationUtils.Instance.Config.LastPlayerMessage, NoahsNationUtils.Instance.Config.LastPlayerMessageDuration);
                    }
                }
            }

            // Handles Vaporizing when dying to Micro
            if (NoahsNationUtils.Instance.Config.EnableMicroVaporizing)
            {
                if (ev.DamageHandler.Type == DamageType.MicroHid)
                {
                    ev.Player.DropItems();
                    ev.Player.Vaporize();
                }
            }

        }

        private bool speaking { get; set; } = false;

        // sets voice chat channel for scp 3114 when speaking on intercom
        public void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            Log.Info("Speaking on IC");
            speaking = true;

            if (ev.Player.Role != RoleTypeId.Scp3114) return;

            Log.Info("Is 3114 & set to intercom vc");

            ToggleGlobalIntercom(ev.Player);

            Timing.RunCoroutine(CheckForNotSpeaking(ev.Player), "Check For Not Speaking");
        }

        public IEnumerator<float> CheckForNotSpeaking(Player player)
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1f);

                Log.Info("Speaking");
                if (Intercom.State == PlayerRoles.Voice.IntercomState.Cooldown)
                {
                    speaking = false;
                }

                if (!speaking)
                {
                    ToggleGlobalIntercom(player);
                    yield break;
                }
            }
        }

        public void ToggleGlobalIntercom(Player player)
        {
            // toggle global vc for user
            if (!Intercom.TrySetOverride(player.ReferenceHub, !Intercom.HasOverride(player.ReferenceHub)))
            {
                Log.Info("Failed to set override flags. User or intercom is null.");
            }
        }
    }
}
