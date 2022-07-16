using MQTTnet.Server;
using MQTTnet;
using System.Text;
using static System.Console;

// Create the options for MQTT Broker
var options = new MqttServerOptionsBuilder()
    //Set endpoint to localhost
    .WithDefaultEndpoint()
    // Port going to use 5004
    .WithDefaultEndpointPort(5004);
// Create a new mqtt server
var server = new MqttFactory().CreateMqttServer(options.Build());
//Add Interceptor for logging incoming messages
server.InterceptingPublishAsync += Server_InterceptingPublishAsync;
// Start the server
await server.StartAsync();
// Keep application running until user press a key
ReadLine();

Task Server_InterceptingPublishAsync(InterceptingPublishEventArgs arg)
{
    // Convert Payload to string
    var payload = arg.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(arg.ApplicationMessage?.Payload);


    WriteLine(
        " TimeStamp: {0} -- Message: ClientId = {1}, Topic = {2}, Payload = {3}, QoS = {4}, Retain-Flag = {5}",

        DateTime.Now,
        arg.ClientId,
        arg.ApplicationMessage?.Topic,
        payload,
        arg.ApplicationMessage?.QualityOfServiceLevel,
        arg.ApplicationMessage?.Retain);
    return Task.CompletedTask;
}