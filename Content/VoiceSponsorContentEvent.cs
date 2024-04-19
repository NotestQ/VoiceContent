using Zorro.Core;
using ContentLibrary;

public class VoiceSponsorContentEvent : PlayerBaseEvent
{
    // Will never reach GetContentValue if this is null by the way, for ex: PlayerEmoteContentEvent. This empty one is only called by the deserializer
    public VoiceSponsorContentEvent()
    {
    }

    public VoiceSponsorContentEvent(string playerName, int actorNumber) : base(playerName, actorNumber)
    {
    }

    public override float GetContentValue()
    {
        return 5f;
    }

    public override ushort GetID()
    {
        return ContentHandler.GetEventID(nameof(VoiceSponsorContentEvent));
    }

    public override string GetName()
    {
        return "Player said sponsor keyword";
    }

    public override Comment GenerateComment()
    {
        VoiceContent.VoiceContent.Logger.LogDebug("Sponsor segment comment generated!");
        return new Comment(base.FixPlayerName(SPONSOR_COMMENTS.GetRandom<string>()));
    }


    // I am so sorry for these comments.
    public string[] SPONSOR_COMMENTS = // [sic]!!
    {
        "why do you guys even take sponsors?",
        "insta skip on that sponsor segment",
        "might check that sponsor",
        "lol great sponsor",
        "tired of so many sponsors nowadays",
        "MORE SPONSORS??",
        "RAID SHADOW LEGENDS!",
        "Cringe sponsor segment, at least it was quick to skip",
        "Surprisingly good sponsor segment",
        "I actually went to that sponsor",
        "That sponsor is not that bad, have used their services",
        "LOL How did any sponsor accept this sponsorship deal",
        "respect on the sponsors to give you full agency",
        "Hope you guys were being honest on that sponsor segment",
        "OMG you guys have sponsors??",
        "grats on the sponsorship deal",
        "im in my deathbed and the last thing im hearing is this stupid sponsor segment goodbye",
        "i thought u got enough money already y do u have sponsors",
        "THIS VIDEO IS BROUGHT TO YOU BY SPONSORBLOCK!!!1",
        "sellouts.",
        "Certified based sponsor",
    };
}