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
		Debug.Log ("wat");
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
            GameObject.Find("Ball").rigidbody2D.AddRelativeForce(collision.contacts[0].normal * hitPlayerMagnitude);
            Destroy(gameObject);
        }
    }
}
