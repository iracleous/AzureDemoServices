﻿using AzureExtended.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
namespace AzureExtended.MyDemos;

/// <summary>
///  package  Microsoft.Azure.Cosmos
/// </summary>

internal class CosmosDemo
{

    // The Cosmos client instance
    CosmosClient? cosmosClient;

    // The database we will create
    Database? database;

    // The container we will create.
    Container? container;


    public static async Task MainCosmos(string[] args)
    {

        try
        {
            Console.WriteLine("Beginning operations...\n");
            var p = new CosmosDemo();
            await p.CosmosAsync();

        }
        catch (CosmosException de)
        {
            Exception baseException = de.GetBaseException();
            Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
        }
        finally
        {
            Console.WriteLine("End of program, press any key to exit.");
            Console.ReadKey();
        }
    }
    //The sample code below gets added below this line




    public async Task CosmosAsync()
    {
        // Create a new instance of the Cosmos Client
        cosmosClient = new CosmosClient(AzureConstants.EndpointUri,
            AzureConstants.PrimaryKey);

        // Runs the CreateDatabaseAsync method
        await CreateDatabaseAsync();

        // Run the CreateContainerAsync method
        await CreateContainerAsync();


        await AddToContainer();

        await Query("Papadopoulou");
        await Query("Guffy");

        await QueryWithLinQ();

    }

    private async Task CreateDatabaseAsync()
    {

        if (cosmosClient == null) { return; }
        // Create a new database using the cosmosClient
        database = await cosmosClient.CreateDatabaseIfNotExistsAsync(
            AzureConstants.databaseId);
        Console.WriteLine("Created Database: {0}\n", database.Id);
    }

    private async Task CreateContainerAsync()
    {
        if (database == null) { return; }
        // Create a new container
        container = await database.CreateContainerIfNotExistsAsync(
            AzureConstants.containerId, "/Country");
        Console.WriteLine("Created Container: {0}\n", container.Id);
    }



    private async Task AddToContainer()
    {

        if (container == null) { return; }
        var customer1 = new Customer
        {
            FirstName = "Anna",
            LastName = "Papadopoulou",
            Country = "Greece",
            Income = 6500,
            Emails = new() { "anna@fake.com", "annak@jmail.com" }
        };
        ItemResponse<Customer> itemResponse = await
            container.CreateItemAsync(customer1, new PartitionKey(customer1.Country));

        var customer2 = new Customer
        {
            FirstName = "Donald",
            LastName = "Guffy",
            Country = "USA",
            Income = 6500,
            Emails = new() { "donald@duck.com", "donald@jmail.com" }
        };
        ItemResponse<Customer> itemResponse2 = await
            container.CreateItemAsync(customer2, new PartitionKey(customer2.Country));

        /*
                string customerId = itemResponse2.Resource.CustId.ToString();
                var partitionKey = new PartitionKey( itemResponse2.Resource.Country);

                ItemResponse<Customer> resp = await container.ReadItemAsync<Customer>(customerId,
                    partitionKey);

                Console.WriteLine($"id = {resp.Resource.CustId} name = {resp.Resource.FirstName}");
        */
    }


    private async Task Query(string customerLastName)
    {
        if (container == null) { return; }
        QueryDefinition query = new QueryDefinition
            ("SELECT c.FirstName, c.LastName FROM Customers c WHERE c.LastName = @LastName")
          .WithParameter("@LastName", customerLastName);

        FeedIterator<Customer> customersFeed = container.GetItemQueryIterator<Customer>
            (query);
        //    (query, requestOptions: new QueryRequestOptions() { 
        //        PartitionKey = new PartitionKey("Greece"), MaxItemCount = 10 });

        while (customersFeed.HasMoreResults)
        {
            FeedResponse<Customer> customtersPage = await customersFeed.ReadNextAsync();

            foreach (Customer customer in customtersPage)
            {
                Console.WriteLine($"Retrieved customer: {customer.id}  {customer.FirstName} 	{customer.LastName}");
            }
        }
    }

    private async Task QueryWithLinQ()
    {
        if (container == null) { return; }
        IOrderedQueryable<Customer> customersQueryable = container.GetItemLinqQueryable<Customer>(); //Get IQueryable Object

        IOrderedQueryable<Customer> linqQuery = customersQueryable
        //    .Where(c => c.Country == "UK")
            .OrderBy(c => c.LastName);

        //Convert Query to Feed Enumerator
        using FeedIterator<Customer> linqFeed = linqQuery.ToFeedIterator();
        while (linqFeed.HasMoreResults) //Enumerator Result Pages
        {
            FeedResponse<Customer> linqFeedResponse = await linqFeed.ReadNextAsync();
            Console.WriteLine("---------------------");
            foreach (Customer c in linqFeedResponse)
            {
                Console.WriteLine($"LINQ Found Customer {c.id} {c.FirstName} {c.LastName}");
            }
        }
    }
}


