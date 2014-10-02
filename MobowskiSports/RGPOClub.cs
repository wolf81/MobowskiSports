using System;
using System.Collections.Generic;

namespace Mobowski.Core.Sports
{
	public class RGPOClub : ClubBase
	{
		internal override SportDataProvider Provider { get { return SportDataProvider.RGPO; } }

		public int Identifier { get { return (int)_parameters ["Identifier"]; } }

		public string Referer { get { return (string)_parameters ["Referer"]; } }

		public bool HasKVNBSource { get { return (bool)_parameters ["IsKNVBSource"]; } }

		public RGPOClub (Dictionary<string,object> parameters) : base (parameters)
		{
		}

		/// <summary>
		/// Interacts with the RGPO webservice to retrieve properties for a club based on the referer. 
		/// These properties are then used to construct a RGPO Club object.
		/// </summary>
		/// <returns>The club.</returns>
		/// <param name="referer">Referer.</param>
		public static RGPOClub CreateClub (string referer)
		{
			RGPOClub club = null;

			var doc = RGPOWebClient.LoadClubsXml ();
			var xpath = String.Format ("//vereniging[referer='{0}']", referer);
			var node = doc.SelectSingleNode (xpath);
			if (node != null) {
				var id = Convert.ToInt32 (node.SelectSingleNode ("id").InnerText);
				var isKnvbSource = Convert.ToInt32 (node.SelectSingleNode ("KNVBdataservice").InnerText);

				var parameters = new Dictionary<string, object> ();
				parameters.Add ("Identifier", id);
				parameters.Add ("Referer", referer);
				parameters.Add ("IsKNVBSource", (isKnvbSource == 1));

				club = new RGPOClub (parameters);
			}

			return club;
		}
	}
}

