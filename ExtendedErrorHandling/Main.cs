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
	[BepInPlugin("ExtendedErrorHandling", "ExtendedErrorHandling", "1.2")]
	public class Main : BaseUnityPlugin
	{

		internal static Main main;
		internal static ManualLogSource BepLogger;
		private void Awake()
		{
			BepLogger = Logger;
			main = this;

			Harmony.CreateAndPatchAll(typeof(MenuItemRedundancy));
			Harmony.CreateAndPatchAll(typeof(PresetErrorHandling));
			Harmony.CreateAndPatchAll(typeof(Serialization));
			Harmony.CreateAndPatchAll(typeof(ErrorTexturePlaceholder));

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += MenuItemRedundancy.DoOnTitleScreen;
		}
	}
}
