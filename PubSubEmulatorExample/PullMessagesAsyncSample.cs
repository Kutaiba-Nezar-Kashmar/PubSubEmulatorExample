using Google.Api.Gax;
using Google.Cloud.PubSub.V1;

namespace PubSubEmulatorExample;

public class PullMessagesAsyncSample
{
    public async Task<int> PullMessagesAsync(string projectId, string subscriptionId, bool acknowledge)
    {
        
        var subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);
        SubscriberClient subscriber = await new SubscriberClientBuilder
        {
            SubscriptionName = subscriptionName,
            EmulatorDetection = EmulatorDetection.EmulatorOnly
        }.BuildAsync();
        //var subscriber = await SubscriberClient.CreateAsync(subscriptionName);
        // SubscriberClient runs your message handle function on multiple
        // threads to maximize throughput.
        var messageCount = 0;
        var startTask = subscriber.StartAsync((message, _) =>
        {
            var text = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
            Console.WriteLine($"Message {message.MessageId}: {text}");
            Interlocked.Increment(ref messageCount);
            return Task.FromResult(acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack);
        });
        // Run for 5 seconds.
        await Task.Delay(5000);
        await subscriber.StopAsync(CancellationToken.None);
        // Lets make sure that the start task finished successfully after the call to stop.
        await startTask;
        return messageCount;
    }
}