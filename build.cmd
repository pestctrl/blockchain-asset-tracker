@echo off
echo "Hello, world!"
cd code\BlockchainApp
echo "cd was successful"
call NAnt.bat

exit %ERRORLEVEL%
