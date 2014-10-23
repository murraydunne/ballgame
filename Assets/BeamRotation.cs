using UnityEngine;
using System.Collections;

public class BeamRotation : MonoBehaviour {

	// Use this for initialization

	private float radius = 0.38f;
	private float sign = -1;
	void Start () {

		Vector2 rayOri = new Vector3 (transform.position.x + 1.025f, transform.position.y);
		Vector2 rayDir = transform.right;

		//Physics.Raycast (rayOri, rayDir, hit, 2.05f);

		if (!Physics2D.Raycast(rayOri,rayDir,2.05f)) {

			Quaternion rot = new Quaternion(0,0,transform.rotation.z,1);
			rayOri.x += transform.right.x * 1.025f;
			rayOri.y += transform.right.y * 1.025f;
			//Vector2 pos = rayOri + transform.right*2.05f;

			GameObject next = (GameObject)Instantiate(gameObject,rayOri,rot);
			next.transform.parent = gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent == null) {
			Destroy(gameObject);		
		}
		Transform top = transform.Find ("topbeam");
		Transform bot = transform.Find("botbeam");


		float topdiff = sign*(1.03f - Mathf.Abs ((top.position.y - transform.position.y)/radius))/20;

		float botdiff = -sign*(1.03f - Mathf.Abs ((top.position.y - transform.position.y)/radius))/20;

		top.Translate (0, topdiff, 0);
		bot.Translate (0, botdiff, 0);
		
		if (Mathf.Abs (top.position.y - transform.position.y) >= radius) {

			top.Translate (0, -sign*0.02f, 0);
			bot.Translate (0, sign*0.02f, 0);
				//top.position.Set(top.position.x,transform.position.y+(sign*(radius-0.1f)),0); 
				//bot.position.Set(bot.position.x,transform.position.y-(sign*(radius-0.1f)),0); 

			sign = -sign;

		}


	}
}
