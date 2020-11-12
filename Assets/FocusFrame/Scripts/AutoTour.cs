using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTour : MonoBehaviour {
	public float speed = 2;
	Vector3[] positions = new Vector3[]{
		new Vector3(-49.57f, 2.41f, -7.18f),new Vector3(-21.97f, 2.41f, -7.18f),
		new Vector3(-20.64f, 1.87f, -14.5f),new Vector3(-20.64f, 1.87f, 15.68f),
		new Vector3(-1.11f, 1.72f, -1.71f),new Vector3(-47.6f, 1.72f, -1.71f),
		new Vector3(-14.0f, 3.01f, -12.32f),new Vector3(1.14f, 3.01f, -12.32f),
		new Vector3(9.16f, 4.41f, -3.39f),new Vector3(36.92f, 4.41f, -3.39f),
		new Vector3(-12.11f, 7.55f, -3.54f),new Vector3(53.08f, 7.5f, -3.5f),
		new Vector3(67.95f, 6.74f, 8.77f),new Vector3(67.95f, 6.74f, -12.01f),
	};
	Vector3[] eulers = new Vector3[]{
		new Vector3(20.45f, 13.94f, -0.01f),new Vector3(20.45f, 13.94f, -0.01f),
		new Vector3(18.09f, 283.74f, 0.0f),new Vector3(18.09f, 283.74f, 0.0f),
		new Vector3(0.0f, 345.0f, 0.0f),new Vector3(0.0f, 345.0f, 0.0f),
		new Vector3(27.37f, 0.0f, 0.0f),new Vector3(27.37f, 0.0f, 0.0f),
		new Vector3(15.85f, 0.0f, 0.0f),new Vector3(15.85f, 0.0f, 0.0f),
		new Vector3(46.96f, 180.0f, 0.0f),new Vector3(46.96f, 180.0f, 0.0f),
		new Vector3(41.81f, 90.0f, 0.0f),new Vector3(41.81f, 90.0f, 0.0f),
	};

	int curPathIdx=0;
	
	// Update is called once per frame
	void Update () {
		transform.position=Vector3.MoveTowards(transform.position,positions[curPathIdx*2+1],speed*Time.deltaTime);
		if(transform.position == positions[curPathIdx*2+1])
		{
			StartPath(++curPathIdx%(positions.Length/2));
		}
		
	}

	public void StartPath(int index)
	{
		// 跳转到起点
		curPathIdx=index;
		transform.position = positions[index*2];
		transform.eulerAngles = eulers[index*2];
	}

	void OnEnable()
	{
		StartPath(Random.Range(0,positions.Length/2));
	}
}
