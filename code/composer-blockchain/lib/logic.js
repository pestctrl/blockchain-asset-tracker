/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

'use strict';
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
async function transferPackage(transfer) {
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
