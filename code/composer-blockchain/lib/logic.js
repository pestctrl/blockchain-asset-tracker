/**
 * Track the trade of a commodity from one trader to another
 * @param {org.acme.biznet.Trade} trade - the trade to be processed
 * @transaction
 */
function tradeAsset(trade) {
    trade.property.owner = trade.newOwner;
    return getAssetRegistry('org.acme.biznet.Property')
        .then(function (assetRegistry) {
            return assetRegistry.update(trade.property);
        });
}
/**
 * Track the trade of a commodity from one trader to another
 * @param {org.acme.biznet.Transfer} transfer - the trade to be processed
 * @transaction
 */
function transferPackage(transfer) {
    transfer.package.handler = transfer.newHandler;
    transfer.package.contents[0].owner = transfer.newHandler;
    getAssetRegistry('org.acme.biznet.Property')
	.then(function (assetRegistry) {
	    return assetRegistry.update(transfer.package.contents[0]);
	});
    return getAssetRegistry('org.acme.biznet.Package')
	.then(function (assetRegistry) {
	    return assetRegistry.update(transfer.package);
	});
}

