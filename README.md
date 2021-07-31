# COM3D2.ExtendedErrorHandling
A compilation of error fixes and handling that prevent the game from crashing or being stupid when encountering harmless errors. Some error fixes already existed, others were made for the first time.

# Implemented So Far
- ErrorTexturePlaceholder, literally a copy paste.
- CCFix, ported to harmony.
- Loading a maid with a missing face or body will no longer cause a flood of errors that cause a crash by falling back to the default ones.
- Having a CategoryCreator or corrupt preset will no longer cause the preset panel to fail to open in Edit Mode. An error code in the console will display the name of any bad presets.
- Loading a menu file with a missing model file will no longer cause a crash or a looping error message. Normally setting `QuitWhenAssert` in your `config.xml` had the exact same effect but Kiss recently broke it and the message reappears infinitely effectively breaking the game. You should be cleaning your mod folder though.
- Auto-sets `QuitWhenAssert` in code. The `config.xml` property is ignored.

More as I encounter them.
