using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TMS_APP.InternalMessages
{
    public class ProtocolUriMessage : ValueChangedMessage<string>
    {
        public ProtocolUriMessage(string uri) : base(uri) { }
    }
}
