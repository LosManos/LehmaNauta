using System;
using System.IO;
using System.Linq;

namespace LehmaNauta.Common
{
	public static class Assert
	{
		public class Argument
		{
			private readonly string _parameterName;
			public static Argument Called( string parameterName ){
				return new Argument( parameterName );
			}
			private Argument( string parameterName ){
				_parameterName = parameterName;
			}

			public void IsNotNull( object value )
			{
				if (null == value) { throw new ArgumentNullException(_parameterName); }
			}

			public void IsNotNullAndDoesNotContain(string value, char[] chars)
			{
				IsNotNull(value);
				foreach (char c in chars)
				{
					if (value.Contains(c)) { throw new ArgumentException(string.Format("Invalid parameter. Must not contain [{0}].", c), _parameterName); }
				}
			}

			public void IsNotNullAndDoesOnlyContainFilename(string value)
			{
				IsNotNullAndDoesNotContain(
					value,
					new char[] { Path.DirectorySeparatorChar, Path.VolumeSeparatorChar }
				);
			}

		}
	}
}