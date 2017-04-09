using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private Rigidbody2D RB2D;
    public Vector2 location;
    public float speed;

    private bool Bounceflag = false;


    void Start()
    {

        RB2D = gameObject.GetComponent<Rigidbody2D>();
        transform.position = new Vector3(location.x, location.y, 0);
    }

    //moves to a point in space
    public void MoveToPoint()
    {
        RB2D.transform.position = Vector2.LerpUnclamped(RB2D.transform.position, location, speed * Time.deltaTime);
    }


    // takes the walldistatnce and the direction its moving and chooses to use either BounceX, or BounceY
    // Bounce speed is based on speed variable
    public void Bounce(float wallDist, string direction)
    {
        if (RB2D == null)
            RB2D = gameObject.GetComponent<Rigidbody2D>();
        Vector2 movement = new Vector2(0, 0);
        if (direction == "Horizontal")
        {
            movement = new Vector2(RB2D.transform.position.x + speed, RB2D.transform.position.y);
            BounceX(movement, wallDist);

        }
        else if (direction == "Vertical")
        {
            movement = new Vector2(RB2D.transform.position.x, RB2D.transform.position.y - speed);
            BounceY(movement, wallDist);
        }


    }

    void BounceX(Vector2 movement, float wallDist)
    {
        if (Bounceflag == false)
        {
            RB2D.MovePosition(new Vector2(movement.x, movement.y));
            if (RB2D.transform.position.x > wallDist)
                Bounceflag = true;

        }
        else
        {
            RB2D.MovePosition(new Vector2(movement.x-(speed*2), movement.y));
            if (RB2D.transform.position.x < -wallDist)
                Bounceflag = false;
        }
    }

    void BounceY(Vector2 movement, float wallDist)
    {
        if (Bounceflag == false)
        {
            RB2D.MovePosition(new Vector2(movement.x, movement.y));
            if (RB2D.transform.position.y < -wallDist)
                Bounceflag = true;

        }
        else
        {
            
            RB2D.MovePosition(new Vector2(movement.x, movement.y+(speed*2)));
            if (RB2D.transform.position.y > wallDist)
                Bounceflag = false;
        }
    }
}
