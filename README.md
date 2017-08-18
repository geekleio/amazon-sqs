![WBPA.Amazon.SimpleQueueService](https://nblcdn.net/themes/weubphoria.dk/nuget/wbpa-awssdk-sqs.png?v=1)

WBPA.Amazon.SimpleQueueService
----------------
The WBPA.Amazon assembly provides extensions and generic classes to help ease the usage towards AWS and is the core member of a range of wrappers tailored to the AWSSDK.

Useful links for this project:

* WBPA.Amazon.SimpleQueueService on [Nuget](https://www.nuget.org/packages/WBPA.Amazon.SimpleQueueService/)

* My profile on [LinkedIn](http://dk.linkedin.com/in/gimlichael)
* My profile on [Twitter](https://twitter.com/gimlichael)
* My profile on [StackOverflow](http://stackoverflow.com/users/175073/michael-mortensen)
* My [blog](http://www.cuemon.net/blog/)
  
  

## Installation

From the Package Manager: `Install-Package WBPA.Amazon.SimpleQueueService -Version 1.0.0.2270`.

## SendMessage Example

```csharp
using (var manager = new StandardMessageQueueManager(Endpoint, new BasicAWSCredentials(AccessKey, SecretKey)))
{
    var response = await manager.SendAsync("Message!").ConfigureAwait(false);
    // do something with the response
}
```

## SendBatchMessage Example

```csharp
using (var manager = new StandardMessageQueueManager(Endpoint, new BasicAWSCredentials(AccessKey, SecretKey)))
{
    var response = await manager.SendBatchAsync(EnumerableUtility.RangeOf(10, i => "Message {0}!".FormatWith(i))).ConfigureAwait(false);
    // do something with the response
}
```

## SendManyBatchMessage Example

```csharp
using (var manager = new StandardMessageQueueManager(Endpoint, new BasicAWSCredentials(AccessKey, SecretKey)))
{
    var responses = await manager.SendManyBatchAsync(EnumerableUtility.RangeOf(200, i => "Message {0}!".FormatWith(i))).ConfigureAwait(false);
    foreach (var response in responses)
    {
        // do something with the response
    }
}
```

## ReceiveMessage Example

```csharp
using (var manager = new StandardMessageQueueManager(Endpoint, new BasicAWSCredentials(AccessKey, SecretKey)))
{
    var response = await manager.ReceiveAsync(options =>
    {
        options.AttributeNames.GetAll();
    }).ConfigureAwait(false);
    // do something with the response
}
```

## ReceiveManyMessage Example

```csharp
using (var manager = new StandardMessageQueueManager(Endpoint, new BasicAWSCredentials(AccessKey, SecretKey)))
{
    var responses = await manager.ReceiveManyAsync(200, options =>
    {
        options.MaxNumberOfMessages = 10;
    }).ConfigureAwait(false);
    foreach (var response in responses)
    {
        // do something with the response
    }
}
```
