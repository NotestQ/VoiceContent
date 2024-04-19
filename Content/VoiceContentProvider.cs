using System.Collections.Generic;
using UnityEngine;

public class VoiceContentProvider : ContentProvider
{
    public VoiceContentProvider(string type)
    {
        typeOfComment = type;
    }
    public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
    {
        string nickName = Player.localPlayer.refs.view.Owner.NickName;
        int actorNumber = Player.localPlayer.refs.view.Owner.ActorNumber;

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

    string typeOfComment;
}