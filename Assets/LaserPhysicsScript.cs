using UnityEngine;
using System.Collections;

public class LaserPhysicsScript : MonoBehaviour
{
    public Vector3 Heading { get; set; }

    private const float hitPlayerMagnitude = 400.0f;
    private const float speed = 0.17f;

    // Use this for initialization
    void Start()
    {
        Invoke("Remove", 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Heading * speed;
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Absorb"))
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.name.Contains("Explode"))
        {
            GameObject explosion = (GameObject)Instantiate(Resources.Load("ExplosionPrefab"));
            explosion.transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
        else if(collision.gameObject.name.Contains("Reflect"))
        {
            Heading = Vector3.Reflect(Heading, collision.contacts[0].normal);

            float angle = Mathf.Atan(Heading.y / Heading.x);
            angle *= (180.0f / Mathf.PI);
            angle -= 90.0f;

            if (Heading.x < 0)
            {
                angle += 180.0f;
            }

            Quaternion laserRotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
            transform.rotation = laserRotation;
        }
        else if (collision.gameObject.name.Equals("Ball"))
        {
			//the vector between the position of the laser and the ball
			Vector2 toBall = new Vector2(collision.gameObject.transform.position.x-gameObject.transform.position.x,collision.gameObject.transform.position.y-gameObject.transform.position.y);
			//the angle between the laser and ball(<PI/2) divided by PI/2 to make it between 0 and 1, subtract from 1 so that a smaller angle means more power 
			//e.g. if the laser hits head on, colModifier will be 1, if it hits tangent to the circle edge, it will essentially be zero
			float colModifier = 1- Mathf.Acos((Heading.x*toBall.x + Heading.y + toBall.y)/toBall.magnitude*Heading.magnitude)*2/Mathf.PI;
			//we want to apply more force the more directly the laser hits, and also make it go slightly in the toBall vector direction
			Vector2 forceApplied = new Vector2((Heading.x + toBall.x) * colModifier,(Heading.y + toBall.y) * colModifier);


            GameObject.Find("Ball").rigidbody2D.AddForce(Heading * hitPlayerMagnitude);
            Destroy(gameObject);
        }
    }
}
