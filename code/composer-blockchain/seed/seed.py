import requests
from string import ascii_uppercase

url = 'http://129.213.108.205:3000/api/org.acme.biznet.Trader'

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
            makeTrader(split[0],split[1],split[2].rstrip('\n'))

def makeProperty(i, description, ownerId):
    data = {
        "$class": "org.acme.biznet.Property", 
        "PropertyId": "Property %s" % i, 
        "description": description, 
        "owner": "TRADER%d" % ownerId
    }
    result = requests.post(url,data=data)

def makeProperties():
    for i in ascii_uppercase:
        makeProperty(i,
                     "Test description",
                     ord(i)%5+1)

def main():
    makeTraders()
    makeProperties()

if __name__ == "__main__":
    main()
