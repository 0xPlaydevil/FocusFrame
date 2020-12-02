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
        DatabaseConfig();
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
        tableName= Config.instance.GetString("DbInfo/NodeTable", tableName);
    	string str= "";
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
