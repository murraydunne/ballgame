using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour
{
    private float createTime;

    private const float duration = 0.1f;
    private const float minSize = 0.01f;
    private const float maxSize = 1.0f;

    public float magnitude = 300.0f;

    private Vector3 startScale;
    private Vector3 endScale;

    // Use this for initialization
    void Start()
    {


        createTime = Time.fixedTime;
		
        startScale = new Vector3(minSize, minSize, 0.0f);
        endScale = new Vector3(maxSize, maxSize, 0.0f);

        gameObject.transform.localScale = startScale;

        Invoke("Remove", duration);
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        ((CircleCollider2D)gameObject.collider2D).radius = Mathf.Lerp(minSize, maxSize, (Time.fixedTime - createTime) / duration);
        gameObject.transform.localScale = Vector3.Lerp(startScale, endScale, (Time.fixedTime - createTime) / duration);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
			Vector2 temp = new Vector2(collision.gameObject.transform.position.x-gameObject.transform.position.x,1);
            collision.gameObject.rigidbody2D.AddForce(temp * magnitude);
			gameObject.collider2D.isTrigger = true;
		
        }
    }
	
}
