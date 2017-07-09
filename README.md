# SpriteSheeter
Creates a sprite sheet from chosen images. Sprite layout is very basic and assumes that all images are the same size.

Not much functionality exists in this program yet. Using it is pretty simple:
1. Open the program.
2. Click File > SelectImages.
3. Select the images you would like to be included in the sprite sheet. The program will assume all images are the same size as the first image in the selection.
4. Click "Open".
5. Once the sprite sheet has been created a save file dialog will appear and the sprite sheet can be saved.

Some things to note about the current sprite layout:
* The resulting sprite sheet will always be square.
* If the images used are Power of Two, the resulting sprite sheet will also be power of two.
* It is assumed all images to be added to the sheet are square. If images are not square, the largest dimension will be used.

More features and better implementations to come.
