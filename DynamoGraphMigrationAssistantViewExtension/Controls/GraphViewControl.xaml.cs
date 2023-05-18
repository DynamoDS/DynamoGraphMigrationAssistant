using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Autodesk.DesignScript.Geometry;
using Dynamo.Utilities;
using DynamoGraphMigrationAssistant.Properties;
using Application = System.Windows.Application;

namespace DynamoGraphMigrationAssistant.Controls
{
    /// <summary>
    ///     Interaction logic for GraphViewControl.xaml
    /// </summary>
    public partial class GraphViewControl : UserControl
    {
        public GraphViewControl()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// Convert exported to image
    /// </summary>
    public class BooleanToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return ResourceUtilities.ConvertToImageSource(Resources.Progress_circle);

            var condition = (bool) value;
            if (condition) return ResourceUtilities.ConvertToImageSource(Resources.checkmark);

            return ResourceUtilities.ConvertToImageSource(Resources.Progress_circle);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Convert exported to image
    /// </summary>
    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE5E5E5"));

            var condition = (bool)value;

            if (condition)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0696D7"));
            }


            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE5E5E5"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}