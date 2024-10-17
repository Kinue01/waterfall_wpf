using CommunityToolkit.Mvvm.Messaging.Messages;

namespace waterfall_wpf.Utils
{
    internal class TimeMessenger : ValueChangedMessage<TimeOnly>
    {
        public TimeMessenger(TimeOnly value) : base(value)
        {
        }
    }
}
