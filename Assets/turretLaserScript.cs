using UnityEngine;
using System.Collections;

public class turretLaserScript : MonoBehaviour {

	private LineRenderer laser;
	private float i;
	// Use this for initialization
	void Start () {
		laser = GetComponent<LineRenderer> ();
		i = 1;
		//laser.SetPosition (0, transform.position);
		//laser.SetPosition (1, transform.position + transform.right * 5);
	}
	
	// Update is called once per frame
	void Update () {

		if (i>0) {

			Vector3 end = new Vector3(transform.position.x - (1/i)/3f,transform.position.y);
			laser.SetPosition (0,transform.position);
			laser.SetPosition(1,end); 
			laser.SetWidth ((i/2)+0.05f,(i/2)+0.05f);
		} else {
			Destroy (gameObject,0.1f);
		}
		i = i - 2f / 60f;


	}
}
