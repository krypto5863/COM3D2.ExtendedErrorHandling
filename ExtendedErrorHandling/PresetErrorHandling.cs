using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExtendedErrorHandling
{
	static class PresetErrorHandling
	{
        [HarmonyPatch(typeof(CharacterMgr), "PresetLoad", new Type[] { typeof(BinaryReader), typeof(string) })]
        [HarmonyFinalizer]
        internal static Exception Finalizer(Exception __exception, string __1)
        {
            if (__exception != null)
            {
                Main.BepLogger.LogError($"{__1} could not be loaded!!");
            }

            return null;
        }

        [HarmonyPatch(typeof(CharacterMgr), "PresetListLoad")]
        [HarmonyPostfix]
        internal static void ListCleaner(ref List<CharacterMgr.Preset> __result)
        {
            CharacterMgr.Preset[] listCopy = new CharacterMgr.Preset[__result.Count];

            __result.CopyTo(listCopy);

            foreach (CharacterMgr.Preset preset in listCopy)
            {
                if (preset == null)
                {
                    __result.Remove(preset);
                }
            }
        }
    }
}
