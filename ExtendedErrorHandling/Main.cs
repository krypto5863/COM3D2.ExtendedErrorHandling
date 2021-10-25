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
using System.Windows.Forms;
using UnityEngine.SceneManagement;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ExtendedErrorHandling
{
	[BepInPlugin("ExtendedErrorHandling", "ExtendedErrorHandling", "1.3")]
	public class Main : BaseUnityPlugin
	{

		internal static Main main;
		internal static ManualLogSource BepLogger;
		private void Awake()
		{
			BepLogger = Logger;
			main = this;

			CreateMissingFolders();

			Harmony.CreateAndPatchAll(typeof(MenuItemRedundancy));
			Harmony.CreateAndPatchAll(typeof(PresetErrorHandling));
			Harmony.CreateAndPatchAll(typeof(Serialization));
			Harmony.CreateAndPatchAll(typeof(ErrorTexturePlaceholder));

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += MenuItemRedundancy.DoOnTitleScreen;
		}

		public static void CreateMissingFolders() 
		{
			var dirs = new string[]{ "Mod", "SaveData", "Preset", "MyRoom", "PhotoModeData", "ScreenShot", "Thumb" };

			foreach (string s in dirs)
			{
				if (!Directory.Exists(BepInEx.Paths.GameRootPath + $"\\{s}")) 
				{
					try
					{
						Directory.CreateDirectory(BepInEx.Paths.GameRootPath + $"\\{s}");
					}
					catch 
					{
						BepLogger.LogFatal($"We couldn't create the directory {BepInEx.Paths.GameRootPath}\\{s}. Please create it manually or you will have errors.");					
					}
				}		
			}			
		}
	}
}
