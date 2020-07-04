using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SDEE_Editor.Miscellaneous
{
    /// <summary>
    /// Only supports FrameworkElement<br/>
    /// Created to be used in XAML
    /// </summary>
    public class ObservableCollectionFrameworkElementReverseConverter : ObservableCollectionReverseConverter<FrameworkElement> { }

    public class ObservableCollectionReverseConverter<T> : MarkupExtension, IValueConverter
    {
        private ObservableCollection<T> _reversedList;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType().GetGenericTypeDefinition() != typeof(ObservableCollection<>))
                throw new NotSupportedException("The type is not supported, please use an ObservableCollection<T>");

            _reversedList = new ObservableCollection<T>();

            var data = (ObservableCollection<T>)value;

            for (var i = data.Count - 1; i >= 0; i--)
                _reversedList.Add(data[i]);

            data.CollectionChanged += DataCollectionChanged;


            return _reversedList;
        }

        void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var data = (ObservableCollection<T>)sender;
            int datMaxIndex = data.Count - 1;
            int reversedNewStartingIndex = datMaxIndex - e.NewStartingIndex,
                reversedOldStartingIndex = datMaxIndex - e.OldStartingIndex;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T elem in e.NewItems)
                        _reversedList.Insert(reversedNewStartingIndex /*For the reverse operation*/, elem);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (T elem in e.OldItems)
                        _reversedList.Remove(elem);
                    break;

                case NotifyCollectionChangedAction.Move:
                    _reversedList.Move(reversedOldStartingIndex, reversedNewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _reversedList.Clear();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    _reversedList.Clear();
                    for (var i = datMaxIndex; i >= 0; i--)
                        _reversedList.Add(data[i]);
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
