rd leavventuredimrb.git

git clone --mirror https://epancini@bitbucket.org/epancini/leavventuredimrb.git

rem using bfg-1.12.14
java -jar bfg.jar --delete-files *.psd leavventuredimrb.git
java -jar bfg.jar --delete-files *.avi leavventuredimrb.git
java -jar bfg.jar --delete-files *.mov leavventuredimrb.git
java -jar bfg.jar --delete-files *.mp4 leavventuredimrb.git
java -jar bfg.jar --delete-files *.mp3 leavventuredimrb.git
java -jar bfg.jar --delete-files *.ogg leavventuredimrb.git
java -jar bfg.jar --delete-files *.wav leavventuredimrb.git

cd leavventuredimrb.git

git reflog expire --expire=now --all 
git gc --prune=now --aggressive 
git push
pause