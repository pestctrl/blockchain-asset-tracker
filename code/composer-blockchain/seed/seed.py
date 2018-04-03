import requests
from string import ascii_uppercase

url = 'http://129.213.108.205:3000/api/org.acme.biznet.Trader'
url2 = 'http://129.213.108.205:3000/api/org.acme.biznet.Property'
url3 = 'http://129.213.108.205:3000/api/org.acme.biznet.Trade'
url4 = 'http://129.213.108.205:3000/api/org.acme.biznet.Package'

def makeTrader(i, fname, lname):
    print("Making %s %s" % (fname, lname))
    data = { 
        "$class": "org.acme.biznet.Trader", 
        "traderId": "TRADER%s" % i, 
        "firstName": fname, 
        "lastName": lname 
    }
    response = requests.post(url,data=data)

def makeTraders():
    with open("traders.txt","r") as file:
        for line in file:
            split = line.split(" ")
            makeTrader(split[0],split[1],split[2].rstrip('\n'))

def makeProperty(i, description, ownerId):
    print("Making property %s" % i)
    data = {
        "$class": "org.acme.biznet.Property", 
        "PropertyId": "Property %s" % i, 
        "description": description, 
        "owner": "TRADER%d" % ownerId
    }
    result = requests.post(url2,data=data)

def makeProperties():
    for i in ascii_uppercase:
        makeProperty(i,
                     "Test description",
                     ord(i)%5+1)

def makeTransaction(propId, t1id, t2id, latitude, longitude): 
    print("Sending %s to %s" % (propId, t2id))
    data = {
        "property" : propId,
        "origOwner" : t1id,
        "newOwner" : t2id,
        "latitude" : latitude,
        "longitude" : longitude,
    }
    result = requests.post(url3, data=data)

def makePackage(packId, handler, sender, recipient, contents):
    data ={
	"PackageId": packId,
	"handler": handler,
	"sender": sender,
	"recipient": recipient,
	"contents": contents
    }
    result = requests.post(url4, data=data)

def makeTransactions():
    makeTransaction("Property A", "TRADER1", "TRADER2", 29.721115, -95.342308)
    makeTransaction("Property A", "TRADER2", "TRADER1", 29.722037, -95.349048)
    makeTransaction("Property A", "TRADER1", "TRADER3", 29.725520, -95.348388)

def main():
    makeTraders()
    makeProperties()
    makeTransactions()
    makePackage("PackageA", "TRADER1", "TRADER1", "TRADER2", ["Property A", "Property B"]);

if __name__ == "__main__":
    main()
