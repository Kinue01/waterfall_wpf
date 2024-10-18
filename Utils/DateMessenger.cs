using CommunityToolkit.Mvvm.Messaging.Messages;

namespace waterfall_wpf.Utils
{
    internal class DateMessenger : ValueChangedMessage<DateOnly>
    {
        public DateMessenger(DateOnly value) : base(value)
        {
        }
    }
}
