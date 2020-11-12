using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
	public string selID
	{
		get{return _selID;}
        set{
            var obj= GetObjByID(value);
            if(obj!=null)
            {
                OnSelected(obj);
                _selID=value
            }
        }
	}
	public GameObject selObj
	{
		get{return GetObjByID(_selID);}
	}

	string _selID;
    List<Node> nodes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select(string nodeID)
    {
        selID= nodeID;
    }

    // public void Select(GameObject nodeObj)
    // {
    //     selID= GetIDByObj(nodeObj);
    // }

    void OnSelected(GameObject obj)
    {

    }

    public void 

    public string GetIDByObj(GameObject gObj)
    {
        var selNode= nodes.Find<Node>(node => {return node.gObj==nodeObj;});
        return selNode!=null? selNode.nodeID: null;
    }

    public GameObject GetObjByID(string nodeID)
    {
        var selNode= nodes.Find<Node>(node => {return node.nodeID==nodeID;});
    	return selNode!=null? selNode.gObj: null;
    }
}

public class Node
{
    public string nodeID;
    public GameObject gObj;
    public string gObjPath;
}
