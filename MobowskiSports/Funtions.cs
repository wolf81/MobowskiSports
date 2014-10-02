using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobowskiSports {
    public static class Funtions {
        public static object RetrieveDataFromCache(string strGUID, DateTime datExpiration = null) {
            if (datExpiration == null)
                datExpiration = DateTime.Now.AddHours(1);

            clsCacheStore objCache = new clsCacheStore();
            string strReturn = null;

            var _with1 = objCache;
            _with1.ConnectionString = Connection.GetConnectionString();
            _with1.ExpirationDate = datExpiration;
            _with1.GUID = strGUID;

            if (_with1.CacheRetrieve()) {
                strReturn = _with1.Content;
            }

            return strReturn;
        }

        public static bool StoreDataInCache(string strGUID, string strContent, DateTime datExpiration = null) {
            bool blnSuccess = false;
            clsCacheStore objCache = new clsCacheStore();

            if (datExpiration == null)
                datExpiration = DateTime.Now.AddHours(1);

            var _with2 = objCache;
            _with2.ConnectionString = Connection.GetConnectionString();
            _with2.ExpirationDate = datExpiration;
            _with2.GUID = strGUID;
            _with2.Content = strContent;

            blnSuccess = _with2.CacheInsert();

            return blnSuccess;
        }


  //        Shared Function RetrieveDataFromCache(ByVal strGUID As String, Optional ByVal datExpiration As DateTime = Nothing) As Object
  //  If datExpiration = Nothing Then datExpiration = DateTime.Now.AddHours(1)

  //  Dim objCache As New clsCacheStore
  //  Dim strReturn As String = Nothing

  //  With objCache
  //    .ConnectionString = Connection.GetConnectionString()
  //    .ExpirationDate = datExpiration
  //    .GUID = strGUID

  //    If .CacheRetrieve() Then
  //      strReturn = .Content
  //    End If
  //  End With

  //  Return strReturn
  //End Function

  //Shared Function StoreDataInCache(ByVal strGUID As String, ByVal strContent As String, Optional ByVal datExpiration As DateTime = Nothing) As Boolean
  //  Dim blnSuccess As Boolean
  //  Dim objCache As New clsCacheStore

  //  If datExpiration = Nothing Then datExpiration = DateTime.Now.AddHours(1)

  //  With objCache
  //    .ConnectionString = Connection.GetConnectionString()
  //    .ExpirationDate = datExpiration
  //    .GUID = strGUID
  //    .Content = strContent

  //    blnSuccess = .CacheInsert()
  //  End With

  //  Return blnSuccess
  //End Function

    
    
    }
}
