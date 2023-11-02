using AzureExtended.Models;
using StackExchange.Redis;

namespace AzureExtended.MyDemos;

// Azure Cache for Redis


// ->  needed packages  ->
// StackExchange.Redis


public static class RedisWorking
{
 
    public static void Main(string[] args)
    {
        using var redisConnection = ConnectionMultiplexer.Connect(AzureConstants.RedisConnectionString);
        IDatabase db = redisConnection.GetDatabase();

        bool wasSet = db.StringSet("favorite:flavor", "mint");
        bool wasSet2 = db.StringSet("favorite:icecream", "vanilla");

        string? value = db.StringGet("favorite:flavor");
        Console.WriteLine(value); // displays: ""i-love-rocky-road""

        var stat = new GameStat
        {
            Game = "Soccer",
            DatePlayed = new DateTime(2019, 7, 16),
            Sport = "Local Game",
            Teams = new[] { "Team 1", "Team 2" },
            Results = new[] { ("Team 1", 2), ("Team 2", 1) }
        };

        string serializedValue = Newtonsoft.Json.JsonConvert.SerializeObject(stat);
        //     bool added = db.StringSet("event:1950-world-cup", serializedValue);

        var result = db.StringGet("event:1951-world-cup");

        if (result != RedisValue.Null)
        {
            var statFromRedis = Newtonsoft.Json.JsonConvert.DeserializeObject<GameStat>(result.ToString());
            Console.WriteLine($"Sport name= {statFromRedis?.Game}");
        }
        else
        {
            Console.WriteLine("not such value");
        }
    }

}
