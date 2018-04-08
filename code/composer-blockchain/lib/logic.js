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
	// Update handler in package
	let pack = transfer.package;
	pack.handler = transfer.newHandler;
	let packRegistry = await getAssetRegistry('org.acme.biznet.Package');
	await packRegistry.update(pack);
	
	// Update all properties to new owner
	let fprop = pack.contents[0];
	fprop.owner = transfer.newHandler;
	let propRegistry = await getAssetRegistry('org.acme.biznet.Property');
	propRegistry.update(fprop);
}

