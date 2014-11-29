using System;
using Mobowski.Core.Sports;

namespace MobowskiSportsTests
{
	public class MockCacheController : ICacheController
	{
		public MockCacheController ()
		{
		}

		#region ICacheController implementation

		public string RetrieveDataFromCache (string strGuid)
		{
			return null;
		}

		public string RetrieveDataFromCache (string strGUID, DateTime datExpiration)
		{
			return null;
		}

		public byte[] RetrieveByteDataFromCache (string strGuid)
		{
			return null;
		}

		public byte[] RetrieveByteDataFromCache (string strGuid, DateTime datExpiration)
		{
			return null;
		}

		public bool StoreDataInCache (string strGuid, string strContent)
		{
			return false;
		}

		public bool StoreDataInCache (string strGUID, string strContent, DateTime datExpiration)
		{
			return false;
		}

		public bool StoreByteDataInCache (string strGuid, byte[] arrContent)
		{
			return false;
		}

		public bool StoreByteDataInCache (string strGuid, byte[] arrContent, DateTime datExpiration)
		{
			return false;
		}

		#endregion
	}
}

