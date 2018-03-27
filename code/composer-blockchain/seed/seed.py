import requests
from string import ascii_uppercase

url = 'http://129.213.108.205:3000/api/org.acme.biznet.Trader'
url2 = 'http://129.213.108.205:3000/api/org.acme.biznet.Property'
url3 = 'http://129.213.108.205:3000/api/org.acme.biznet.Trade'

def makeTrader(i, fname, lname):
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
            print("Making %s %s" % (split[1], split[2]))
            makeTrader(split[0],split[1],split[2].rstrip('\n'))

def makeProperty(i, description, ownerId):
    data = {
        "$class": "org.acme.biznet.Property", 
        "PropertyId": "Property %s" % i, 
        "description": description, 
        "owner": "TRADER%d" % ownerId
    }
    result = requests.post(url2,data=data)

def makeProperties():
    for i in ascii_uppercase:
        print("Making property %s" % i)
        makeProperty(i,
                     "Test description",
                     ord(i)%5+1)

def makeTransaction(propId, t1id, t2id, longitude, latitude): 
    data = {

    }


def main():
    makeTraders()
    makeProperties()
    makeTransactions()

if __name__ == "__main__":
    main()
