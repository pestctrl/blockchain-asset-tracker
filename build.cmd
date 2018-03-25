echo "this will run your assignment build eventually" 

cd code/BlockchainApp
call nant 

exit %ERRORLEVEL%
