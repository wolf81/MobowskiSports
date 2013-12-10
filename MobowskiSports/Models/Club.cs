using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mobowski.Core.Sports
{
	/// <summary>
	/// A base class for a club. A club essentially has 2 internal values:
	/// 1.) a dictionary with keys and objects
	/// 2.) a value indicating the data provider we use to retrieve and parse club data
	/// </summary>
	public abstract class ClubBase
	{
		/// <summary>
		/// Parameter dictionary. Subclasses can create wrapper properties to retrieve values from 
		/// this dictionary. Depending on the service used, we might need different parameters.
		/// </summary>
		readonly protected Dictionary<string,object> _parameters;

		/// <summary>
		/// Gets the provider. Subclasses are required to implement this.
		/// </summary>
		/// <value>The provider.</value>
		internal abstract SportDataProvider Provider { get; }

		public ClubBase (Dictionary<string, object> parameters)
		{
			_parameters = parameters;
		}

		public override string ToString ()
		{
			var sb = new StringBuilder ("[Club: ");
			var i = 0;
			var keyCount = _parameters.Keys.Count;
			foreach (var key in _parameters.Keys) {
				sb.Append (String.Format ("{0}={1}", key, _parameters [key]));
				sb.Append ((++i != keyCount) ? ", " : "]");
			}
			return sb.ToString ();
		}
	}

	public class RGPOClub : ClubBase
	{
		internal override SportDataProvider Provider { get { return SportDataProvider.RGPO; } }

		public int Identifier { get { return (int)_parameters ["Identifier"]; } }

		public string Referer { get { return (string)_parameters ["Referer"]; } }

		public bool HasKVNBSource { get { return (bool)_parameters ["IsKNVBSource"]; } }

		public RGPOClub (Dictionary<string,object> parameters) : base (parameters)
		{
		}

		public static async Task<RGPOClub> RetrieveClub (string referer)
		{
			return await RGPOSportManager.RetrieveClub (referer);
		}
	}

	public class MCNClub : ClubBase
	{
		internal override SportDataProvider Provider { get { return SportDataProvider.MCN; } }

		public string Identifier { get { return (string)_parameters ["Identifier"]; } }

		public MCNClub (Dictionary<string,object> parameters) : base (parameters)
		{
		}
	}

	public class OWKClub : ClubBase
	{
		internal override SportDataProvider Provider { get { return SportDataProvider.OWK; } }

		public string Code { get { return (string)_parameters ["Code"]; } }

		public OWKClub (Dictionary<string,object> parameters) : base (parameters)
		{
		}
	}
}

