@echo off
set path=%Path%;c:\programs\nuget
nuget install Netwonsoft.JSON=
dir
cd packages
dir
echo "Newtonsoft installed!"
