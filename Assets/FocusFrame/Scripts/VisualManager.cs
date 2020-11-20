using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VisualManager : MonoBehaviour
{
	[Range(0,3)]public float targetValue = 0.3f;
	[Range(0.001f,2)]public float animTime = 0.4f;
	public int renderQueue= 2900;

	MeshRenderer[] allRdrs;

	// Start is called before the first frame update
	void Start()
	{
		List<Material> mats= new List<Material>();
		allRdrs = transform.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer render in allRdrs)
		{
			Material[] ms = render.sharedMaterials;
			foreach(Material mat in ms)
			{
				if(mats.IndexOf(mat)<0)
				{
					mats.Add(mat);
				}
			}
		}
		foreach(Material mat in mats)
		{
			mat.renderQueue= renderQueue;
		}
		foreach(var renderer in allRdrs)
		{
			var status= renderer.gameObject.AddComponent<VisualStatus>();
			status.animTime= animTime;
		}
	}

	public void OnGmObjSelected(GameObject gmObj)
	{
		MeshRenderer[] selRdrs= gmObj.GetComponentsInChildren<MeshRenderer>();
		SetAlphas(selRdrs, 1);
		SetAlphas(GetInverse(selRdrs),targetValue);
	}

	public MeshRenderer[] GetInverse(MeshRenderer[] part)
	{
		int delta= allRdrs.Length-part.Length;
		List<MeshRenderer> result= new List<MeshRenderer>();
		foreach(var rdr in allRdrs)
		{
			if(Array.IndexOf(part,rdr)<0)
			{
				result.Add(rdr);
			}
			if(result.Count==delta)
			{
				break;
			}
		}
		return result.ToArray();
	}

	public void SetAlphas(MeshRenderer[] rdrs, float alpha)
	{
		foreach(var rdr in rdrs)
		{
			VisualStatus status= rdr.GetComponent<VisualStatus>();
			status.targetAlpha=alpha;
		}
	}
}
