using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MyceliumNetworking;
using Newtonsoft.Json;
using Photon.Pun;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VoiceRecognitionAPI;
using VoiceContent.Localization;

namespace VoiceContent;

[BepInDependency("me.loaforc.voicerecognitionapi", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("RugbugRedfern.MyceliumNetworking", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin(modGUID, modName, modVersion)]
public class VoiceContent : BaseUnityPlugin
{
    public const string modGUID = "Notest.VoiceContent";
    public const string modName = "VoiceContent";
    public const string modVersion = "0.9.0";
    public const uint modID = 2215935315;
    public static VoiceContent Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    private Dictionary<string, List<string>>? localizedLists;
    private string[]? cussWords;
    private string[]? youtuberPhrases;
    private string[]? sponsorPhrases;
    private ConfigEntry<string>? configLocalizationLanguage;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        configLocalizationLanguage = Config.Bind(
            "General",
            "language",
            "en",
            "Which language should the VoiceContent mod use for it's localization. (available: en, pt-BR)"
        );

        cussWords = LocalizationData.GetLocalizedList(configLocalizationLanguage.Value, "cussWords").ToArray();
        youtuberPhrases = LocalizationData.GetLocalizedList(configLocalizationLanguage.Value, "youtuberPhrases").ToArray();
        sponsorPhrases = LocalizationData.GetLocalizedList(configLocalizationLanguage.Value, "sponsorPhrases").ToArray();
        if (cussWords.Any() && youtuberPhrases.Any() && sponsorPhrases.Any())
        {
            Logger.LogInfo($"Successfully loaded localization {configLocalizationLanguage.Value} for mod {modGUID}");
        }
        else
        {
            Logger.LogError($"Failed to load localization {configLocalizationLanguage.Value} for mod {modGUID}");
        }

        Patch();

        Logger.LogInfo($"{modGUID} v{modVersion} has loaded!");
        MyceliumNetwork.RegisterNetworkObject(this, modID);

        Voice.ListenForPhrases(youtuberPhrases, (message) => {
            Logger.LogInfo("YouTuber phrase was said");
            HandlePhrase("like");
        });

        Voice.ListenForPhrases(sponsorPhrases, (message) => {
            Logger.LogInfo("Sponsor segment phrase was said");
            HandlePhrase("sponsor");
        });

        Voice.ListenForPhrases(cussWords, (message) => {
            Logger.LogInfo("Cuss word was said");
            HandlePhrase("cuss");
        });
    }

    private static Photon.Realtime.Player? GetPlayerWithCamera()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!GlobalPlayerData.TryGetPlayerData(player, out var globalPlayerData)) continue;
            
            if (globalPlayerData.inventory.GetItems().Any(item => item.item.name == "Camera"))
            {
                return player;
            }
        }

        return null;
    }
    
    private void HandlePhrase(string phraseType)
    {
        var player = GetPlayerWithCamera();

        if (player is not null)
        {
            if (player.IsLocal)
            {
                Logger.LogDebug("Local player is the one holding the camera, creating provider");
                CreateProvider(phraseType);
            }
            else
            {
                Logger.LogDebug("Local player is not the one holding the camera");
                CSteamID steamID;
                bool idSuccess = SteamAvatarHandler.TryGetSteamIDForPlayer(player, out steamID);
                if (idSuccess)
                {
                    Logger.LogDebug("Got steamID successfully");
                    /*
                     * From prior testing I'm sure only the camera man needs to create the provider
                     * But if extensive testing comes out inconclusive I might just give up and RPC providers to everyone always
                    */
                    MyceliumNetwork.RPCTarget(modID, nameof(ReplicateProvider), steamID, ReliableType.Reliable, phraseType);
                }
                else
                {
                    Logger.LogDebug("Could not get SteamId");
                }
                //MyceliumNetwork.RPC(modID, nameof(ReplicateProvider), ReliableType.Reliable, phraseType); 
                CreateProvider(phraseType);
            }
        }
        else
        {
            Logger.LogDebug("Could not get player holding camera, if any");
        }
    }

    [CustomRPC]
    private void ReplicateProvider(string type)
    {
        Logger.LogDebug($"Asked to replicate provider of type {type}");
        CreateProvider(type);
    }

    private void CreateProvider(string type)
    {
        VoiceContentProvider componentInParent = new VoiceContentProvider(type);
        if (!ContentPolling.contentProviders.TryAdd(componentInParent, 400000))
        {
            Dictionary<ContentProvider, int> dictionary = ContentPolling.contentProviders;
            ContentProvider key = componentInParent;
            int seenAmount = dictionary[key];
            dictionary[key] = seenAmount + 400000;
        }
    }
    internal static void Patch()
    {
        Harmony ??= new Harmony(modGUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}
