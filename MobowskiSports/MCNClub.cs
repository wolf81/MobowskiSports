using System;
using System.Collections.Generic;

namespace Mobowski.Core.Sports
{
	public class MCNClub : ClubBase
	{
		internal override SportDataProvider Provider { get { return SportDataProvider.MCN; } }

		public string Identifier { get { return (string)_parameters ["Identifier"]; } }

		public MCNClub (Dictionary<string,object> parameters) : base (parameters)
		{
		}
	}
}

