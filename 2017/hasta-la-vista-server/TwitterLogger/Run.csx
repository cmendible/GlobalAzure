#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage req, IAsyncCollector<MyTwitterLog> outTable, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    log.Info($"content was deserialized");

    var myTwitterLog = new MyTwitterLog();
    myTwitterLog.RowKey = Guid.NewGuid().ToString();
    myTwitterLog.PartitionKey = "twitterlog";
    myTwitterLog.TweetBy = data.TweetedBy;
    myTwitterLog.TweetText = data.TweetText;

    log.Info($"item was created item");

    await outTable.AddAsync(myTwitterLog); 

    return req.CreateResponse(HttpStatusCode.OK);
}

public class MyTwitterLog : TableEntity 
{
    public string TweetBy { get; set; }
    public string TweetText { get; set; }
}