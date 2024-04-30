using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using System.IO;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ExtendedErrorHandling
{
	[BepInPlugin("ExtendedErrorHandling", "ExtendedErrorHandling", "1.6.1")]
	[BepInDependency("COM3D2.CornerMessage", BepInDependency.DependencyFlags.SoftDependency)]
	public class ExtendedErrorHandling : BaseUnityPlugin
	{
		internal static ExtendedErrorHandling Instance;
		internal static ManualLogSource PluginLogger => Instance.Logger;
		internal static ConfigEntry<bool> LoadRawPng;

		[UsedImplicitly]
		private void Awake()
		{
			Instance = this;

			LoadRawPng = Config.Bind("General", "Load Raw Images (Experimental)", false, "When a .tex file can't be found, it will instead attempt to find a png file of the same name and load it in place. Not suggested, can increase memory usage.");

			CreateMissingFolders();

			CornerMessage.CornerMessageLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("COM3D2.CornerMessage");

			Harmony.CreateAndPatchAll(typeof(MenuItemRedundancy));
			Harmony.CreateAndPatchAll(typeof(PresetErrorHandling));
			Harmony.CreateAndPatchAll(typeof(Serialization));
			Harmony.CreateAndPatchAll(typeof(ErrorTexturePlaceholder));

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += MenuItemRedundancy.DoOnTitleScreen;
		}

		public static void CreateMissingFolders()
		{
			var dirs = new[] { "Mod", "SaveData", "Preset", "MyRoom", "PhotoModeData", "ScreenShot", "Thumb" };

			foreach (var s in dirs)
			{
				if (Directory.Exists(Paths.GameRootPath + $"\\{s}"))
				{
					continue;
				}

				try
				{
					Directory.CreateDirectory(Paths.GameRootPath + $"\\{s}");
				}
				catch
				{
					PluginLogger.LogFatal($"We couldn't create the directory {Paths.GameRootPath}\\{s}. Please create it manually or you will have errors.");
				}
			}
		}
	}

	//Classes for optional CornerMessage support. The segmentation of classes should prevent the TypeLoadException error when the dll isn't loaded.
	internal static class CornerMessage
	{
		internal static bool CornerMessageLoaded;

		internal static void DisplayMessage(string mess, float dur = 6f)
		{
			if (CornerMessageLoaded)
			{
				TryCornerMessage.DisplayMessage(mess, dur);
			}
		}

		internal static class TryCornerMessage
		{
			internal static void DisplayMessage(string mess, float dur) => COM3D2.CornerMessage.CornerMessage.DisplayMessage(mess, dur);
		}
	}
}