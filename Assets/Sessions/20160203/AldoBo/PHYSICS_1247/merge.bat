ffmpeg -i "concat:screen0.avi"  -c copy screen.avi
ffmpeg -i screen.avi -i C:\Sessions\20160317\EugenioLonzi\PHYSICS_1247\audio.wav -c:v h264 -c:a aac -strict experimental -map 0:v:0 -map 1:a:0 screenCapture.mp4
if %errorlevel% equ 0 del screen*.avi
ffmpeg -i "concat:rgb0.avi"  -c copy rgb.avi
ffmpeg -i rgb.avi -i C:\Sessions\20160317\EugenioLonzi\PHYSICS_1247\audio.wav -c:v h264 -c:a aac -strict experimental -map 0:v:0 -map 1:a:0 KinectRGBCapture.mp4
if %errorlevel% equ 0 del rgb*.avi
if %errorlevel% equ 0 del C:\Sessions\20160317\EugenioLonzi\PHYSICS_1247\audio.wav
ffmpeg -i "concat:ir0.avi"  -c copy ir.avi
ffmpeg -i ir.avi -c:v h264 KinectIRCapture.mp4
if %errorlevel% equ 0 del ir*.avi
