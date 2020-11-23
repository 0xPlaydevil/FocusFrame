using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualStatus : MonoBehaviour
{
	// public bool isTransparent= false;
	public float targetAlpha
	{
		get{
			return _targetAlpha;
		}
		set{
			_targetAlpha= value;
			deltaAlpha= Mathf.Abs(_targetAlpha-curAlpha);
		}
	}
	public float animTime=0.3f;
	float _targetAlpha= 1;
	float deltaAlpha=0;
	float curAlpha=1;
	
	Color[] origColors;
	float[] origMetals;
	Material[] mats;
	MeshRenderer rdr;

    // Start is called before the first frame update
    void Start()
    {
        rdr=GetComponent<MeshRenderer>();
        mats= rdr.materials;
        origColors= new Color[mats.Length];
        origMetals= new float[mats.Length];
        for(int i=0;i<mats.Length;++i) 
        {
        	origColors[i]= mats[i].GetColor("_Color");
        	origMetals[i]= mats[i].GetFloat("_Metallic");
        }
    }

    void Update()
    {
    	var tmp= curAlpha;
		curAlpha = Mathf.MoveTowards(curAlpha,targetAlpha,Time.deltaTime*deltaAlpha/animTime);
		if(tmp!=curAlpha)
		{
			ApplyAlpha(curAlpha);
		}
    }

    void ApplyAlpha(float alpha)
    {
    	Vector4 scaleV= Vector4.one;
    	scaleV.w=alpha;
		for(int i=0;i<mats.Length;++i)
		{
			mats[i].SetColor("_Color",Vector4.Scale(origColors[i],scaleV));
			mats[i].SetFloat("_Metallic",origMetals[i]*alpha);
			mats[i].SetFloat("_ZWrite",mats[i].GetColor("_Color").a>0.75f? 1: 0);
		}
    }
}
