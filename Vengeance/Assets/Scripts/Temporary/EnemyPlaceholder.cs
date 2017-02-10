using UnityEngine;
using System.Collections;

public class EnemyPlaceholder : BaseEnemy {

	private Rigidbody2D rb;
	private bool isVisible;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();	
	}
	
	void FixedUpdate() {
		if (isVisible) {
			move();
		}
	}

	protected override void move() {
		rb.MovePosition(rb.position + Vector2.down * 2 * Time.fixedDeltaTime);
	}

	void OnBecameVisible() {
		isVisible = true;
	}
	void OnBecameInvisible() {
		isVisible = false;
		Destroy(gameObject);
	}
}
