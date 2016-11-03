using UnityEngine;
using System.Collections;

public class EnemyPlaceholder2 : BaseEnemy {

	private Rigidbody2D rb;
	private bool isVisible;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isVisible) {
			move();
		}
	}

	protected override void move() {
		rb.MovePosition(Vector2.left);
	}

	void OnBecameVisible() {
		isVisible = true;
	}

	void OnBecameInvisible() {
		isVisible = false;	
	}
}
