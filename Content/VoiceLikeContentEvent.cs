using Zorro.Core;
using ContentLibrary;

public class VoiceLikeContentEvent : PlayerBaseEvent
{
    // Will never reach GetContentValue if this is null by the way, for ex: PlayerEmoteContentEvent. This empty one is only called by the deserializer
    public VoiceLikeContentEvent()
    {
    }

    public VoiceLikeContentEvent(string playerName, int actorNumber) : base(playerName, actorNumber)
    {
    }

    public override float GetContentValue()
    {
        return 8f;
    }

    public override ushort GetID()
    {
        return ContentHandler.GetEventID(nameof(VoiceLikeContentEvent));
    }

    public override string GetName()
    {
        return "Player said YouTuber keyword";
    }

    public override Comment GenerateComment()
    {
        VoiceContent.VoiceContent.Logger.LogDebug("YouTuber phrase comment generated!");
        return new Comment(base.FixPlayerName(LIKE_COMMENTS.GetRandom<string>()));
    }


    // I am so sorry for these comments.
    public string[] LIKE_COMMENTS = // [sic]!!
    {
        "haha like and subscribe? maybe i will",
        "liking this!",
        "I am liking and subscribing.",
        "sir yes sir",
        "they said the thing lol",
        "stupid spooktuber segment",
        "I DID WHAT YOU SAID!!",
        "i got that hater in me, i will not be liking and subscribing",
        "Nice job on the SpookTuber segment",
        "What a nice SpookTube channel. I will be subscribing thank you very much.",
        "You deserve more subscribers",
        "lmao, this channel is great",
        "AND DONT FORGET TO HIT THAT NOTIFICATION BELL TO NOT MISS ANY OF MY VIDEOS",
        "i am begign you to not include these spooktuber segments anymore",
        "I know they're corny but I absolutely love these spooktuber segments",
        "I love your chanel i watch all of your videos i liked and hit the notifiction bell",
        "No option to not recommend this channel.",
        "This segment will get ingrained in my brain, if I hear like and subscribe in my dreams I'm lbaming you",
        "Love your vids.......",
        "you guys say that every single time haha",
        "Usually I find thse spooktuber segments cringe but I subbed because of it",
    };
}