using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	public Transform f;
	public Transform t;
	public float angle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var btwn = t.position - f.position;
		angle = Mathf.Atan2(btwn.y, btwn.x) * Mathf.Rad2Deg;
	}
}
