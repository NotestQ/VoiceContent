# VoiceContent
Adds content to Content Warning via voice recognition
This is old, has smelly code, and is probably broken. If you want, look into having it use VOSK Voice Recognition and use a better Content ID Library.

## Issues
If the mod itself is throwing an error then copy and paste the error in [the github issue page](https://github.com/NotestQ/VoiceContent/issues), preferrably describe your issue too 

### Windows
If the mod is not throwing errors but it's not recognizing your voice  
 - Make sure you have set your [default microphone](https://www.howtogeek.com/700440/how-to-choose-your-default-microphone-on-windows-10/) correctly.
 - Change BepInEx's `LogLevels` in its config found at `(ContentWarningDir)/BepInEx/config/BepInEx.cfg`. to include `, Debug` or change it to `All`. If it is getting wrong results try [improving your speech recognition](https://support.microsoft.com/en-us/windows/use-voice-recognition-in-windows-83ff75bd-63eb-0b6c-18d4-6fae94050571#:~:text=In%20Control%20Panel%2C%20select%20Ease,to%20set%20up%20speech%20recognition.)

## Dependencies
This depends on my fork of [VoiceRecognitionAPI](https://github.com/NotestQ/VoiceRecognitionAPI) for Content Warning, [Mycelium](https://github.com/RugbugRedfern/Mycelium-Networking-For-Content-Warning) and my [ContentLibrary](https://github.com/NotestQ/ContentLibrary)!

## Types of content
Right now, the mod recognizes three types of content: Swearing, sponsor segments and acting like a YouTuber (SpookTuber)  
Examples: "Don't forget to share, like and subscribe!" (YouTuber) "fuck" (swearing) and "This video is sponsored by" (sponsors)  
Saying any of these keywords will trigger content, which will give you views and add a respective comment!

## Credits
Thanks a lot to:
- [leonardomurakami](https://github.com/leonardomurakami) for making translations possible and making me aware of other voice recognition software!
- [bananasov](https://github.com/bananasov) for rewriting part of my horrible code and partly helping me with getting used to C#!
