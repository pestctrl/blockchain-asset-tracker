using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAPI
{
    class MockBlockchain : BlockchainClient
    {
        IBlockchainService blockchainService;
        HttpClient client;
        public Trader thisTrader;

        public MockBlockchain(IBlockchainService blockchain) : base(blockchain)
        {
            blockchainService = blockchain;
            client = new HttpClient();
        }

        public class User
        {
            public string UserId { get; set; }
            public string password { get; set; }
        }

        List<User> users = new List<User>
        {
            new User(){ UserId = "TRADER1", password = "password"},
            new User(){ UserId = "TRADER2", password = "password"}
        };

        List<Trader> traders = new List<Trader>
        {
            new Trader(){ firstName="Tom", lastName="Vuong", objectType="URL", traderId = "TRADER1"},
            new Trader(){ firstName="Joe", lastName="Henry", objectType="URL", traderId = "TRADER2"}
        };

        List<Transaction> transactions = new List<Transaction>
        {
            new Transaction(){ latitude= 29.721115, longitude = -95.342308, newOwner="TRADER1", origOwner="TRADER2", property = "Asset A", timestamp = new DateTime(2018, 2, 5), transactionId="1", objectType="org.example.biznet.Trade"},
            new Transaction(){ latitude= 29.721115, longitude = -95.342308, newOwner="TRADER1", origOwner="TRADER2", property = "Asset B", timestamp = new DateTime(2018, 2, 6), transactionId="2", objectType="org.example.biznet.Trade"},
            new Transaction(){ latitude= 29.721115, longitude = -95.342308, newOwner="TRADER2", origOwner="TRADER1", property = "Asset C", timestamp = new DateTime(2018, 2, 8), transactionId="3", objectType="org.example.biznet.Trade"},
            new Transaction(){ latitude= 29.721115, longitude = -95.342308, newOwner="TRADER2", origOwner="TRADER1", property = "Asset D", timestamp = new DateTime(2018, 2, 11), transactionId="4", objectType="org.example.biznet.Trade"},
            new Transaction(){ latitude= 29.721115, longitude = -95.342308, newOwner="TRADER1", origOwner="TRADER2", property = "Asset E", timestamp = new DateTime(2018, 2, 12), transactionId="5", objectType="org.example.biznet.Trade"},
        };

        List<Property> properties = new List<Property>
        {
            new Property(){ description="Test", objectType="property", owner="TRADER1", PropertyId = "Asset A"},
            new Property(){ description="Test", objectType="property", owner="TRADER1", PropertyId = "Asset B"},
            new Property(){ description="Test", objectType="property", owner="TRADER2", PropertyId = "Asset C"},
            new Property(){ description="Test", objectType="property", owner="TRADER2", PropertyId = "Asset D"},
            new Property(){ description="Test", objectType="property", owner="TRADER1", PropertyId = "Asset E"},
        };



        public async Task<bool> userExists(string text)
        {
            foreach(Trader trader in traders)
            {
                if (trader.traderId == text)
                    return true;            
            }

            return false;
        }

        public async Task<bool> login(string text, string password)
        {
            foreach(User user in users)
            {
                if (user.UserId == text && user.password == password)
                    return true;
            }
            return false;
        }

        public async Task<bool> RegisterNewTrader(Trader t)
        {
            traders.Add(t);
            return true;
        }

        public async Task<bool> sendProperty(Transaction t)
        {
            transactions.Add(t);
            return true;
        }

        public async Task<bool> RegisterNewProperty(Property p)
        {
            properties.Add(p);
            return true;
        }

        public async Task<List<Property>> GetMyProperties()
        {
            List<Property> localProperties = new List<Property>();
            foreach(Property property in properties)
            {
                if (property.owner == thisTrader.traderId)
                    localProperties.Add(property);
            }

            return localProperties;
        }

        public async Task<List<Transaction>> GetUserTransactions()
        {
            List<Transaction> localTransactions = new List<Transaction>();
            foreach(Transaction transaction in transactions)
            {
                if (transaction.origOwner == thisTrader.traderId)
                    localTransactions.Add(transaction);
            }

            return localTransactions;
        }

        public async Task<List<Transaction>> GetPropertyHistory(string property)
        {
            List<Transaction> localTransactions = new List<Transaction>();
            foreach(Transaction transaction in transactions)
            {
                if (transaction.property == property)
                    localTransactions.Add(transaction);
            }

            return localTransactions;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return transactions;
        }
    }
}
