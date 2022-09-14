using Google.Cloud.PubSub.V1;
using Grpc.Core;

namespace PubSubEmulatorExample;

public class CreateTopicSample
{
    public Topic CreateTopic(string projectId, string topicId)
    {
        var publisher = PublisherServiceApiClient.Create();
        var topicName = TopicName.FromProjectTopic(projectId, topicId);
        Topic topic = null;

        try
        {
            topic = publisher.CreateTopic(topicName);
            Console.WriteLine($"Topic {topic.Name} created.");
        }
        catch (RpcException e) when (e.Status.StatusCode == StatusCode.AlreadyExists)
        {
            Console.WriteLine($"Topic {topicName} already exists.");
        }
        return topic;
    }
}