using UnityEngine;
using System.Collections;

public class SimpleMovement_ittai : MonoBehaviour
{
    public float speed = 6.0f;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D Playerbod;
    private Transform trans;
    // Use this for initialization
    void Start()
    {
       Playerbod = GetComponent < Rigidbody2D >();
       trans = GetComponent < Transform >();

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal")*speed, Input.GetAxis("Vertical")*speed);
        Playerbod.MovePosition(Playerbod.position + moveDirection * Time.deltaTime);
                 
    }
}
