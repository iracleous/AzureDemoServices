using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureExtended.MyDemos;

/*
 * 
 
    Azure.Messaging.EventHubs

 * 
 * 
 */



public class EventHubsDemo
{
    // abraeventhubs.servicebus.windows.net

    public static readonly string eventHubName = "event1";
    public static readonly string eventHubsConnectionString = "Endpoint=sb://abraeventhubs.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aoJdOGU+j9tfVPV4zPQy3CSbNHKn9TmKQ+AEhGrchP4=";

    private static readonly string _storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=potatochips;AccountKey=SyOl8W8rfXwpiMp/TxsSxG6rpRU+mKCEGSn4dfMNjiBR59TzThaTI7gQTpihUhtxg+ss1Ls6s0Ma+ASttwrnbw==;EndpointSuffix=core.windows.net";
    private static readonly string _containerName = "images";

    public static readonly string _consumerGroup = "<< NAME OF THE EVENT HUB CONSUMER GROUP >>";


    public static async Task Main(string[] args)
    {

        //Inspect Event Hubs
        await using (var producer = new EventHubProducerClient(eventHubsConnectionString, eventHubName))
        {
            string[] partitionIds = await producer.GetPartitionIdsAsync();
        }

        //Publish events to Event Hubs
        await using (var producer = new EventHubProducerClient(eventHubsConnectionString, eventHubName))
        {
            using EventDataBatch eventBatch = await producer.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(new BinaryData("First")));
            eventBatch.TryAdd(new EventData(new BinaryData("Second")));

            await producer.SendAsync(eventBatch);
        }


        await using (var consumerClient = new EventHubConsumerClient(_consumerGroup, eventHubsConnectionString, eventHubName))
        {
            using CancellationTokenSource cancellationSource = new CancellationTokenSource();

            await foreach (PartitionEvent receivedEvent in consumerClient.ReadEventsAsync(cancellationSource.Token))
            {
                Console.WriteLine($"Received event from partition {receivedEvent.Partition.PartitionId}: {Encoding.UTF8.GetString(receivedEvent.Data.Body.ToArray())}");
            }
        }


        //Read events from an Event Hubs
        string consumerGroup2 = EventHubConsumerClient.DefaultConsumerGroupName;

        await using (var consumer = new EventHubConsumerClient(consumerGroup2, eventHubsConnectionString, eventHubName))
        {
            using var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

            await foreach (PartitionEvent receivedEvent in consumer.ReadEventsAsync(cancellationSource.Token))
            {
                // At this point, the loop will wait for events to be available in the Event Hub.  When an event
                // is available, the loop will iterate with the event that was received.  Because we did not
                // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
                // the cancellation token.
            }
        }

        //Read events from an Event Hubs partition
        //   string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

        await using (var consumer = new EventHubConsumerClient(consumerGroup2, eventHubsConnectionString, eventHubName))
        {
            EventPosition startingPosition = EventPosition.Earliest;
            string partitionId = (await consumer.GetPartitionIdsAsync()).First();

            using var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

            await foreach (PartitionEvent receivedEvent in consumer.ReadEventsFromPartitionAsync(partitionId, startingPosition, cancellationSource.Token))
            {
                // At this point, the loop will wait for events to be available in the partition.  When an event
                // is available, the loop will iterate with the event that was received.  Because we did not
                // specify a maximum wait time, the loop will wait forever unless cancellation is requested using
                // the cancellation token.
            }
        }
    }


    public async Task ProcessEvents()
    {
        var cancellationSource = new CancellationTokenSource();
        cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

        Task processEventHandler(ProcessEventArgs eventArgs) => Task.CompletedTask;
        Task processErrorHandler(ProcessErrorEventArgs eventArgs) => Task.CompletedTask;

        var storageClient = new BlobContainerClient(_storageConnectionString, _containerName);
        var processor = new EventProcessorClient(storageClient, _consumerGroup,
            eventHubsConnectionString, eventHubName);

        processor.ProcessEventAsync += processEventHandler;
        processor.ProcessErrorAsync += processErrorHandler;

        await processor.StartProcessingAsync();

        try
        {
            // The processor performs its work in the background; block until cancellation
            // to allow processing to take place.
            await Task.Delay(Timeout.Infinite, cancellationSource.Token);
        }
        catch (TaskCanceledException)
        {
            // This is expected when the delay is canceled.
        }

        try
        {
            await processor.StopProcessingAsync();
        }
        finally
        {
            // To prevent leaks, the handlers should be removed when processing is complete.

            processor.ProcessEventAsync -= processEventHandler;
            processor.ProcessErrorAsync -= processErrorHandler;
        }
    }
}