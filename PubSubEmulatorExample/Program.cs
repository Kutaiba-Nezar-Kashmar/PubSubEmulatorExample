// See https://aka.ms/new-console-template for more information

using PubSubEmulatorExample;
using PubSubEmulatorExample.VehiclePubSub;

Environment.SetEnvironmentVariable("PUBSUB_EMULATOR_HOST",
    "localhost:8085"); // I would really like to not have to do this, but it is required by the emulator

var pub = new PublishMessageAsyncSample();
await pub.PublishMessagesAsync("myproject", "mytopic", new[] { "Hello ", "world" });
var sub = new PullMessagesAsyncSample();
await sub.PullMessagesAsync("myproject", "mysubscription", true);

var v1 = new Vehicle
{
    Color = "Red",
    NumberOfWeals = 4,
    DateOfManufacture = DateTime.Now,
    IsElectric = true,
    Type = "Car"
};

var v2 = new Vehicle
{
    Color = "Black",
    NumberOfWeals = 2,
    DateOfManufacture = DateTime.Now,
    IsElectric = false,
    Type = "Motorcycle"
};

var pub1 = new PublishVehicle();
await pub1.PublishVehicleAsync("myproject", "mytopic", new[] { v1, v2 });
var sub1 = new PullVehicle();
await sub1.PullVehicleAsync("myproject", "mysubscription", true);