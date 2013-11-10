using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class MCNResultParser : IParser<Result>
	{
		internal enum ParseMode
		{
			Club = 0,
			Team = 1
		}

		internal ParseMode Mode { get; private set; }

		internal MCNResultParser (ParseMode parseMode)
		{
			this.Mode = parseMode;
		}

		#region implemented abstract members of ResultParserBase

		public void Parse (Result result, object data)
		{
			var node = (XmlNode)data;

			switch (Mode) {
			case ParseMode.Club:
				result.HomeTeam = (string)node.SelectSingleNode ("./thuisteam").InnerText;
				result.GuestTeam = (string)node.SelectSingleNode ("./uitteam").InnerText;

				XmlNode childNode = node.SelectSingleNode ("./uitslag");
				if (childNode != null) {
					result.HomeTeamScore = Convert.ToInt32 (childNode.Attributes ["voor"].InnerText);
					result.GuestTeamScore = Convert.ToInt32 (childNode.Attributes ["tegen"].InnerText);
				}

				break;
			case ParseMode.Team:
				var childNodes = node.ChildNodes;
				if (childNodes.Count != 3) {
					throw new Exception ("parsing aborted; invalid count of child nodes: " + childNodes.Count);
				}

				string text;
				string[] items;

				// date
				node = childNodes [0];

				// team
				text = childNodes [1].InnerText;
				items = text.Split (new char[] { '-' });
				if (items.Length == 2) {
					result.HomeTeam = items [0];
					result.GuestTeam = items [1];
				} else {
					throw new Exception ("failed to parse teams from string: " + text);
				}

				// score
				text = childNodes [2].InnerText;
				items = text.Split (new char[] { '-' });
				if (items.Length == 2) {
					result.HomeTeamScore = Convert.ToInt32 (items [0]);
					result.GuestTeamScore = Convert.ToInt32 (items [1]);
				} else {
					throw new Exception ("failed to parse score from string: " + text);
				}

				break;
			}
		}

		#endregion

	}
}
