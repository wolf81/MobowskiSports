using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class CacheStore {
    private string _tableName;
    private DateTime _expirationData;
    private string _guid;
    private string _content;
    private Database _database;
    private int _cacheID;
    private string _connectionString;

    private bool _cacheCleared;
    public CacheStore() {
    }

    public void InitConnection() {
        if (_database == null) {
            _database = new Database();
            _database.ConnectionString = _connectionString;
        }

        CacheClear();
    }

    public int CacheID {
        get { return _cacheID; }
    }

    public string ConnectionString {
        set { _connectionString = value; }
    }

    public string TableName {
        set { _tableName = value; }
    }

    public DateTime ExpirationDate {
        get { return _expirationData; }
        set { _expirationData = value; }
    }

    public string GUID {
        get { return _guid; }
        set { _guid = value; }
    }

    public string Content {
        get { return _content; }
        set { _content = value; }
    }

    private void DataTableToMembers(DataTable tblData) {
        if (tblData != null && tblData.Rows.Count == 1) {
            DataRow rowData = tblData.Rows[0];

            _guid = rowData["CacheGUID"].ToString();
            _cacheID = Convert.ToInt32(rowData["CacheID"]);
            _expirationData = Convert.ToDateTime(rowData["ExpirationDate"]);
            _content = rowData["CacheContent"].ToString();
        }
    }

    private void CacheClear() {
        if (_cacheCleared) { return; }

        _database.ExecuteNonQuery("DELETE FROM dbo.tblCache WHERE ExpirationDate < GetDate()");
        _cacheCleared = true;
    }

    public bool CacheRetrieve() {
        InitConnection();

        bool blnReturn = false;
        DataTable tblData = null;

        var _with1 = _database;
        if (_cacheID > 0) {
            _with1.AddParameter("@CacheID", _cacheID);
            tblData = _with1.GetDataTable("SELECT * FROM dbo.tblCache WHERE CacheID = @CacheID");
        } else {
            _with1.AddParameter("@CacheGUID", _guid);
            tblData = _with1.GetDataTable("SELECT * FROM dbo.tblCache WHERE CacheGUID = @CacheGUID");
        }

        if (tblData == null || tblData.Rows.Count != 1) {
            blnReturn = false;
        } else {
            DataTableToMembers(tblData);
            blnReturn = true;
        }

        return blnReturn;
    }

    public bool CacheInsert() {
        InitConnection();

        bool blnReturn = false;

        _database.AddParameter("@CacheGUID", _guid);
        _database.AddParameter("@CacheContent", _content);
        _database.AddParameter("@ExpirationDate", _expirationData);

        _cacheID = Convert.ToInt32(_database.ExecuteScalar("INSERT INTO dbo.tblCache (CacheGUID, CacheContent, ExpirationDate) VALUES (@CacheGUID, @CacheContent, @ExpirationDate)"));

        blnReturn = Convert.ToBoolean(_cacheID);

        return blnReturn;
    }
}