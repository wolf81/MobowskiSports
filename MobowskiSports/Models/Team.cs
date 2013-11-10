using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	public class Team
	{
		public int Identifier { get; internal set; }

		public string Name { get; internal set; }

		internal Team (IParser<Team> parser, object data)
		{
			try {
				parser.Parse (this, data);
			} catch (Exception ex) {
				throw new Exception ("failed to parse Team", ex);
			}
		}
	}
}

