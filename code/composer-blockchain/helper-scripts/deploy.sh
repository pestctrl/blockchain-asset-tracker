#!/bin/bash

echo "Be sure to run the 'startFabric.sh' script first!"
composer network install --card PeerAdmin@hlfv1 --archiveFile oracle-asset-track@0.0.1.bna
composer network start --networkName oracle-asset-track --networkVersion 0.0.1 --networkAdmin admin --networkAdminEnrollSecret adminpw --card PeerAdmin@hlfv1 --file networkadmin.card
