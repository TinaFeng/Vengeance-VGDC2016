using UnityEngine;
using System.Collections;

public class EnemyPlaceholder : MonoBehaviour {

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private bool isMoving;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();	
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void FixedUpdate() {
		// Enemy moves downwards at a fixed rate
		if (isMoving) {
			rb.MovePosition(rb.position + Vector2.down * 2 * Time.fixedDeltaTime);
		}
	}

	void OnBecameVisible() {
		isMoving = true;
	}
	void OnBecameInvisible() {
		isMoving = false;
		Destroy(gameObject);
	}
}
