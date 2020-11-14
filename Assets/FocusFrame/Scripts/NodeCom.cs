using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NodeCom : MonoBehaviour
{
	public Node nodeData= null;
	public DbManager dbMng;
	public static string tableName= "NodeTable";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("SetNodeData2Db")]
    public void SetNodeObjData()
    {
    	nodeData.gObjPath= GetPath(transform);
    	nodeData.gObj= transform.gameObject;

    	if(dbMng==null)
    	{
    		dbMng=FindObjectOfType<DbManager>();
    	}
    	nodeData.Save2Db(dbMng, tableName);
    }

#if UNITY_EDITOR
    [MenuItem("Test/TestGetPath")]
    public static void TestGetPath()
    {
    	Debug.Log(GetPath(Selection.activeTransform));
    }

#endif

    public static string GetPath(Transform trans)
    {
    	StringBuilder sb=new StringBuilder();
    	GetParentsPath(trans,sb);
    	sb.Remove(sb.Length-1,1);
    	return sb.ToString();
    }

    static void GetParentsPath(Transform trans,StringBuilder sb)
    {
    	if(trans.parent!=null)
    	{
    		GetParentsPath(trans.parent,sb);
    	}
		sb.Append(trans.name);
		sb.Append('/');
    }
}

[System.Serializable]
public class Node
{
    public string nodeID;
    public GameObject gObj;
    public string gObjPath;
    public static string[] colNames= new string[]{"NodeID","NodePath"};

    public Node(string nodeID)
    {
        this.nodeID= nodeID;
    }

    public void Save2Db(DbManager dbMng,string tableName)
    {
        dbMng.db.UpdateValues(tableName,new string[]{colNames[1]},new string[]{DbManager.SQuote(gObjPath)},colNames[0],"=",DbManager.SQuote(nodeID));
    }
}
