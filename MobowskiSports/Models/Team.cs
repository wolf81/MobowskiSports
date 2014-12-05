using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	public class Team
	{
		public int Identifier { get; internal set; }

		public string Name { get; internal set; }

		internal Team ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[Team: Identifier={0}, Name={1}]", Identifier, Name);
		}
	}
}

