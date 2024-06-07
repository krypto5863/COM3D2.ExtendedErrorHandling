using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ExtendedErrorHandling
{
	internal static class Serialization
	{
		[HarmonyPatch(typeof(Maid), nameof(Maid.GetProp), typeof(string))]
		[HarmonyPrefix]
		public static void GetPropStringFix(Maid __instance, ref string __0)
		{
			if (!__instance.m_dicMaidProp.ContainsKey(__0))
			{
				__0 = "null_mpn";
			}
		}

		[HarmonyPatch(typeof(CM3), nameof(CM3.Init))]
		[HarmonyPrefix]
		public static void CM_dic_fix()
		{
			CM3.dicDelItem[MPN.null_mpn] = string.Empty;
		}

		[HarmonyPatch(typeof(MaidProp), nameof(MaidProp.Deserialize))]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> MaidPropDesFix(IEnumerable<CodeInstruction> instructions)
		{
			var codeMatch = new CodeMatcher(instructions);

			//Only one LdToken in the whole thing so just prefix it.
			codeMatch.MatchForward(false, new CodeMatch(OpCodes.Ldtoken))
			.Insert(
				//Loads the Instance (MaidProp) into the stack and pushes it to our delegate
				new CodeInstruction(new CodeInstruction(OpCodes.Ldarg_0)),
				//Action is ran. No return type and we do the changes directly to the variable instead of using ILCode so it's easy though admittedly janky.
				Transpilers.EmitDelegate<Action<MaidProp>>((maidProp) =>
				{
					try
					{
						_ = (int)Enum.Parse(typeof(MPN), maidProp.name, false);
					}
					catch (Exception)
					{
						ExtendedErrorHandling.PluginLogger.LogWarning($"{maidProp.name} does not exist! Will not be loaded.");

						maidProp.name = "null_mpn";
					}
				}
			)
			);

			return codeMatch.InstructionEnumeration();
		}
	}
}