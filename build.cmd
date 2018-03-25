@echo off
cd code\BlockchainApp
dir
call package.cmd

exit %ERRORLEVEL%
