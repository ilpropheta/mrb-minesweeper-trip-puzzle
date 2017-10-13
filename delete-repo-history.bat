git checkout --orphan newBranch
rem # Add all files and commit them
git add -A  
git commit
rem # Deletes the master branch
git branch -D master  
rem # Rename the current branch to master
git branch -m master  
rem # Force push master branch to github
git push -f origin master  
rem # remove the old file
git gc --aggressive --prune=all     