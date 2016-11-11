using UnityEngine;
using System.Collections;

public class Enemy_movement_2 : MonoBehaviour {
    private Rigidbody2D RB;

    public float x_coor;
    public float y_coor;
    public float speed;
	// Use this for initialization
	void Start () {

        RB = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {
        Vector2 origin = new Vector2(RB.transform.position.x,RB.transform.position.y);
        if (RB.transform.position!= new Vector3(x_coor, y_coor,0) )
        {
            RB.MovePosition(Vector2.MoveTowards(origin, new Vector2(x_coor, y_coor), Time.deltaTime*speed));

        }
	}
}
