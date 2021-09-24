using System;

namespace PSModule.AlmLabMgmtClient.SDK.Util
{
    [AttributeUsage(AttributeTargets.Field)]
	public class StringValueAttribute : Attribute
    {
		public string Value { get; private set; }

		public StringValueAttribute(string value)
		{
			Value = value;
		}
	}
}

