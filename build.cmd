@echo off
echo "Hello, world!"
cd code\BlockchainApp
echo "cd was successful"
pwd
dir
NAnt.bat

exit %ERRORLEVEL%
