@echo off
set path=%Path%;c:\programs\nuget
nuget install Netwonsoft.Json
dir
cd packages
dir
echo "Newtonsoft installed!"
