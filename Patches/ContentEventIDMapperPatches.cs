using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace VoiceContent.Patches
{
    [HarmonyPatch(typeof(ContentEventIDMapper))]
    internal class ContentEventIDMapperPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ContentEventIDMapper.GetContentEvent))]
        public static bool GetContentEventPrefix(ushort id, ref ContentEvent __result)
        {
            if (id == 2000)
            {
                __result = new VoiceContentEvent();
                return false;
            }

            VoiceContent.Logger.LogInfo($"GetContentEvent was called: {id}, {__result}");
            return true;
        }
    }
}
            