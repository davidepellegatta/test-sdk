    using System;
    using Couchbase;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Extensions.Logging;

    namespace test_sdk
    {
        class Program
        {
            [Obsolete]
            public static async System.Threading.Tasks.Task Main(string[] args)
            {
                Console.WriteLine("Hello World!");


                ILoggerFactory loggerFactory = new LoggerFactory();

                _ = loggerFactory.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });

                LogManager.LoadConfiguration("Nlog.config");

                var options = new ClusterOptions()
                {
                    Logging = loggerFactory,
                    UserName = "Administrator",
                    Password = "couchbase",
                    ConnectionString = "http://localhost"
                };

                var cluster = await Cluster.ConnectAsync(options);
                var bucket = await cluster.BucketAsync("travel-sample");
                var collection = bucket.DefaultCollection();

                var result = await cluster.QueryAsync<dynamic>(
                    "SELECT t.* FROM `travel-sample` t WHERE t.type=$1",
                    options => options.Parameter("landmark")
                );

                await foreach (var row in result)
                {
                    // each row is an instance of the Query<T> call (e.g. dynamic or custom type)
                    var name = row.name;        // "Hollywood Bowl"
                    var address = row.address;      // "4 High Street, ME7 1BB"
                    Console.WriteLine($"{name},{address}");
                }

                Console.WriteLine(result.Rows.ToString());

                await collection.UpsertAsync<string>("beer-sample-101", "logging example 101");

                var returnVal = await collection.GetAsync("beer-sample-101");

                Console.WriteLine(returnVal.ContentAs<string>());
            
            }
        }


    }
