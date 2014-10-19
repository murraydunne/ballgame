using UnityEngine;
using System.Collections;

public class ButtonScript: MonoBehaviour {

	protected Animator animator;

	public GameObject door;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		
		if(collision.gameObject.tag == "Player" || collision.gameObject.tag.Contains("Phys"))
		{
			animator.SetBool ("Pressed",true);
			door.GetComponent<DoorScript>().exists = false;
		}
	}
}
