# Ballance Skybox

## Ballance Skybox Generator

A stupid generator which solved the main problem of common Ballance skybox image work. In almost custom skybox image, the edge is too obvious due to unproper clip. This app use a stupid method to generae no edge skybox image to solve this problem.

This is a commandline app. The path of image is passed to app via commandline arguments. This is a example:

`BallanceSkyboxGenerator.exe the-processed-image.bmp out_back.bmp out_front.bmp out_left.bmp out_right.bmp out_down.bmp`

## Ballance Skybox Viewer

A skybox viewer to view Ballance skybox individually. So you can view skybox and check the display of edge outside of game.

There are 3 ways to load image:

* Click rectangle to open a Open File dialog and choose file one by one.
* Drop file into rectangle one by one.
* Select 5 files, and drop them into rectangle together. It requires that each item in the file series contains `down`, `back`, `front`, `left`, `right` in turn.
* Click rectangle to open a Open File dialog. Select 5 files as a file series. The file name requirements are given above.

Click `Apply` to load image into 3d viewer. Then drag mouse to change camera's direction.
