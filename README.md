# LALE
Link's Awakening Level Editor Source (Rough)

The source presented is a visual studio C# solution that is poorly organized.

Please note that the Link's Awakening Level Editor is many years old and discontinued. I designed it prior to having a computer science background. This means you will see many disturbing coding practices, including tons of code reuse, poor object oriented programming practices and poor commenting practices.

This code is intended for learning purposes. GBHL.dll, InterpolationPictureBox.cs, GridBox.cs and FastPixel.cs do not belong to me.

-- LALE README --

The Link's Awakening Level Editor (LALE) is an editor created by Fatories and is designed to edit a "Legend of Zelda, The - Link's Awakening DX (U) (V1.2) [C][!]" ROM. The editor is based around this version of the game and using any other ROM could and most likely will result in many errors.

Here is a quick rundown of editing the overlay and collisions of a map:
- While on overlay editing mode, (overworld only) select a tile in the tileset located in the bottom left corner and then left click click on the map box in the center to place that tile.
- While on overlay editing mode, right clicking a tile will select that tile for quick placing.
- While on collision editing mode, (overworld and dungeons) left clicking on the map will select the collision that your mouse is overtop.
- While on collision editing mode, a double right click will remove the object you have your mouse hovered over.
- While on collision editing mode, right clicking the collision list will allow you to delete all collisions in that room.

* The draw raising floors option is meant to work as a way to tell you how adding raised floors to a map will effect your tileset on a map. However it is experimental and not full proof which means it may not change all the necessary tiles or it may even change a tile that isn't actually affected in game. Use it as a guide and a guide only. Always check in game to be sure.

Contained within this package is a file labeled "GBHL.dll". This file is necessary for the operation of LALE. Without it the program will not be able to run, so make sure to keep the file within the same directory as the LALE.exe.

"GBHL.dll" is a file created by Lin (creator of ZOLE 4) which is designed to make making an editor one hundred times easier. Without it I would have never gotten this far.

It should be noted that the Start Position Editor within LALE edits the X and Y position of Link for when he is not in the beginning bed scene. If you wish to edit the positon of Link while in bed you must use a hex editor and change these two values:
0x152B2 - Link's Start X-Pos
0x152B8 - Link's Start Y-Pos

A special thanks to Lin for supplying resources for this project and helping me when I needed it.

Thanks for reading and enjoy making a ROM hack of LA!

==Update History==

1.00 - Initial release

1.01 - Fixed an issue with collision borders crashing the program if 2-Byte objects X/Y pos were very high.

1.10 - Fixed issue with the warp editor overwriting warps when index -1 was selected.
     - Remade the text editor to draw text like the game does. Added new syntax to allow hexadecimal for text.
     - Added two new features to LALE. A sign pointer editor and an owl statue editor.

1.11 - Fixed issue with collision list not updating the ID properly (cosmetic issue).

1.20 - Fixed an issue with collision borders for special collision on the overworld not drawing properly.
     - Fixed an issue with the text editor only drawing one special icon after updating the text.
     - Fixed an issue with 3-byte collisions not loading properly if x/y were too big.
     - Fixed an issue with the minimap editor crashing if map value was larger than 0x64.
     - Fixed an issue with map 7's (overworld) map data not saving correctly.
     - Enabled map value for dungeons as well as enabled all regions to be selected for indoor maps.
     - Enabled 3-byte objects to be of length zero (no practical use, just prevents crashing).
     - Added copying, pasting and deleting features for overlay data.
     - Added new features to the dungeon minimap editor.
     - Added an export dungeon map feature.
     - Added a repoint collisions/objects feature.
     - Added an X and Y label for the current map tile you hover over.
     - Added a portal editor (this edits object 0x61's warp destinations for the overworld and dungeons).
     - Added a basic palette editor (not capable of repointing).
     - Enabled the sprite viewer/bank editor. This is in its very early stages and is capable of very little.
