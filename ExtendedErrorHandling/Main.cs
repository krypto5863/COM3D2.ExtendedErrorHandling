using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using UnityEngine.SceneManagement;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ExtendedErrorHandling
{
    [BepInPlugin("ExtendedErrorHandling", "ExtendedErrorHandling", "1.1")]
    public class Main : BaseUnityPlugin
    {

        internal static Main main;
        internal static ManualLogSource BepLogger;
        private void Awake() 
        {
            Harmony.CreateAndPatchAll(typeof(MenuItemRedundancy));
            Harmony.CreateAndPatchAll(typeof(PresetErrorHandling));
            Harmony.CreateAndPatchAll(typeof(Serialization));
            Harmony.CreateAndPatchAll(typeof(ErrorTexturePlaceholder));

            BepLogger = Logger;

            main = this;

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += MenuItemRedundancy.DoOnTitleScreen;
        }
    }
}
