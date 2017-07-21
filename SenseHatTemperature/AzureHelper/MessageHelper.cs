using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SenseHatTelemeter.Helpers;
using System.Text;

namespace SenseHatTelemeter.AzureHelper
{
    public static class MessageHelper
    {
        public static Message Serialize(object obj)
        {
            Check.IsNull(obj);

            var jsonData = JsonConvert.SerializeObject(obj);

            return new Message(Encoding.UTF8.GetBytes(jsonData));
        }

        public static RemoteCommand Deserialize(Message message)
        {
            Check.IsNull(message);

            var jsonData = Encoding.UTF8.GetString(message.GetBytes());

            return JsonConvert.DeserializeObject<RemoteCommand>(jsonData);
        }
    }
}
