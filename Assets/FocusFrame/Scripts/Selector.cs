// 选中针对节点，节点惟一标识为ID；节点中其它属性可为空，选中逻辑不作判断；

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Selector : MonoBehaviour
{
    public GmObjEvent onGameObjectSelected= null;
    public StringEvent onNodeSelected= null;
	public string selID
	{
		get{return _selID;}
        set{
            // if(nodes.Find(node => {return node.nodeID==value;}));
            GameObject obj=null;
            try
            {
                obj= GetObjByID(value);
                _selID=value;
            }
            catch
            {
                Debug.Log("Node not found by ID: "+value);
            }
            if(_selID==value)
            {
                OnSelected(value);
                OnSelected(obj);
            }
        }
	}
	public GameObject selObj
	{
		get{return GetObjByID(_selID);}
	}

    [HideInInspector]public List<Node> nodes;
	string _selID;
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

    // public void Select(GameObject gObj)
    // {
    //     selID= GetIDByObj(nodeObj);
    // }

    void OnSelected(GameObject obj)
    {
        if(onGameObjectSelected!= null && obj!=null)
        {
            onGameObjectSelected.Invoke(obj);
        }
    }

    void OnSelected(string nodeID)
    {
        if(onNodeSelected!= null)
        {
            onNodeSelected.Invoke(nodeID);
        }
    }

    // 未查到节点或输入物体为空则报异常
    public string GetIDByObj(GameObject gObj)
    {
        if(gObj==null)
        {
            throw new ArgumentNullException();
        }
        var selNode= nodes.Find(node => {return node.gObj==gObj;});
        // return selNode!=null? selNode.nodeID: null;
        return selNode.nodeID;
    }

    // 没有查到Node报异常；
    public GameObject GetObjByID(string nodeID)
    {
        var selNode= nodes.Find(node => {return node.nodeID==nodeID;});
    	// return selNode!=null? selNode.gObj: null;
        return selNode.gObj; 
    }

    [System.Serializable]
    public class GmObjEvent:UnityEvent<GameObject>{}
    [System.Serializable]
    public class StringEvent:UnityEvent<string>{}
}

