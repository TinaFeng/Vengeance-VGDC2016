using UnityEngine;
using System.Collections;

public class Basic_enemy_ai : MonoBehaviour {

    private Rigidbody2D RB;

    public float[] xbounds; //int 0= left boundm int 1= right bound, 
    public float[] ybounds; //int 0 = upper bound, int 1 = lower bound

    public int LIFE=50;
    public int speed;

    public enum Directions {horizontal, vertical};
    public Directions direction;

	// Use this for initialization
	void Start () {
	}


    void FixedUpdate() {

	}
}
