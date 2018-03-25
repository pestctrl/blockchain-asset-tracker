@echo off
mkdir packages
cd packages
set path=%Path%;c:\programs\nuget
nuget install Newtonsoft.Json