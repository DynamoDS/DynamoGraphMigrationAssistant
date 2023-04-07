using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DynamoGraphUpdater.Controls
{
    public class ConditionalCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = 0;

            if (value is ObservableCollection<UpdateableGraphsViewModel> collection)
            {
                foreach (UpdateableGraphsViewModel c in collection)
                {
                    var graphsToUpdate = c.UpdateableGraphs.Where(g => g.Update);
                    output = output + graphsToUpdate.Count();
                }
            }

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
