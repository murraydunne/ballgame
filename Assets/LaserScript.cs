using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour 
{
    private GameObject laserPrefab;

	// Use this for initialization
	void Start () 
    {
        laserPrefab = (GameObject)Resources.Load("LaserPrefab");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3 ballPos = GameObject.Find("Ball").transform.position;

            Vector3 laserVector = mousePos - ballPos;
            float angle = Mathf.Atan(laserVector.y / laserVector.x);

            angle *= (180.0f / Mathf.PI);
            angle -= 90.0f;

            if (laserVector.x < 0)
            {
                angle += 180.0f;
            }

            Quaternion laserRotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

			Vector3 laserStartPos = ballPos + laserVector.normalized * (((CircleCollider2D)GameObject.Find("Ball").collider2D).radius + 0.01f);

            GameObject laser = (GameObject)Instantiate(laserPrefab, laserStartPos, laserRotation);

            laser.GetComponent<LaserPhysicsScript>().Heading = laserVector.normalized;
        }
	}
}
