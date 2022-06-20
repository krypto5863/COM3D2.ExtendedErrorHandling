﻿using HarmonyLib;
using System;
using UnityEngine.SceneManagement;

namespace ExtendedErrorHandling
{
	internal static class MenuItemRedundancy
	{
		//Awake is carried out before the game has even had time to load anything but the assembly. As a result, setting QuitWhenAssert false on awake has it being reverted moments later. Had to delay our own set.
		internal static void DoOnTitleScreen(Scene s, LoadSceneMode load)
		{
			if (s.name.Equals("SceneTitle"))
			{
				NDebug.m_bQuitWhenAssert = false;
				UnityEngine.SceneManagement.SceneManager.sceneLoaded -= DoOnTitleScreen;
			}
		}

		[HarmonyPatch(typeof(Maid), "ProcItem")]
		[HarmonyPrefix]
		internal static void ErrorHandleMissingMenus(ref MaidProp __0)
		{
			if (!String.IsNullOrEmpty(__0.strFileName) && !GameUty.IsExistFile(__0.strFileName))
			{
				MPN category = (MPN)Enum.Parse(typeof(MPN), __0.name, true);

				if (category == MPN.body)
				{
					Main.BepLogger.LogWarning($"Body menu: {__0.strFileName} could not be found! Reverting to base body...");

	
					CornerMessage.DisplayMessage($"[fffd7a]Missing Body: {__0.strFileName}[-]");

					__0.strFileName = "body001_I_.menu";
				}
				else if ((MPN)__0.idx == MPN.head)
				{
					Main.BepLogger.LogWarning($"Face menu: {__0.strFileName} could not be found! Reverting to base face...");

					CornerMessage.DisplayMessage($"[fffd7a]Missing Face: {__0.strFileName}[-]");

					__0.strFileName = "face006_I_.menu";
				}
			}
		}

		[HarmonyPatch(typeof(Menu), "ProcScriptBin", new Type[] { typeof(Maid), typeof(byte[]), typeof(MaidProp), typeof(bool), typeof(SubProp) })]
		[HarmonyFinalizer]
		internal static Exception Finalizer2(Exception __exception, MaidProp mp)
		{
			if (__exception != null)
			{
				Main.BepLogger.LogError($"There was an exception while trying to read {mp.strFileName}");
				CornerMessage.DisplayMessage($"[ff4e33]Couldn't read {mp.strFileName}[-]");
			}

			return null;
		}
	}
}