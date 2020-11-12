using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour {
	public Transform camTrans;
	public Transform camRig;
	Vector3 origPos;
	Vector3 origRot;
	Vector3 targetPos;
	Vector3 targetRot;
	int curState = -1;
	MonoBehaviour[] modeComs;
	bool adjusting = false;
	float deltaAngle = 0;

	public UnityEvent onStartAnim= new UnityEvent();
	public UnityEvent onFinishAnim= new UnityEvent();

	// Use this for initialization
	void Start () {
		modeComs = new MonoBehaviour[3];
		modeComs[0] = camRig.GetComponent<FirstViewWalk>();
		modeComs[1] = camRig.GetComponent<FreeCamMove>();
		modeComs[2] = camRig.GetComponent<AutoTour>();
		origPos = camRig.position;
		origRot = camRig.eulerAngles;

		SetMode(1);
	}
	
	// Update is called once per frame
	void Update () {
		if(adjusting)
		{
			// TODO:调整
			camTrans.localPosition = Vector3.Lerp(camTrans.localPosition,Vector3.zero,4*Time.deltaTime);
			// camTrans.localRotation = Quaternion.RotateTowards(camTrans.localRotation,Quaternion.identity,1/(Quaternion.Angle(camTrans.localRotation,Quaternion.identity)/10+0.01f));
			camTrans.localRotation = Quaternion.RotateTowards(camTrans.localRotation,Quaternion.identity,0.8f*Time.deltaTime*deltaAngle*deltaAngle/(Quaternion.Angle(camTrans.localRotation,Quaternion.identity)+0.001f));
			if(camTrans.localPosition==Vector3.zero && camTrans.localEulerAngles==Vector3.zero)
			{
				camTrans.localPosition=Vector3.zero;
				camTrans.localEulerAngles=Vector3.zero;
				adjusting = false;
				if(onFinishAnim!=null)
				{
					onFinishAnim.Invoke();
					onFinishAnim.RemoveAllListeners();
				}
			}
		}
	}

	public void ResetTrans()
	{
		camRig.position = origPos;
		camRig.eulerAngles = origRot;
		camRig.GetChild(0).localRotation=Quaternion.identity;
		camRig.GetChild(0).localPosition=Vector3.zero;
		camTrans.localPosition=Vector3.zero;
		camTrans.localRotation=Quaternion.identity;
		SetMode(1);
	}

	public void SetTrans(Vector3 pos, Vector3 eulers)
	{
		camRig.position = pos;
		camRig.eulerAngles = eulers;
		camRig.GetChild(0).localRotation=Quaternion.identity;
		camRig.GetChild(0).localPosition=Vector3.zero;
		camTrans.localPosition=Vector3.zero;
		camTrans.localRotation=Quaternion.identity;
	}

	public void AlignRig()
	{
		camRig.position=camTrans.position;
		camRig.rotation=camTrans.rotation;
		camRig.GetChild(0).localRotation=Quaternion.identity;
		camRig.GetChild(0).localPosition=Vector3.zero;
		camTrans.localPosition=Vector3.zero;
		camTrans.localRotation=Quaternion.identity;
	}

	public int GetMode()
	{
		return curState;
	}

	public void SetMode(int code)
	{
		if(curState==code)	return;
		SetModeCom(-1);
		switch(code)
		{
			case 0:	// 第一人称
				RaycastHit hit;
				if(camTrans.eulerAngles.x>15 && Physics.Raycast(camTrans.position,camTrans.forward,out hit))
				{
					targetPos = hit.point;
					targetPos.y = 1.8f;
					targetRot = Vector3.up*(camTrans.eulerAngles.y>180 ?270 :90);
				}
				else
				{
					targetPos = camTrans.position;
					targetPos.y = 1.8f;
					targetRot = Vector3.up*camTrans.eulerAngles.y;
				}
				ChangeAndAnim(targetPos,targetRot,()=>SetModeCom(code));
				break;
			default:	// 自由视角和自动视角
				ChangeAndAnim(camTrans.position,camTrans.eulerAngles,()=>SetModeCom(code));
				break;
		}
		// SetModeCom(code);
	}

	public void ChangeAndAnim(Vector3 targetPos, Vector3 targetRot, UnityAction callback=null)
	{
		Vector3 camPos = camTrans.position;
		Vector3 camRot = camTrans.eulerAngles;
		camRig.position = targetPos;
		camRig.eulerAngles = targetRot;
		camRig.GetChild(0).localEulerAngles = Vector3.zero;
		camTrans.position = camPos;
		camTrans.eulerAngles = camRot;
		deltaAngle = Quaternion.Angle(camTrans.localRotation,Quaternion.identity);
		adjusting = true;
		if(callback!=null)
		{
			onFinishAnim.AddListener(callback);
		}
		if(onStartAnim!=null) onStartAnim.Invoke();
	}

	void SetModeCom(int code)
	{
		for(int i=0;i<modeComs.Length;++i)
		{
			modeComs[i].enabled = i==code ?true :false;
		}
		curState=code;
	}
}
