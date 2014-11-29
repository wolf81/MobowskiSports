using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobowski.Core.Sports {
    public interface ICacheController {
        string RetrieveDataFromCache(string strGuid);

        string RetrieveDataFromCache(string strGUID, DateTime datExpiration);

        byte[] RetrieveByteDataFromCache(string strGuid);

        byte[] RetrieveByteDataFromCache(string strGuid, DateTime datExpiration);

        bool StoreDataInCache(string strGuid, string strContent);

        bool StoreDataInCache(string strGUID, string strContent, DateTime datExpiration);

        bool StoreByteDataInCache(string strGuid, byte[] arrContent);

        bool StoreByteDataInCache(string strGuid, byte[] arrContent, DateTime datExpiration);
    }
}