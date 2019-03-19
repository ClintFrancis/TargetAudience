﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace TargetAudienceClient.Converters
{

	public class ChartDataConverter<T> : IValueConverter
	{
		public T TrueObject { set; get; }

		public T FalseObject { set; get; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? TrueObject : FalseObject;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((T)value).Equals(TrueObject);
		}
	}
}