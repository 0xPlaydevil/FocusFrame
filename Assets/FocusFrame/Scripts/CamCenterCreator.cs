using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCenterCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	var rdrs= GetComponentsInChildren<MeshRenderer>();
    	foreach(var rdr in rdrs)
    	{
    		rdr.gameObject.AddComponent<MeshCollider>();
    	}
    }
}
