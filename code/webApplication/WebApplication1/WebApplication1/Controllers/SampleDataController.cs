using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        private static string[] Entries = new[]
        {
            "ActivateCurrentIdentity", "StartBusinessNetwork", "IssueIdentity", "AddParticipant"
        };
        private static string[] Participants = new[]
        {
            "Xing", "Giang", "Nghia", "Benson", "Son"
        };

        [HttpGet("[action]")]
        public IEnumerable<TransactionHistory> TransactionHistories()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new TransactionHistory
            {
                GetDate = DateTime.Today.ToString("d"),
                GetTime = DateTime.Today.ToString(),
                Entry = Entries[rng.Next(Entries.Length)],
                Participant = Participants[rng.Next(Participants.Length)]
            });
        }

        public class TransactionHistory
        {
            public string GetDate { get; set; }
            public string GetTime { get; set; }
            public string Entry { get; set; }
            public string Participant { get; set; }
        }


        private static string[] ClassIds = new[]
        {
            "ActivateCurrentIdentity", "StartBusinessNetwork", "IssueIdentity", "AddParticipant"
        };
        private static string[] AssetIds = new[]
        {
            "Xing", "Giang", "Nghia", "Benson", "Son"
        };

        [HttpGet("[action]")]
        public IEnumerable<Assets> AssetsInfo()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Assets
            {
                ClassId = ClassIds[rng.Next(ClassIds.Length)],
                AssetId = AssetIds[rng.Next(AssetIds.Length)],
                Owner = "Mik",
                Value = "2300"
            });
        }

        public class Assets
        {
            public string ClassId { get; set; }
            public string AssetId { get; set; }
            public string Owner { get; set; }
            public string Value { get; set; }
        }
    }
}
