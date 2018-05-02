from seed import *

def main():
    makeTraders()
    makeProperties()
    #makeTransactions()
    makePackage("PackageA", "TRADER1", "TRADER2", ["Property B", "Property C"])
    makePackage("PackageB", "TRADER1", "TRADER2", ["Property D", "Property E", "Property F"])
    makeTransfer("PackageA", "TRADER1", "TRADER3", 29.721115, -95.342308)
    makeTransfer("PackageB", "TRADER1", "TRADER4", 29.721115, -95.342308)


if __name__ == "__main__":
    main()
