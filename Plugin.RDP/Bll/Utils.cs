using System;

namespace Plugin.RDP.Bll
{
	internal static class Utils
	{
		public delegate String EnumToStringDelegate<T>(T mode);

		public static T EnumFromString<T>(String text, Utils.EnumToStringDelegate<T> toString)
		{
			foreach(T t in Enum.GetValues(typeof(T)))
			{
				if(text.Equals(toString(t)))
					return t;
			}
			throw new InvalidOperationException("Can't match " + typeof(T).Name + " text: " + text);
		}
	}
}