using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GlobalManager : MonoBehaviour
{
	public InputField idInput;
	public string tableName= "NodeTable";
	public GameObject testCanvas= null;
	Selector selector;
	CamFocusData camData;
	Transform camRig;
	DbManager dbMng;
    // Start is called before the first frame update
    void Awake()
    {
        selector= GetComponent<Selector>();
        camData= GetComponent<CamFocusData>();
        camRig= FindObjectOfType<FreeCamMove>().transform;
        dbMng= GetComponent<DbManager>();
    }

    public void OnBtnSetNode()
    {
    	camData.SetNode(idInput.text,camRig.position,camRig.eulerAngles);
    }

    public void OnBtnSelect()
    {
    	selector.selID= idInput.text;
    }

    public void OnNodeSelected(GameObject gmObj)
    {
    	print(gmObj.name+ " "+ gmObj.GetComponent<NodeCom>().nodeData.nodeID);
    }

    public void DatabaseConfig()
    {
    	string str= "";
        str= Config.instance.GetString("DbInfo/Server", "");
        if (str != "") dbMng.dbPath = str;
        str= Config.instance.GetString("DbInfo/User", "");
        if (str != "") dbMng.user = str;
        str= Config.instance.GetString("DbInfo/Password", "");
        if (str != "") dbMng.password = str;
        str= Config.instance.GetString("DbInfo/DbName", "");
        if (str != "") dbMng.dbName = str;
        str= Config.instance.GetString("DbInfo/Port", "");
        if (str != "") dbMng.port = str;
        str= Config.instance.GetString("DbInfo/NodeTable", "");
        if (str != "") camData.tableName= tableName = str;
        str= Config.instance.GetString("DbInfo/NodeInfo/ObjPath", "");
        if (str != "") Node.colNames[1] = str;
    }

    public void TestCanvasConfig()
    {
    	if(testCanvas)
    	{
    		testCanvas.SetActive(Config.instance.GetBool("TestCanvas",false));
    	}
    }

    void OnEnable()
    {
        DatabaseConfig();
        TestCanvasConfig();

    	var reader = dbMng.db.ReadFullTable(tableName);
    	while(reader.Read())
    	{
    		Node nd= new Node((string)reader[Node.colNames[0]]);
    		try
    		{
	    		nd.gObjPath= (string)reader[Node.colNames[1]];
	    		nd.gObj= GameObject.Find("/"+nd.gObjPath);
	    		if(!nd.gObj)
	    		{
	    			Debug.LogWarning("GameObject not found: "+nd.gObjPath);
	    		}
    		}
    		catch(InvalidCastException e)
    		{}
    		catch(IndexOutOfRangeException e)
    		{
    			Debug.LogErrorFormat("Column `{0}` not found in database table `{1}`", Node.colNames[1], tableName);
    		}
    		catch(Exception e)
    		{
    			Debug.LogWarning(e.Message);
    		}
    		selector.nodes.Add(nd);
    	}
    	reader.Close();
    }
}
