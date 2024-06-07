using HarmonyLib;
using UnityEngine.SceneManagement;

namespace ExtendedErrorHandling
{
	public enum MessageBoxHandling
	{
		Default = 0,
		Disable = 1,
		NotifyInCorner = 2
	}
	public static class NDebugMod
	{
		//Awake is carried out before the game has even had time to load anything but the assembly. As a result, setting QuitWhenAssert false on awake has it being reverted moments later. Had to delay our own set.
		internal static void DoOnTitleScreen(Scene s, LoadSceneMode load)
		{
			if (!s.name.Equals("SceneTitle"))
			{
				return;
			}

			NDebug.m_bQuitWhenAssert = false;
			SceneManager.sceneLoaded -= DoOnTitleScreen;
		}

		[HarmonyPatch(typeof(NDebug), nameof(NDebug.MessageBox))]
		[HarmonyPrefix]
		public static bool MessageBoxToCorner(ref string f_strTitle, ref string f_strMsg)
		{
			switch (ExtendedErrorHandling.ConvertMessageBoxToCornerMessage.Value)
			{
				case MessageBoxHandling.Disable:
					return false;
				case MessageBoxHandling.NotifyInCorner when ExtendedErrorHandling.CornerMessage.CornerMessageLoaded:
					ExtendedErrorHandling.CornerMessage.DisplayMessage($"[ff4e33]{f_strTitle}: Check console...[-]");
					return false;
				case MessageBoxHandling.Default:
				default:
					return true;
			}
		}
	}
}