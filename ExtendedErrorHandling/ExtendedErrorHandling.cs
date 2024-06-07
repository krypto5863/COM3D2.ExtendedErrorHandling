using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using System.IO;
using System.Security;
using System.Security.Permissions;
using UnityEngine.SceneManagement;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ExtendedErrorHandling
{
	[BepInPlugin("ExtendedErrorHandling", "ExtendedErrorHandling", "1.7")]
	[BepInDependency("COM3D2.CornerMessage", BepInDependency.DependencyFlags.SoftDependency)]
	public class ExtendedErrorHandling : BaseUnityPlugin
	{
		internal static ExtendedErrorHandling Instance;
		internal static ManualLogSource PluginLogger => Instance.Logger;

		internal static ConfigEntry<MessageBoxHandling> ConvertMessageBoxToCornerMessage;
		internal static ConfigEntry<bool> VerboseCornerMessages;
		internal static ConfigEntry<bool> LoadRawPng;

		[UsedImplicitly]
		private void Awake()
		{
			Instance = this;

			ConvertMessageBoxToCornerMessage = Config.Bind("General", "Error Message Box Behavior",
				MessageBoxHandling.Default,
				"When the game encounters an error it considers fatal, it'll display a message box. Some users find this intrusive, this allows you to change the behavior.");
			VerboseCornerMessages = Config.Bind("General", "Verbose Corner Messages", false,
				"Corner messages will display more useful and complete information.");

			LoadRawPng = Config.Bind("Extra", "Load Raw Images (Experimental)", false,
				"When a .tex file can't be found, it will instead attempt to find a png file of the same name and load it in place. Not suggested, can increase memory usage.");

			CornerMessage.CornerMessageLoaded =
				BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("COM3D2.CornerMessage");

			Harmony.CreateAndPatchAll(typeof(ExtendedErrorHandling));
			Harmony.CreateAndPatchAll(typeof(MenuItemRedundancy));
			Harmony.CreateAndPatchAll(typeof(PresetErrorHandling));
			Harmony.CreateAndPatchAll(typeof(Serialization));
			Harmony.CreateAndPatchAll(typeof(ErrorTexturePlaceholder));
			Harmony.CreateAndPatchAll(typeof(NDebugMod));

			SceneManager.sceneLoaded += NDebugMod.DoOnTitleScreen;
			SceneManager.sceneLoaded += SomeSceneLoaded;
		}

		private static void SomeSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			PluginLogger.LogDebug("First scene loaded! Creating folders...");
			SceneManager.sceneLoaded -= SomeSceneLoaded;
			CreateMissingFolders();
		}

		public static void CreateMissingFolders()
		{
			TryCreateDirectory(Path.Combine(Paths.GameRootPath, "Mod"));

			var dirs = new[] { "SaveData", "Preset", "MyRoom", "PhotoModeData", "ScreenShot", "Thumb" };

			foreach (var s in dirs)
			{
				var dir = Path.Combine(GameMain.Instance.SerializeStorageManager.StoreDirectoryPath, s);
				TryCreateDirectory(dir);
			}

			return;

			void TryCreateDirectory(string s)
			{
				if (Directory.Exists(s))
				{
					return;
				}

				try
				{
					Directory.CreateDirectory(s);
				}
				catch
				{
					PluginLogger.LogFatal(
						$"We couldn't create the directory {s}. Please create it manually or you will have errors.");
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
				internal static void DisplayMessage(string mess, float dur) =>
					COM3D2.CornerMessage.CornerMessage.DisplayMessage(mess, dur);
			}
		}
	}
}