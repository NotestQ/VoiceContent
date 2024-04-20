using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MyceliumNetworking;
using Photon.Pun;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using VoiceRecognitionAPI;
using VoiceContent.Localization;
using ContentLibrary;

namespace VoiceContent;

/*
 * Sadly pt-BR doesn't work as of yet because of Windows Voice Recognition and the Vosk Voice Recognition API not being done yet :(
 * PLANNED: Make VoskVoiceRecognitionAPI to support more languages
 * TODO: Include checks for language, if WindowsVoiceRecognitionAPI tries to start and your language is one that the API doesn't support it'll break
 * TODO: Include checks for OS, if you try to load the Windows API in Linux your house explodes
 */

[BepInDependency("me.loaforc.voicerecognitionapi", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(MyceliumNetworking.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(ContentLibrary.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin(modGUID, modName, modVersion)]
public class VoiceContent : BaseUnityPlugin
{
    public const string modGUID = "Notest.VoiceContent";
    public const string modName = "VoiceContent";
    public const string modVersion = "0.10.4";
    public const uint modID = 2215935315;
    public static VoiceContent Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    private Dictionary<string, List<string>>? localizedLists;
    private string[]? swearWords;
    private string[]? youtuberPhrases;
    private string[]? sponsorPhrases;
    private ConfigEntry<string>? configLocalizationLanguage;
    private ConfigEntry<string>? configVoiceRecognitionAPI;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        configLocalizationLanguage = Config.Bind(
            "General",
            "Language",
            "en",
            "Which language should the VoiceContent mod uses for its localization. \n (Available for Windows API: en)"
        );

        configVoiceRecognitionAPI = Config.Bind(
            "General",
            "Voice Recognition Api",
            "Windows",
            "Which voice recognition API the VoiceContent mod should use. (Available: Windows)"
            );

        swearWords = LocalizationData.GetLocalizedList(configLocalizationLanguage.Value, "swearWords").ToArray();
        youtuberPhrases = LocalizationData.GetLocalizedList(configLocalizationLanguage.Value, "youtuberPhrases").ToArray();
        sponsorPhrases = LocalizationData.GetLocalizedList(configLocalizationLanguage.Value, "sponsorPhrases").ToArray();

        if (swearWords.Any() && youtuberPhrases.Any() && sponsorPhrases.Any())
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

        ContentHandler.AssignEvent(new VoiceLikeContentEvent());
        ContentHandler.AssignEvent(new VoiceSwearContentEvent());
        ContentHandler.AssignEvent(new VoiceSponsorContentEvent());

        Voice.ListenForPhrases(youtuberPhrases, (message) => {
            Logger.LogDebug("YouTuber phrase was said");
            HandlePhrase("like");
        });

        Voice.ListenForPhrases(sponsorPhrases, (message) => {
            Logger.LogDebug("Sponsor segment phrase was said");
            HandlePhrase("sponsor");
        });

        Voice.ListenForPhrases(swearWords, (message) => {
            Logger.LogDebug("Swear word was said");
            HandlePhrase("swear");
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
                    var localPlayerData = Player.localPlayer.refs.view.Owner;
                    MyceliumNetwork.RPCTarget(modID, nameof(ReplicateProvider), steamID, ReliableType.Reliable, phraseType, localPlayerData.NickName, localPlayerData.ActorNumber);
                }
                else
                {
                    Logger.LogDebug("Could not get SteamId");
                }
                
                CreateProvider(phraseType);
            }
        }
        else
        {
            Logger.LogDebug("Could not get player holding camera, if any");
        }
    }

    [CustomRPC]
    private void ReplicateProvider(string type, string playerNickName, int playerActorNumber)
    {
        Logger.LogDebug($"Asked to replicate provider of type {type}");
        CreateProvider(type, playerNickName, playerActorNumber);
    }

    private void CreateProvider(string type, string? playerNickName = null, int? playerActorNumber = null)
    {
        VoiceContentProvider componentInParent = new VoiceContentProvider(type, playerNickName, playerActorNumber);
        ContentHandler.ManualPoll(componentInParent, 1f, 100);
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
