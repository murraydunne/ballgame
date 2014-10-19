using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	public bool exists = true;
	public GameObject door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!exists) {
			foreach (Transform child in transform)
			{
				if (child.gameObject != null)
					Destroy (child.gameObject);
			}
					
		}
	}
}
