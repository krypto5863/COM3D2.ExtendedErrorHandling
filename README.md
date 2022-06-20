# COM3D2.ExtendedErrorHandling
A compilation of error fixes and handling that prevent the game from crashing or being stupid when encountering harmless errors. Some error fixes already existed, others were made for the first time.

# Implemented So Far
- ErrorTexturePlaceholder, ~~literally a copy paste.~~
- CCFix, ported to harmony.
- Loading a maid with a missing face or body will no longer cause a flood of errors that cause a crash by falling back to the default ones.
- Having a CategoryCreator or corrupt preset will no longer cause the preset panel to fail to open in Edit Mode. An error code in the console will display the name of any bad presets.
- Loading a menu file with a missing model file will no longer cause a crash or a looping error message. Normally setting `QuitWhenAssert` in your `config.xml` had the exact same effect but Kiss recently broke it and the message reappears infinitely effectively breaking the game. You should be cleaning your mod folder though.
- Auto-sets `QuitWhenAssert` in code. The `config.xml` property is ignored.
- Auto-creates missing game folders (those that can be empty) in order to avoid crashes when the game finds the folder missing. Critical game folders aren't recreated because if they're missing, you have a much bigger issue.
- Optional ability to load a raw png file of the same name as a missing .tex file. This is more meant for debugging or mod development and should not be relied upon for mods. Toggleable in the F1 menu or config file.

More as I encounter them.

# Usage
1. Download the Plugin.
2. (Optional) Download [COM3D2.CornerMessage](https://github.com/krypto5863/COM3D2.CornerMessage/releases)
3. Put them in `BepinEx/Plugins`
4. Done

Make sure your game and CMI or whatever are up to date at the time of the release you download. Please keep in mind EEH replaces CCFix, ErrorTexturePlaceholder, and ExceptionHusherUpper, meaning that these should be removed or you will likely encounter issues.

# Enjoy!
And don't abuse your meidos!
