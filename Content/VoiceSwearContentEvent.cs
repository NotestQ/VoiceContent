using Zorro.Core;
using ContentLibrary;

public class VoiceSwearContentEvent : PlayerBaseEvent
{
    // Will never reach GetContentValue if this is null by the way, for ex: PlayerEmoteContentEvent. This empty one is only called by the deserializer
    public VoiceSwearContentEvent()
    {
    }

    public VoiceSwearContentEvent(string playerName, int actorNumber) : base(playerName, actorNumber) 
    {
    }

    public override float GetContentValue()
    {
        return 5f;
    }

    public override ushort GetID()
    {
        return ContentHandler.GetEventID(nameof(VoiceSwearContentEvent));
    }

    public override string GetName()
    {
        return "Player said swear keyword";
    }

    public override Comment GenerateComment()
    {
        VoiceContent.VoiceContent.Logger.LogDebug("Swear comment generated!");
        return new Comment(base.FixPlayerName(SWEAR_COMMENTS.GetRandom<string>()));
    }

    // I am so sorry for these comments.

    public string[] SWEAR_COMMENTS = // [sic]!!
    {
        "stop swearing, my dog watches this!",
        "you just ruined my family outing where we watch diving bell videos...",
        "My son is starting to learn how to swear because of <playername>! Shameful.",
        "like this comment if you think kids should stop using spooktube so we get more channels that swear",
        "lol they cussed",
        "I thought this content was for kids.",
        "you folk swear a lot",
        "<playername> swears a lot",
        "hehe swears",
        "How many swears do you people know?",
        "All channels are family friendly nowadays so this is a breath of fresh air",
        "Nice comedic timing on those swears",
        "And to think I showed my kids this content. For shame.",
        "I thought this channel was family friendly!!!!",
        "Liking just because of that moment you swore",
        "It's nice to see channels that don't fall to being family friendly",
        "Such vulgar profanity.",
        "how are there so many kids in spooktube lol its the last place i expected to get hit by family friendliness",
        "my mom says im not allowed to swear so you shouldt either",
        "swearing is bad for you",
        "Legit subscribed when you swore lol",
        "LMAO, nice!",
        "based spooktuber that swears",
    };
}