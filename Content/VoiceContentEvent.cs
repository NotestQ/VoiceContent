using BepInEx.Logging;
using System;
using System.Collections.Generic;
using VoiceContent;
using Zorro.Core;
using Zorro.Core.Serizalization;
using System.Text;

using static UnityEngine.ParticleSystem;
public class VoiceContentEvent : ContentEvent
{
    // Will never reach GetContentValue if this is null by the way, for ex: PlayerEmoteContentEvent. This empty one is only called by the deserializer
    public VoiceContentEvent()
    {

    }
    public VoiceContentEvent(string typeOfComment)
    {
        type = typeOfComment;
    }
    public override float GetContentValue()
    {
        switch (type)
        {
            case "cuss":
                return 20f;
            case "sponsor":
                return 20f;
            case "like":
                return 50f;
            default:
                VoiceContent.VoiceContent.Logger.LogDebug("Type was not recognized.");
                return 0f;
        }
    }

    public override ushort GetID()
    {
        return 2000;
    }

    public override string GetName()
    {
        return "Player said keyword";
    }

    public override Comment GenerateComment()
    {
        VoiceContent.VoiceContent.Logger.LogDebug($"Asked to generate comment, current type is {type}");
        switch (type)
        {
            case "cuss":
                VoiceContent.VoiceContent.Logger.LogDebug("Cuss comment generated!");
                return new Comment(SWEAR_COMMENTS.GetRandom<string>());
            case "like":
                VoiceContent.VoiceContent.Logger.LogDebug("YouTuber phrase comment generated!");
                return new Comment(LIKE_COMMENTS.GetRandom<string>());
            case "sponsor":
                VoiceContent.VoiceContent.Logger.LogDebug("Sponsor segment comment generated!");
                return new Comment(SPONSOR_COMMENTS.GetRandom<string>());
            default:
                VoiceContent.VoiceContent.Logger.LogDebug("Fallback comment generated!");
                return new Comment(FALLBACK_COMMENTS.GetRandom<string>());
        }
    }

    public override void Serialize(BinarySerializer serializer)
    {
        serializer.WriteString(type, Encoding.UTF8);
    }

    public override void Deserialize(BinaryDeserializer deserializer)
    {
        type = deserializer.ReadString(Encoding.UTF8);
    }

    string? type;

    public string[] FALLBACK_COMMENTS =
    {
        "Please send on Discord. Okay" // Actual string that can appear in "base game" if your content ID is wrong so, why not
    };

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

    public string[] SWEAR_COMMENTS = // [sic]!!
    {
        "stop swearing, my dog watches this!",
        "you just ruined my family outing where we watch diving bell videos...",
        "My son is starting to learn how to swear because of you! Shameful.",
        "like this comment if you think kids should stop using spooktube so we get more channels that swear",
        "lol they cussed",
        "I thought this content was for kids.",
        "you folk swear a lot",
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