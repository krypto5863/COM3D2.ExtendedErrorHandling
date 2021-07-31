using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedErrorHandling
{
	static class Serialization
	{
		[HarmonyPatch(typeof(Maid), "GetProp", new Type[] { typeof(string) })]
		[HarmonyPrefix]
		public static void GetPropStringFix(Maid __instance, ref string __0)
		{
			if (!__instance.m_dicMaidProp.ContainsKey(__0))
			{
				__0 = "null_mpn";
			}
		}

		[HarmonyPatch(typeof(CM3), "Init")]
		[HarmonyPrefix]
		public static void CM_dic_fix()
		{
			CM3.dicDelItem[MPN.null_mpn] = string.Empty;
		}

		[HarmonyPatch(typeof(MaidProp), "Deserialize")]
		[HarmonyFinalizer]
		public static Exception MaidPropDesFix(MaidProp __instance, Exception __exception)
		{
			if (__exception != null)
			{
				Main.BepLogger.LogError($"{__instance.name} could not be loaded!!");
			}

			__instance.name = "null_mpn";

			return null;
		}
	}
}
