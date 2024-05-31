using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// below is necessary to avoid using exiled intercom
using Intercom = PlayerRoles.Voice.Intercom;
using PlayerRoles;
using PluginAPI.Events;
using VoiceChat;
using Exiled.API.Features;
using PlayerRoles.PlayableScps.Scp3114;
using MEC;
using PlayerRoles.Voice;
using VoiceChat.Playbacks;

namespace NoahsNationUtils.Patches
{
    [HarmonyPatch(typeof(Intercom), nameof(Intercom.CheckPlayer))]
    public class Scp3114IntercomPatch
    {
        public static bool Prefix(Intercom __instance, ReferenceHub hub, ref bool __result)
        {
            if (!Player.TryGet(hub.gameObject, out Player player)) return true;
            
            if (player.Role is Exiled.API.Features.Roles.Scp3114Role Scp3114) 
            {
                // checks if current disguise is not active, and therefore disallow intercom (normal function)
                if (Scp3114.Identity.CurIdentity.Status != Scp3114Identity.DisguiseStatus.Active)
                {
                    return true;
                }

                // carries on with code if reaches here (3114 AND disguised)
            }
            else
            {
                // any other role, normal check
                return true;
            }

            PlayerRoleBase currentRole = hub.roleManager.CurrentRole as Scp3114Role;

            bool distCheck = ((currentRole as Scp3114Role).FpcModule.Position - __instance._worldPos).sqrMagnitude < __instance._rangeSqr;

            __result = distCheck && (currentRole as Scp3114Role).VoiceModule.ServerIsSending && player.VoiceChannel == VoiceChatChannel.Proximity && !VoiceChatMutes.IsMuted(hub, true) 
                && EventManager.ExecuteEvent(new PlayerUsingIntercomEvent(hub, Intercom.State));

            return false;
        }
    }

    /* [HarmonyPatch(typeof(StandardVoiceModule), nameof(StandardVoiceModule.GlobalChatIcon), MethodType.Getter)]
    public class Scp3114IntercomIconPatch
    {
        public static bool Prefix(StandardVoiceModule __instance, ref GlobalChatIconType __result)
        {
            if (!Player.TryGet(__instance.Owner, out Player player)) return true;

            if (player.Role is Exiled.API.Features.Roles.Scp3114Role scp3114)
            {
                __result = GlobalChatIconType.Intercom;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(StandardVoiceModule), nameof(StandardVoiceModule.GlobalChatName), MethodType.Getter)]
    public class Scp3114IntercomNamePatch
    {
        public static bool Prefix(StandardVoiceModule __instance, ref string __result)
        {
            if (!Player.TryGet(__instance.Owner, out Player player)) return true;

            if (player.Role is Exiled.API.Features.Roles.Scp3114Role scp3114 && player.VoiceChannel == VoiceChatChannel.Proximity)
            {
                __result = scp3114.Disguise.Owner.name;
                return false;
            }

            return true;
        }
    } */
}
