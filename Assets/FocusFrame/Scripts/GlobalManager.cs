using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour
{
	public InputField idInput;
	public string tableName= "NodeTable";
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

    void OnEnable()
    {
    	var reader = dbMng.db.ReadFullTable(tableName);
    	while(reader.Read())
    	{
    		Node nd= new Node((string)reader["NodeID"]);
    		selector.nodes.Add(nd);
    	}
    	reader.Close();
    }
}
