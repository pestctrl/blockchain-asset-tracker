@echo off
cd code\BlockchainApp
dir
call code\BlockchainApp\package.cmd

exit %ERRORLEVEL%
