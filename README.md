# Additive Scene Manager
Switch quickly additive scenes (levels, prefabs, UI scenes)

## 1. INTRODUCTION
We work on it different scenes most of time when create games. Same time, We want to get quickly specific prefabs, gameObject etc. when we needed.
At this point, this asset will make our job much easer. <br> By adding this asset to project. You can get...

•	Switch quickly between scenes in the same folder <br>
•	Easly add scenes in diffrent folder <br>
•	In runtime, Jumping between levels in a continuous loop (Actually, this asset is editor tool. But use to jump between levels simply) <br>


## 2. HOW TO USE

You wish, You can watch how to use tool on youtube : https://www.youtube.com/watch?v=AdGRB_8aJrQ

<img src="Assets/Additive Scene Manager/Images/Screenshot 00.png" width="500" height="600">

<img src="Assets/Additive Scene Manager/Images/Screenshot 01.png" >

It is not enough to collect our scenes under a certain folder to use this tool. <br>
Our scenes must assign to Editor Build Settings. So that you can reorder our scenes and remove specific ones.

<img src="Assets/Additive Scene Manager/Images/Screenshot 02.png">

#### 2.1. ReInitialize Button: 
It is a button which need to be pressed when added scene, changed name, appear error in console, changed of editorBuildSettings stuff etc.
#### 2.2. Remove All Additive Scene Button: 
Remove all additive scene in hierarchy
#### 2.3. SceneInfo -> Name
For information on which slider belogs to which scene. It is also used it for the Playerprefs key.
#### 2.4. SceneInfo -> FolderPath
The folder path where the scenes are located.
#### 2.5. Shortcuts (Runtime)
"P: Pause | N: Jump next level | R: Restart"
#### 2.6. Shortcuts (Editor)
Alt + A	=> Quickly open/close LevelManager <br>
Ctrl + Shift + B =>	Build Settings <br>
Alt+Ctrl+C => Copy folder path <br>

