using System;
using System.Collections.Generic;

namespace Mobowski.Core.Sports
{
	public class OWKClub : ClubBase
	{
		internal override SportDataProvider Provider { get { return SportDataProvider.OWK; } }

		public string Code { get { return (string)_parameters ["Code"]; } }

		public OWKClub (Dictionary<string,object> parameters) : base (parameters)
		{
		}
	}
}

