using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MyceliumNetworking;
using Photon.Pun;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoiceRecognitionAPI;

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

    /*
     * Try not to repeat words and/or phrases
     * Try to be specific while trying to minimize repetition, so words and phrases that can be used in normal situations don't trigger content
     * Obviously, don't add sponsors or words like "honey" or "sofi" because these can be used in day to day conversation
     */

    private string[] cussWords = { 
        "fuck",
        "shit",
        "cunt",
        "bitch",
        "bastard",
        "asshole",
        "bullshit",
        "cock",
        "twat",
        "wanker",
        "bellend",
        "slut",
        "prick",
        "pussy",
        "motherfucker",
        "hell",
        "balls",
    };

    private string[] youtuberPhrases = {
        "like and subscribe",
        "don't forget to share",
        "hit that subscribe button",
        "like comment and subscribe",
        "don't forget to subscribe",
        "let's jump right into it",
        "give this video a thumbs up",
        "that like button",
        "the like button",
        "that notification bell",
        "the notification bell",
        "that subscribe button",
        "the subscribe button",
        "before starting this video",
    };

    private string[] sponsorPhrases =
    {
        "i want to give a huge shoutout to our sponsor",
        "i want to give a shoutout to our sponsor",
        "i want to take a quick moment to thank",
        "video is sponsored by",
        "video is made possible by",
        "video is brought to you by",
        "episode is sponsored by",
        "episode is brought to you by",
        "episode is made possible by",
        "before we begin i want to thank",
        "promo code",
        "discount code",
        "star code",
        "creator code",
        "patreon",
        "kofi",
        "gfuel",
        "temu",
        "nordvpn",
        "private internet access",
        "expressvpn",
        "audible",
        "skillshare",
        "squarespace",
        "raid shadow legends",
        "raycon",
        "hello fresh",
        "manscaped",
        "betterhelp",
        "grammarly",
        "blue apron",
        "dollar shave club",
        "rocket money",
        "fortnite",
        "honkai star rail",
        "genshin impact",
        "paypal",
    };

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

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


    private Photon.Realtime.Player? GetPlayerWithCamera()
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
