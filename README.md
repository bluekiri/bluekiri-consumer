# ![Bluekiri](https://avatars1.githubusercontent.com/u/30432294?s=40&v=4) Bluekiri-Consumer
Bluekiri Consumer is a framework for abstracting the business layers from streaming platforms like Kafka, RabittMq or any other related. 
## Getting started (Net Core 2.x)
   + Start creating a new command-line application.
   + Install Bluekiri.Consume NuGet package from 
       + Nuget
   + Add the following lines on your configure services block if your are using Kafka:
```csharp
 .ConfigureServices((hostContext, services) =>
    {
        services.AddConsumerConfiguration<KafkaConsumer, KafkaConsumerOptions>(o =>
        {
            o.Topics.Add("test-topic");
            o.SetProperty("bootstrap.servers", "localhost:9092");
            o.SetProperty("group.id", "mygroupId");
            ...
        });
    })
```
## Producer and Consumer

## Configure Options
## New Consumers
## New Formatters
## Handlers for messages
To create new handlers you must use the abstract class MessageHander in addition to adding the attribute MessageType to the class, you can follow the next example:
```csharp
[MessageType("sample-message", typeof(Test))]
public class SampleHandler : MessageHandler
{
    public override Task HandleAsync<Test>(Test message)
    {
        return Task.CompletedTask;
    }
}
```
One of the requirements for everything to work are the headers. 
Two are necessary: **ContentType** and **MessageType**

+ **ContentType**: It is a string and defines which is the format with which we have introduced the message in the queue (Json, Protobuff, etc...). By default we use "application/json".

+ **MessageType**: is a Type and defines the type of the message.



