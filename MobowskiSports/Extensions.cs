using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;

namespace Mobowski.Core
{
	/// <summary>
	/// Extensions for the System namespace.
	/// </summary>
	public static class SystemExtensions
	{
		public static XmlDocument LoadXml (this WebClient client, string url)
		{
			var doc = new XmlDocument ();
			try {
				var data = client.DownloadData (url);
				var stream = new MemoryStream (data);
				doc.Load (stream);
			} catch (Exception ex) {
				throw new Exception ("failed to load XML", ex);
			}
			return doc;
		}

		public static DateTime? ToDate (this string dateTimeStr, string dateFmt, CultureInfo culture) {
			const DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces;
			DateTime? result = null;
			DateTime dt;
			if (DateTime.TryParseExact (dateTimeStr, dateFmt, culture, style, out dt)) {
				result = dt;
			}

			return result;		
		}

		public static DateTime? ToDate (this string dateTimeStr, string dateFmt)
		{
			return ToDate (dateTimeStr, dateFmt, CultureInfo.InvariantCulture);
		}

		public static string AttributeValue (this XmlNode element, string node)
		{
			string result = null;

			var childNode = element.Attributes [node];
			if (childNode != null) {
				result = (childNode.InnerText != null) ? childNode.InnerText : "";
			}

			return result;
		}

		public static string NodeValue (this XmlNode element, string xpath)
		{
			string result = null;

			var childNode = element.SelectSingleNode (xpath);
			if (childNode != null) {
				result = (childNode.InnerText != null) ? childNode.InnerText : "";
			}

			return result;
		}
	}
}

