#!/bin/bash

echo "Be sure to run the 'startFabric.sh' script first!"
composer runtime install --card PeerAdmin@hlfv1 --businessNetworkName tutorial-network && 
composer network start --card PeerAdmin@hlfv1 --networkAdmin admin --networkAdminEnrollSecret adminpw --archiveFile tutorial-network@0.0.1.bna --file networkadmin.card
