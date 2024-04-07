using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using UnityEngine;

namespace VoiceContent.Patches
{
    [HarmonyPatch(typeof(UploadCompleteUI))]
    internal class UploadCompleteUIPatches
    {
        /*
         * This is a bandaid fix. Do not do this. 
         * This hangs the game for a few miliseconds and gives time for the video player to get the correct length I guess
         */
        [HarmonyPrefix]
        [HarmonyPatch(nameof(UploadCompleteUI.DisplayVideoEval))]
        public static void DisplayVideoEvalPrefix(ref UploadCompleteUI __instance)
        {
            VoiceContent.Logger.LogInfo($"DisplayVideoEval was called: {__instance.m_videoPlayer.length}");
            float time = (float)__instance.m_videoPlayer.length;
            for (int i = 0; i < 1000000; i++)
            {
                time = (float)__instance.m_videoPlayer.length;
                if (time > 0.01f)
                {
                    break;
                }
            }
            VoiceContent.Logger.LogInfo($"DisplayVideoEval was called: {__instance.m_videoPlayer.length}");
        }
    }
}
            