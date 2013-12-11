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
}

