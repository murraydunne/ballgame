using UnityEngine;
using System.Collections;


public class HoverBeam : MonoBehaviour {

	// Use this for initialization

	public float sensitivityZone;
	public float xSpeed;
	public GameObject beamPrefab;
	private GameObject beam;
	public bool running;
	void Start () {
		if (!running) {
			animation.Play ("toIdle");
			particleSystem.Stop ();
			//Destroy(beam);
			
			
		} else {
			animation.Play ("toRunning");
			particleSystem.Play();
			beam = (GameObject)Instantiate(beamPrefab,transform.position,transform.rotation);
			beam.transform.parent = gameObject.transform;
			
			
		}
		//running = !running;
		//animation.Play ("toIdle");
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody2D ball = GameObject.Find("Ball").rigidbody2D;
		if (running) {


			if(Mathf.Abs(ball.position.y-transform.position.y)<sensitivityZone) {

				ball.gravityScale = 0f;
				Vector2 force = new Vector2(-ball.velocity.x/2+xSpeed,(transform.position.y - ball.position.y)*5 - ball.velocity.y*3);
				//ball.velocity.Set(3*(ball.velocity.x + xSpeed)/4,transform.position.y-ball.position.y);

				ball.AddForce(force);
			} else {
				ball.gravityScale = 1f;
			}
					
		} else {
			ball.gravityScale = 1f;
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
