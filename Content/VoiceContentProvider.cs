using System.Collections.Generic;
using UnityEngine;

public class VoiceContentProvider : ContentProvider
{
    public VoiceContentProvider(string type, string? playerNickName, int? playerActorNumber)
    {
        if (playerNickName != null && playerActorNumber != null)
        {
            nickName = playerNickName!;
            actorNumber = (int)playerActorNumber!;
        } 
        else 
        {
            nickName = Player.localPlayer.refs.view.Owner.NickName;
            actorNumber = Player.localPlayer.refs.view.Owner.ActorNumber;
        }

        typeOfComment = type;
    }
    public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
    {

        switch (typeOfComment)
        {
            case "swear":
                contentEvents.Add(new ContentEventFrame(new VoiceSwearContentEvent(nickName, actorNumber), seenAmount, time));
                break;
            case "like":
                contentEvents.Add(new ContentEventFrame(new VoiceLikeContentEvent(nickName, actorNumber), seenAmount, time));
                break;
            case "sponsor":
                contentEvents.Add(new ContentEventFrame(new VoiceSponsorContentEvent(nickName, actorNumber), seenAmount, time));
                break;
            default:
                VoiceContent.VoiceContent.Logger.LogError($"typeOfComment not recognized: {typeOfComment}");
                break;
        }
        return;
    }

    string nickName;
    int actorNumber;
    string typeOfComment;
}