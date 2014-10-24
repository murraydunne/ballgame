using UnityEngine;
using System.Collections;

public class LaserPhysicsScript : MonoBehaviour
{
    public Vector3 Heading { get; set; }

    public float hitPlayerMagnitude = 400.0f;
    public float speed = 0.25f;
    public float autoCorrectDegrees = 10.0f;

    // Use this for initialization
    void Start()
    {
		Destroy (gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Heading * speed;
    }

  

    void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.name.Contains("Absorb") || collision.gameObject.tag.Contains("Absorb") )
        {
            Destroy(gameObject);
        }
		else if(collision.gameObject.name.Contains("Explode") || collision.gameObject.tag.Contains("Explode"))
        {
            GameObject explosion = (GameObject)Instantiate(Resources.Load("ExplosionPrefab"));
            explosion.transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
		else if(collision.gameObject.name.Contains("Reflect") || collision.gameObject.tag.Contains("Reflect"))
        {
            OnHitReflect(collision);
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


            GameObject.Find("Ball").rigidbody2D.AddForce(Heading.normalized * hitPlayerMagnitude);
            Destroy(gameObject);
        }
    }

    float SpeedPerSecond()
    {
        return this.speed * (float)(1.0 / Time.deltaTime);
    }

    float LaserHitParabolaTime(Vector2 ballPos, Vector2 ballVelocity, Vector2 laserStart, float ballTime)
    {
        float hitX = ballPos.x + ballVelocity.x * ballTime;

        // d = vt + 0.5at^2
        float hitY = ballPos.y + ballVelocity.y * ballTime + 0.5f * -9.8f * (ballTime * ballTime);

        float deltaX = hitX - laserStart.x;
        float deltaY = hitY - laserStart.y;

        float distanceToHit = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

        float timeToHit = distanceToHit / SpeedPerSecond();

        return timeToHit;
    }

    float DeltaAngleToHitBall(Vector2 ballPos, Vector2 ballVelocity, Vector2 laserStart, float currentAngle)
    {
        int iterationLimit = 100;
        int iteration = 0;

        // first find an upper bound, a time such that the laser will intersect the parabola at a point
        // before the ball reaches it
        float upperTime = 0.0f;

        for (iteration = 0; iteration < iterationLimit; iteration++)
        {
            float laserTime = LaserHitParabolaTime(ballPos, ballVelocity, laserStart, upperTime);
            
            if (laserTime < upperTime)
            {
                break;
            }

            upperTime += 0.1f;
        }

        if(iteration == iterationLimit)
        {
            Debug.Log("No Solution");
            return 0;
        }

        // zero will be a fine lower bound, as the ball will always reach the point it is already at before
        // the laser gets there
        float lowerTime = 0.0f;

        // now binary search between lower time and upper time until the ball and the laser time are within
        // a small delta
        float acceptableDelta = 0.05f;

        // binary search result
        float foundTime = 0.0f;

        for (iteration = 0; iteration < iterationLimit; iteration++)
        {
            float halfTime = (upperTime + lowerTime) / 2;

            float laserTime = LaserHitParabolaTime(ballPos, ballVelocity, laserStart, halfTime);

            if (Mathf.Abs(halfTime - laserTime) < acceptableDelta)
            {
                foundTime = halfTime;
                break;
            }
            else if (laserTime < halfTime)
            {
                // laser will get to point first, pass ball ahead of it, move upper down
                upperTime = halfTime;
            }
            else
            {
                // ball will get to point first, laser will pass ball behind it, move lower up
                lowerTime = halfTime;
            }
        }

        if (iteration == iterationLimit)
        {
            Debug.Log("No Solution");
            return 0;
        }

        float hitX = ballPos.x + ballVelocity.x * foundTime;
        // d = vt + 0.5at^2
        float hitY = ballPos.y + ballVelocity.y * foundTime + 0.5f * -9.8f * (foundTime * foundTime);

        // the decided path line
        //Debug.DrawLine(laserStart, new Vector2(hitX, hitY), Color.cyan, 2.0f);

        float deltaX = hitX - laserStart.x;
        float deltaY = hitY - laserStart.y;

        float angle = Mathf.Atan(deltaY / deltaX);
        angle *= (180.0f / Mathf.PI);

        if (deltaX < 0)
        {
            angle += 180.0f;
        }

        return angle - currentAngle;
    }

    void OnHitReflect(Collision2D collision)
    {
        Rigidbody2D ball = GameObject.Find("Ball").rigidbody2D;

        // the ball parabola
        //for (float ti = ball.position.x - 5; ti < ball.position.x + 15; ti += 0.1f)
        //{
        //    Vector2 doop = new Vector2(ti,
        //                               ball.velocity.y * (ti - ball.position.x) / ball.velocity.x +
        //                               0.5f * (-9.8f) * ((ti - ball.position.x) / ball.velocity.x) * ((ti - ball.position.x) / ball.velocity.x) + ball.position.y);
        //    ti = ti + 0.1f;
        //    Vector2 dooop = new Vector2(ti,
        //                                ball.velocity.y * (ti - ball.position.x) / ball.velocity.x +
        //                                0.5f * (-9.8f) * ((ti - ball.position.x) / ball.velocity.x) * ((ti - ball.position.x) / ball.velocity.x) + ball.position.y);

        //    Debug.DrawLine(doop, dooop, Color.red, 2);

        //    ti = ti - 0.1f;
        //}

        Heading = Vector3.Reflect(Heading, collision.contacts[0].normal);

        float angle = Mathf.Atan(Heading.y / Heading.x);
        angle *= (180.0f / Mathf.PI);
        angle -= 90.0f;

        if (Heading.x < 0)
        {
            angle += 180.0f;
        }

        // correction
        float deltaAngle = DeltaAngleToHitBall(ball.position, ball.velocity, gameObject.transform.position, angle);

        if(Mathf.Abs(deltaAngle) < autoCorrectDegrees)
        {
            angle += deltaAngle;
            Heading = Quaternion.AngleAxis(deltaAngle - 90.0f, new Vector3(0, 0, 1)) * Heading;
            angle += 90.0f;
        }

        Quaternion laserRotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        transform.rotation = laserRotation;

        // the path taken line
        //Debug.DrawLine(transform.position, transform.position + 40.0f * Heading, Color.red, 2);
    }
}
