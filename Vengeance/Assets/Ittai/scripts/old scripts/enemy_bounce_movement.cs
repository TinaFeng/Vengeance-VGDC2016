using UnityEngine;
using System.Collections;

public class enemy_bounce_movement : MonoBehaviour
{

    private Rigidbody2D RB;

    public float[] xbounds; //int 0= left boundm int 1= right bound, 
    public float[] ybounds; //int 0 = upper bound, int 1 = lower bound

    public int speed;

    public enum Directions { horizontal, vertical };
    public Directions direction;

    // Use this for initialization
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        Vector2 Move_direction;


        if (direction == Directions.horizontal)
            Move_direction = new Vector2(speed, 0);
        else
            Move_direction = new Vector2(0, -speed); // negative bc coordinate system I guess


        if (RB.transform.position.x <= xbounds[0] || RB.transform.position.x >= xbounds[1])
        {
            RB.transform.position = new Vector2(Mathf.Clamp(RB.transform.position.x,
                    (xbounds[0] + .1f), (xbounds[1] - .1f)), RB.transform.position.y);// clamps the transform to the boundaries, makes sure that it won't move beyond those and then
                                                                                      //sets it back into place
            speed *= -1; //reverse
        }

        if (RB.transform.position.y >= ybounds[0] || RB.transform.position.y <= ybounds[1])
        {
            RB.transform.position = new Vector2(RB.transform.position.x, Mathf.Clamp(RB.transform.position.y,
                    (ybounds[1] + .1f), (ybounds[0] - .1f)));// clamps the transform to the boundaries, makes sure that it won't move beyond those and then
                                                             //sets it back into place
            speed *= -1;
        }

        RB.MovePosition(RB.position + Move_direction * Time.deltaTime);
    }
}

