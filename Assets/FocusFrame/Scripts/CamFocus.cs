using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamFocus : MonoBehaviour {
	public CameraManager camManager;
	Vector3[] positions;
	Vector3[] eulers;

	// Use this for initialization
	void Start () {
		positions = new Vector3[]{new Vector3(-43.9f, 2.9f, -1.7f)
								,new Vector3(-17.56f, 2.92f, 1.62f)
								,new Vector3(-10.53f, 2.96f, 1.41f)
								,new Vector3(-1.06f, 1.19f, -8.6f)
								,new Vector3(15.57f, 0.75f, -8.74f)};
		eulers = new Vector3[]{new Vector3(23.8f, 25.2f, 0.0f)
							,new Vector3(20.66f, 14.03f, 0.0f)
							,new Vector3(22.72f, 10.93f, 0.0f)
							,new Vector3(32.18f, 346.35f, 0.0f)
							,new Vector3(357.11f, 9.56f, 0.0f)};
		Debug.Assert(positions.Length==eulers.Length,"positions.Length != eulers.Length");
	}
	
	public void Focus(int idx)
	{
		if(idx>=positions.Length)
		{
			Debug.Log("no pos data");
			return;
		}
		Focus(positions[idx],eulers[idx]);
	}

	public void Focus(Vector3 pos, Vector3 eulers)
	{
		camManager.SetMode(1);
		camManager.camRig.DOMove(pos,0.5f).SetEase(Ease.InOutQuart);
		camManager.camRig.DORotate(eulers,0.5f,RotateMode.Fast).SetEase(Ease.InCubic);
	}

}
