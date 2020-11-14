using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbManager : MonoBehaviour
{
	public SQLHelper.DbType dbType;
	public string dbPath;
	public string user;
	public string password;
	public string dbName;
	public string port;

	public bool isDebug= false;

	SQLHelper _db= null;
	public SQLHelper db
	{
		get
		{
			if(_db==null)
			{
				_db= new SQLHelper(dbType,dbPath,dbName,user,password,port);
			}
			_db.debug=isDebug;
			return _db;
		}
	}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
    	_db.CloseConnection();
    }

	public static string SQuote(string orig)
	{
		return "'" + orig + "'";
	}
}
