using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ExtendedErrorHandling
{
	internal static class Serialization
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
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> MaidPropDesFix(IEnumerable<CodeInstruction> instrs)
		{
			var codeMatch = new CodeMatcher(instrs);

			//Only one LdToken in the whole thing so just prefix it.
			codeMatch.MatchForward(false, new CodeMatch(OpCodes.Ldtoken))
			.Insert(
				//Loads the instance (MaidProp) into the stack and pushes it to our delegate
				new CodeInstruction(new CodeInstruction(OpCodes.Ldarg_0)),
				//Action is ran. No return type and we do the changes directly to the variable instead of using ILCode so it's easy though admittedly janky.
				Transpilers.EmitDelegate<Action<MaidProp>>((MpProp) =>
				{
					try
					{
						int num = (int)Enum.Parse(typeof(MPN), MpProp.name, false);
					}
					catch (Exception)
					{
						Main.BepLogger.LogWarning($"{MpProp.name} does not exist! Will not be loaded.");

						MpProp.name = "null_mpn";
					}
				}
			)
			);

			return codeMatch.InstructionEnumeration();
		}
	}
}