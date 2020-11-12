using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class FirstViewWalk : MonoBehaviour {
	public float maxSpeed = 3;
	public float accelerate = 5f;
	MonoBehaviour subControl;
	Transform trans;
	Vector2 curVel = Vector2.zero;

	// Use this for initialization
	void Awake () {
		subControl = transform.GetComponent<FreeLookCam>();
		trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		float run = Input.GetButton("Fire3") ?2.5f :1;
		curVel=Vector2.MoveTowards(curVel, run*maxSpeed*new Vector2(h,v),run*accelerate*Time.deltaTime);
		trans.Translate(new Vector3(curVel.x,0,curVel.y)*Time.deltaTime);

	}

	void OnEnable()
	{
		subControl.enabled= true;
	}

	void OnDisable()
	{
		subControl.enabled= false;
	}

}
