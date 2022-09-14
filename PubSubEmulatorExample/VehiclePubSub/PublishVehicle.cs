using System.Text.Json;
using Google.Api.Gax;
using Google.Cloud.PubSub.V1;

namespace PubSubEmulatorExample.VehiclePubSub;

public class PublishVehicle
{
    public async Task<int> PublishVehicleAsync(string projectId, string topicId, IEnumerable<Vehicle> vehicles)
    {
        var topicName = TopicName.FromProjectTopic(projectId, topicId);
        var publisher = await new PublisherClientBuilder
        {
            TopicName = topicName,
            EmulatorDetection = EmulatorDetection.EmulatorOnly
        }.BuildAsync();

        var publishedVehicleCount = 0;
        var publishedTasks = vehicles.Select(async vehicle =>
        {
            try
            {
                var message = await publisher.PublishAsync(JsonSerializer.Serialize(vehicle));
                Console.WriteLine($"Published message {message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred when publishing message {vehicle}: {e.Message}");
            }
        });
        await Task.WhenAll(publishedTasks);
        return publishedVehicleCount;
    }
}