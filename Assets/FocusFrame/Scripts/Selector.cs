using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selector : MonoBehaviour
{
    public GmObjEvent onGameObjectSelected= null;
	public string selID
	{
		get{return _selID;}
        set{
            var obj= GetObjByID(value);
            if(obj!=null)
            {
                OnSelected(obj);
                _selID=value;
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

    // public void Select(GameObject gObj)
    // {
    //     selID= GetIDByObj(nodeObj);
    // }

    void OnSelected(GameObject obj)
    {
        if(onGameObjectSelected!= null)
        {
            onGameObjectSelected.Invoke(obj);
        }
    }

    public string GetIDByObj(GameObject gObj)
    {
        var selNode= nodes.Find(node => {return node.gObj==gObj;});
        return selNode!=null? selNode.nodeID: null;
    }

    public GameObject GetObjByID(string nodeID)
    {
        var selNode= nodes.Find(node => {return node.nodeID==nodeID;});
    	return selNode!=null? selNode.gObj: null;
    }

    [System.Serializable]
    public class GmObjEvent:UnityEvent<GameObject>{}
}

