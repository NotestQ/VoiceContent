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
        contentEvents.Add(new ContentEventFrame(new VoiceContentEvent(typeOfComment), seenAmount, time));
        return;
    }
    string typeOfComment;
}