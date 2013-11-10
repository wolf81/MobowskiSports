using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	/// <summary>
	/// Generic interface for a parser. 
	/// </summary>
	internal interface IParser<T>
	{
		/// <summary>
		/// Parse the data into object T, e.g. if the data is a dictionary, parse the dictionary
		/// values into properties for instance of type T.
		/// </summary>
		/// <param name="t">T.</param>
		/// <param name="data">Data.</param>
		void Parse (T t, object data);
	}
}
