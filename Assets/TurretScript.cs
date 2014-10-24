using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {
	public LayerMask playerMask;
	public GameObject turretLaser;
	private bool shooting = false;
	private float shootTime;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!shooting && Physics2D.Raycast(transform.position,-transform.right,8,playerMask)) {
			shooting = true;
			shootTime = Time.time;
			Debug.Log(transform.position);
		
			Instantiate(turretLaser,transform.position-transform.position/2,transform.rotation);

		}
		if (Time.time>shootTime + 1) {
				shooting = false;
		}
	}
}
