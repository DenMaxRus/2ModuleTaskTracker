using System;
using System.Globalization;
using System.Windows.Data;

namespace _2ModuleTaskTracker {
	public class UserPasswordConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value as string;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return "das" + (value as string);
		}
	}
}
