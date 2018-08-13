using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour 
{
	public float rotationSpeed = 1;
	public float angle;	

	void Start () {
		angle = transform.rotation.eulerAngles.y;
	}
	
	void Update () {
		angle = (angle + rotationSpeed * Time.deltaTime) % 360;
		 
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
	}
}
