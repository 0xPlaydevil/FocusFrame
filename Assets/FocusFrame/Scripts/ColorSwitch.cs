using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;

public class ColorSwitch : MonoBehaviour {
    [Header("Parameter")]
    [SerializeField] bool _transparent = false;
	public bool transparent
    {
        get => _transparent;
        set
        {
            _transparent = value;
            if(onSwitchBegin!=null)
            {
                onSwitchBegin.Invoke(value);
            }
        }
    }
	[Range(0,3)]public float targetValue = 0.3f;
	[Range(0.001f,2)]public float animTime = 0.4f;
	public int renderQueue= 2900;

	[Header("Config")]
	public Transform[] gameObjs;
	[ContextMenuItem("SetTransparent","SetTransparent")]
	[ContextMenuItem("SetFade","SetFade")]
	[ContextMenuItem("SetOpaque","SetOpaque")]
	public List<Material> mats;
    public SwitchEvent onSwitchBegin;
    public SwitchEvent onSwitchEnd;


    float curValue = 1;
	// List<float> origFloats = new List<float>();
	List<Color> origColors = new List<Color>();
	Vector4 scaleV = Vector4.one;

	// Use this for initialization
	void Awake () {
		foreach(Transform trans in gameObjs)
		{
			MeshRenderer[] renders = trans.GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer render in renders)
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
		}
		mats.TrimExcess();

		foreach(Material mat in mats)
		{
			origColors.Add(mat.GetColor("_Color"));
			mat.renderQueue= renderQueue;
		}

	}
	
	// Update is called once per frame
	void Update () {
        var tmp = curValue;
		curValue = Mathf.MoveTowards(curValue,transparent? targetValue: 1,Time.deltaTime*Mathf.Abs(1-targetValue)/animTime);
		scaleV.w=curValue;

		for(int i=0;i<mats.Count;++i)
		{
			mats[i].SetColor("_Color",Vector4.Scale(origColors[i],scaleV));
			mats[i].SetFloat("_ZWrite",mats[i].GetColor("_Color").a>0.75f? 1: 0);
		}
        if(curValue==(transparent?targetValue: 1) && tmp!=curValue)
        {
            if(onSwitchEnd!=null)
            {
                onSwitchEnd.Invoke(transparent);
            }
        }
	}

	void OnDisable()
	{
		for(int i=0;i<mats.Count;++i)
		{
			mats[i].SetColor("_Color",origColors[i]);
		}
	}

	public void SwitchTransparent()
	{
		transparent = !transparent;
	}

	void SetTransparent()
	{
		foreach(Material mat in mats)
		{
			SetMaterialRenderingMode(mat,RenderingMode.Transparent);
		}
		print("SetTransparent");
	}
	void SetFade()
	{
		foreach(Material mat in mats)
		{
			SetMaterialRenderingMode(mat,RenderingMode.Fade);
		}
		print("SetFade");
	}
	void SetOpaque()
	{
		foreach(Material mat in mats)
		{
			SetMaterialRenderingMode(mat,RenderingMode.Opaque);
		}
		print("SetOpaque");
	}

    public enum RenderingMode 
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    public void SetMaterialRenderingMode (Material material, RenderingMode renderingMode)
    {
        switch (renderingMode) {
        case RenderingMode.Opaque:
            material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt ("_ZWrite", 1);
            material.DisableKeyword ("_ALPHATEST_ON");
            material.DisableKeyword ("_ALPHABLEND_ON");
            material.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
            material.SetInt("_Mode",0);
            material.SetOverrideTag("RenderType","");
            break;
        case RenderingMode.Cutout:
            material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt ("_ZWrite", 1);
            material.EnableKeyword ("_ALPHATEST_ON");
            material.DisableKeyword ("_ALPHABLEND_ON");
            material.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 2450;
            material.SetInt("_Mode",1);
            material.SetOverrideTag("RenderType","TransparentCutout");
            break;
        case RenderingMode.Fade:
            material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt ("_ZWrite", 0);
            material.DisableKeyword ("_ALPHATEST_ON");
            material.EnableKeyword ("_ALPHABLEND_ON");
            material.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            material.SetInt("_Mode",2);
            material.SetOverrideTag("RenderType","Transparent");
            break;
        case RenderingMode.Transparent:
            material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt ("_ZWrite", 0);
            material.DisableKeyword ("_ALPHATEST_ON");
            material.DisableKeyword ("_ALPHABLEND_ON");
            material.EnableKeyword ("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            material.SetInt("_Mode",3);
            material.SetOverrideTag("RenderType","Transparent");
            break;
        }
    }

    [System.Serializable]
    public class SwitchEvent : UnityEvent<bool> { }
}
