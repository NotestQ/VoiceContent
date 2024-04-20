# VoiceContent
Adds content to Content Warning via voice recognition

## It doesn't work??
If the mod itself is throwing an error then copy and paste the error in [the github issue page](https://github.com/NotestQ/VoiceContent/issues), preferrably describe your issue too 

### Windows
If the mod is not throwing errors but it's not recognizing your voice  
 - Make sure you have set your [default microphone](https://www.howtogeek.com/700440/how-to-choose-your-default-microphone-on-windows-10/) correctly.
 - Change BepInEx's `LogLevels` in its config found at `(ContentWarningDir)/BepInEx/config/BepInEx.cfg`. to include `, Debug` or change it to `All`. If it is getting wrong results try [improving your speech recognition](https://support.microsoft.com/en-us/windows/use-voice-recognition-in-windows-83ff75bd-63eb-0b6c-18d4-6fae94050571#:~:text=In%20Control%20Panel%2C%20select%20Ease,to%20set%20up%20speech%20recognition.)

## Dependencies
This depends on my fork of [VoiceRecognitionAPI](https://github.com/NotestQ/VoiceRecognitionAPI) for Content Warning, [Mycelium](https://github.com/RugbugRedfern/Mycelium-Networking-For-Content-Warning) and my [ContentLibrary](https://github.com/NotestQ/ContentLibrary)

## Types of content
Right now, the mod recognizes three types of content: Swearing, sponsor segments and acting like a YouTuber (SpookTuber)  
Examples: "Don't forget to share, like and subscribe!" (YouTuber) "fuck" (swearing) and "This video is sponsored by" (sponsors)  
Saying any of these keywords will trigger content, which will give you views and add a respective comment  
Keep in mind every content fights to be the one that gives views and a comment, so if you film a creature or do any other sort of content and trigger this mod's content it might not appear in the TV  

## Contributing
This is my actual first time making a git repo, programming specifically in C# AND modding an Unity game so bear with me if something is wrong, multiple things probably are  

If you'd like to build the mod for yourself:  
 - Clone the repo  
 - Set your references  
 - Then contribute??  
 - Build like you would a normal mod  

### Credits
Thanks a lot to:
- [leonardomurakami](https://github.com/leonardomurakami) for making translations possible and making me aware of other voice recognition software!
- [bananasov](https://github.com/bananasov) for rewriting part of my horrible code and partly helping me with getting used to C#!
