using Google.Api.Gax;
using Google.Cloud.PubSub.V1;

namespace PubSubEmulatorExample;

public class PublishMessageAsyncSample
{
    public async Task<int> PublishMessagesAsync(string projectId, string topicId, IEnumerable<string> messagesTexts)
    {
        
        var topicName = TopicName.FromProjectTopic(projectId, topicId);
        PublisherClient publisher = await new PublisherClientBuilder
        {
            TopicName = topicName,
            EmulatorDetection = EmulatorDetection.EmulatorOnly
        }.BuildAsync();

        var publishedMessageCount = 0;
        var publishedTasks = messagesTexts.Select(async text =>
        {
            try
            {
                var message = await publisher.PublishAsync(text);
                Console.WriteLine($"Published message {message}");
                Interlocked.Increment(ref publishedMessageCount);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error ocurred when publishing message {text}: {e.Message}");
            }
        });
        await Task.WhenAll(publishedTasks);
        return publishedMessageCount;
    }
}