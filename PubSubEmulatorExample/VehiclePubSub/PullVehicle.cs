using Google.Api.Gax;
using Google.Cloud.PubSub.V1;

namespace PubSubEmulatorExample.VehiclePubSub;

public class PullVehicle
{
    public async Task<int> PullVehicleAsync(string projectId, string subscriptionId, bool acknowledge)
    {
        var subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);
        SubscriberClient subscriber = await new SubscriberClientBuilder
        {
            SubscriptionName = subscriptionName,
            EmulatorDetection = EmulatorDetection.EmulatorOnly
        }.BuildAsync();
        var messageCount = 0;
        var startTask = subscriber.StartAsync((message, _) =>
        {
            var text = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
            Console.WriteLine($"Message {message.MessageId}: {text}");
            Interlocked.Increment(ref messageCount);
            return Task.FromResult(acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack);
        });
        await Task.Delay(5000);
        await subscriber.StopAsync(CancellationToken.None);
        await startTask;
        return messageCount;
    }
}