using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using Scp096 = Exiled.Events.Handlers.Scp096;
using PlayerRoles.Spectating;
using System;
using Exiled.Events.Handlers;
using System.Collections.Generic;
using HarmonyLib;
using MEC;

namespace NoahsNationUtils
{
    public class NoahsNationUtils : Plugin<Config>
    {
        public override string Author => "noah / punishn";
        public override string Name => "NoahsNationUtils";
        public override string Prefix => Name;

        public static NoahsNationUtils Instance;

        public EventHandlers _handlers;

        private Harmony _harmony;

        private string HarmonyId { get; } = "punish.dev";

        public override void OnEnabled()
        {
            Instance = this;

            RegisterEvents();
            RegisterPatch();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;

            UnregisterEvents();
            UnregisterPatch();

            base.OnDisabled();
        }
        private void RegisterEvents()
        {
            _handlers = new EventHandlers();

            Scp096.AddingTarget += _handlers.OnAddingTarget;
            Player.Dying += _handlers.OnPlayerDying;
            Player.IntercomSpeaking += _handlers.OnIntercomSpeaking;
        }

        private void UnregisterEvents()
        {
            Scp096.AddingTarget -= _handlers.OnAddingTarget;
            Player.Dying -= _handlers.OnPlayerDying;
            Player.IntercomSpeaking -= _handlers.OnIntercomSpeaking;

            _handlers = null;
        }

        private void RegisterPatch()
        {
            try
            {
                _harmony = new(HarmonyId);
                _harmony.PatchAll();
            }
            catch (HarmonyException ex)
            {
                Log.Error($"[RegisterPatch] Patching Failed : {ex}");
            }
        }

        private void UnregisterPatch()
        {
            _harmony.UnpatchAll();
            _harmony = null;
        }
    }
}
