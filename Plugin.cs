using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace Booling2
{
    [BepInPlugin(PluginInfo.modGUID, PluginInfo.modName, PluginInfo.modVersion)]
    public class Booling2 : BaseUnityPlugin
    {

        private readonly Harmony harmony = new Harmony(PluginInfo.modGUID);

        private static Booling2 instance;

        public static class PluginInfo
        {
            public const string modGUID = "Virus.Booling2";
            public const string modName = "Booling2";
            public const string modVersion = "1.0.0.0";
        }


        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.modGUID);

            Log.Log((BepInEx.Logging.LogLevel)16, "BOOLING loaded!");

            if (instance == null)
            {
                instance = this;
            }

            harmony.PatchAll(typeof(Booling2).Assembly);
        }

    }
}
