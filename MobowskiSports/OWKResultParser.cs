using System;

namespace Mobowski.Core.Sports
{
	public class OWKResultParser : IParser<Result>
	{

		#region IParser implementation

		public Result Parse (object data)
		{
			var result = new Result ();

			try {
				
			} catch (Exception ex) {
				throw new Exception ("failed to parse OWK result", ex);
			}

			return result;
		}

		#endregion

	}
}

