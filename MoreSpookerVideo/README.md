# More Spooker Video

*A mod adding camera video in shop. You can also activate all the items and make it all free (in config file). 
You can set the starting currency, or even increase the camera recording time and increase the number of views to reach per quota.*

## Notes
- Required by all players to work properly!
- In fact the client is not required to download the mod for it to work, but only the host will have control over the prices of items. 
The mod is necessary for the client if they also want to purchase items.
- You can edit the settings either from the config file or from the 'edit config' tab on Thunderstore once a profile is selected.

## Requirements
- BepInEx LTS

## Bugs
- When uploading your film, it's possible that the end interface might appear. 
Refreshing the playback of the film unlocks it in most cases. The view count is still added to the total!
- When you unlocked and activated all items, there are certain tabs where the list is too long and extends beyond the screen (no scrollbar).
- The error message **"[Warning: Unity Log] Couldn't create a Convex Mesh from source mesh "Cube" within the maximum polygons limit (256). The partial hull will be used. Consider simplifying your mesh.'** appears when the 'Unlock all items' option is active.
It can block the game's launch. The only workaround I've 'found' so far is to force close the game or close the terminal. Then, relaunch the game with the active mods (usually it works), otherwise relaunch at least once without mods before.
This happens during the processing of the list of items when retrieving the *'ItemDatabase'* instance.
