﻿using HarmonyLib;
using System;

namespace ExtendedErrorHandling
{
	internal static class MenuItemRedundancy
	{
		[HarmonyPatch(typeof(Maid), nameof(Maid.ProcItem))]
		[HarmonyPrefix]
		internal static void ErrorHandleMissingMenus(ref MaidProp __0)
		{
			if (string.IsNullOrEmpty(__0.strFileName) || GameUty.IsExistFile(__0.strFileName))
			{
				return;
			}

			var category = (MPN)Enum.Parse(typeof(MPN), __0.name, true);

			if (category == MPN.body)
			{
				ExtendedErrorHandling.PluginLogger.LogWarning($"Body menu: {__0.strFileName} could not be found! Reverting to base body...");

				ExtendedErrorHandling.CornerMessage.DisplayMessage($"[fffd7a]Missing Body: {__0.strFileName}[-]");

				__0.strFileName = "body001_I_.menu";
			}
			else if ((MPN)__0.idx == MPN.head)
			{
				ExtendedErrorHandling.PluginLogger.LogWarning($"Face menu: {__0.strFileName} could not be found! Reverting to base face...");

				ExtendedErrorHandling.CornerMessage.DisplayMessage($"[fffd7a]Missing Face: {__0.strFileName}[-]");

				__0.strFileName = "face006_I_.menu";
			}
		}

		[HarmonyPatch(typeof(Menu), nameof(Menu.ProcScriptBin), typeof(Maid), typeof(byte[]), typeof(MaidProp), typeof(bool), typeof(SubProp))]
		[HarmonyFinalizer]
		internal static Exception MaidPropErrorFix(Exception __exception, MaidProp mp)
		{
			if (__exception == null)
			{
				return null;
			}

			ExtendedErrorHandling.PluginLogger.LogError($"There was an exception while trying to read {mp.strFileName}");

			ExtendedErrorHandling.CornerMessage.DisplayMessage(ExtendedErrorHandling.VerboseCornerMessages.Value
				? $"[ff4e33]Menu read failed: {mp.strFileName}.[-]"
				: "[ff4e33]Failed to read menu![-]");

			return null;
		}

		[HarmonyPatch(typeof(TBody), nameof(TBody.MulTexSet))]
		[HarmonyPrefix]
		internal static void TattooLayerFixer(ref string __0, ref int __3, SubProp f_SubProp)
		{
			if (f_SubProp == null)
			{
				return;
			}

			if (__0.Equals("head") && __3 == 1)
			{
				return;
			}

			if (__0.Equals("body") && __3 <= 3)
			{
				return;
			}

			__3 = 1;
			ExtendedErrorHandling.PluginLogger.LogWarning($"{f_SubProp.strFileName} uses an improper layer which will cause odd operation! Fix when possible...");
		}
	}
}