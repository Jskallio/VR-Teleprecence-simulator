This is a project we made during writing our banchelor thesis: Effectiveness of a Wii Balance Board as a Locomotion Control Method for a Virtual Reality Telepresence Robot

Choosing  a  locomotion  control  method  for  a  virtual  reality  telepresence  robot
can  be  a  difficult  task  to  accomplish.   In  this  study,  we  wanted  to  explore  if
the  Wii  Balance  Board  could  be  a  suitable  locomotion  control  method  for  a
virtual reality telepresence robot. The Wii Balance Board was compared against 
joysticks,  which  were  chosen  as  they  are  one  of  the  most  common  locomotion 
control methods in virtual reality.

The project used paid asset called WiiBuddy (https://assetstore.unity.com/packages/tools/input-management/wiibuddy-4929)
Because it is paid we can not include it in the project files.
To get this project working you will have to buy the asset and install it into the project folder.

You will need a VR headset and controllers that works with SteamVR. You might have to change the controls from steam menu.

How to install and use:
Download the files and open the project in Unity 2019.12.15f1 or newer.
Install the WiiBuddy asset from the asset store.

HOW TO CONNECT TO BALANCE BOARD!!!

For the test runs:
Run the scene from Scenes/FinalScenes/StartMenu
Select Subject ID and wheter you want to start with joystick run or not and press "Start test"

First you will have to calibrate the Balance Board.
Press space to start the calibration.
When subject is standing still on board press space to calibrate the middle position
Next press space when the subject is leaning forward to calibrate the forward position
Last press space when the subject is leaning backward to calibrate tha backward position
After the calibration press space to start the first run.

Between the runs you will need to calibrate the middle position again but that is much faster than this.

After all the runs is done the game will close and you will have to open it again for further tests.

The tracking files are saved into: 
C:\Users\*USER*\AppData\LocalLow\DefaultCompany\Balance Board\

Fixing the tracking files:
We found a bug that messed up the control switches in the tracking files.
To fix this we made small script.
Open scene: Scenes/FinalScenes/FixFiles
From System-object set the "From path" and "To path" on the "Tracker Fixer"-script
  From path is place where the tracking files are
  To path is place where fixed files will be saved into.
From same object change the "Path" and "Output" of the "Output Data"-script
  Path should be the same as "To Path" of the fixer-script
  Output is csv file where you want to save the data from all the track files.

Viewing the tracker files:
Open scene: Scenes/CampusVisualizer
On Canvas-object change the "Path" in "Visalizer_File_Controller"-script into the path that you gave as "To Path" in previous part.
After that you can start the scene.
You can select track files to be viewed from "Menu" button in top right (Aspect ratio should be 16:9 or close to that)

