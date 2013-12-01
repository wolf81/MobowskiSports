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

		private Result ParseResultClub (XmlNode node)
		{
			Result result = new Result ();

			try {
				result.HomeTeam = (string)node.SelectSingleNode ("./thuisteam").InnerText;
				result.GuestTeam = (string)node.SelectSingleNode ("./uitteam").InnerText;

				XmlNode childNode = node.SelectSingleNode ("./uitslag");
				if (childNode != null) {
					result.HomeTeamScore = Convert.ToInt32 (childNode.Attributes ["voor"].InnerText);
					result.GuestTeamScore = Convert.ToInt32 (childNode.Attributes ["tegen"].InnerText);
				}
			} catch (Exception ex) {
				throw new Exception ("failed to parse MCN result for club", ex);		
			}

			return result;
		}

		private Result ParseResultTeam (XmlNode node)
		{
			// TODO: this method could probably be cleaner ...
			Result result = new Result ();

			try {
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
			} catch (Exception ex) {
				throw new Exception ("failed to parse MCN result for team", ex);		
			}

			return result;
		}

		#region IParser implementation

		public Result Parse (object data)
		{
			var node = (XmlNode)data;
			Result result = null;

			switch (Mode) {
			case ParseMode.Club:
				result = ParseResultClub (node);
				break;
			case ParseMode.Team:
				result = ParseResultTeam (node);
				break;
			}

			return result;
		}

		#endregion

	}
}
