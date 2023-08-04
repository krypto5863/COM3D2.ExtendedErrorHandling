using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExtendedErrorHandling
{
	internal static class PresetErrorHandling
	{
		[HarmonyPatch(typeof(CharacterMgr), "PresetLoad", typeof(BinaryReader), typeof(string))]
		[HarmonyFinalizer]
		internal static Exception PresetLoadErrorFix(Exception __exception, string __1)
		{
			if (__exception == null)
			{
				return null;
			}

			ExtendedErrorHandling.PluginLogger.LogError($"{__1} could not be loaded!!");
			CornerMessage.DisplayMessage($"[ff4e33]Preset Not Loaded: {__1}[-]");

			return null;
		}

		//previous code is commented, it seemed to be pointless when a quick linq does the same.
		[HarmonyPatch(typeof(CharacterMgr), "PresetListLoad")]
		[HarmonyPostfix]
		internal static void PresetListCleaner(ref List<CharacterMgr.Preset> __result)
		{
			__result.RemoveAll(r => r == null);

			/*
			var listCopy = new CharacterMgr.Preset[__result.Count];

			__result.CopyTo(listCopy);

			foreach (var preset in listCopy)
			{
				if (preset == null)
				{
					__result.Remove(preset);
				}
			}
			*/
		}
	}
}