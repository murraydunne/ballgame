using UnityEngine;
using System.Collections;


public class HoverBeam : MonoBehaviour {

	// Use this for initialization
	public GameObject beamPrefab;
	private GameObject beam;
	private bool running = false;
	void Start () {
	
		animation.Play ("toIdle");
	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			Rigidbody2D ball = GameObject.Find("Ball").rigidbody2D;

			if(Mathf.Abs(ball.position.y-transform.position.y)<1f) {
				Debug.Log("wat");
				Vector2 force = new Vector2(-ball.rigidbody2D.velocity.x/2,-(ball.position.y-transform.position.y)*150);

				ball.AddForce(force);
			}
					
		}
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (running) {
			animation.Play ("toIdle");
			particleSystem.Stop ();
			Destroy(beam);

		
		} else {
			animation.Play ("toRunning");
			particleSystem.Play();
			beam = (GameObject)Instantiate(beamPrefab,transform.position,transform.rotation);
			beam.transform.parent = gameObject.transform;


		}
		running = !running;
	}
}
