using CommunityToolkit.Mvvm.ComponentModel;

namespace waterfall_wpf.Utils
{
    public class DialogViewModelBase : ObservableObject
    {
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }

        public DialogViewModelBase(DateTime date, TimeOnly time)
        {
            Date = date;
            Time = time;
        }
    }
}
