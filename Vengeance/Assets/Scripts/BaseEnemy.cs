using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D), typeof(SpriteRenderer))]
public abstract class BaseEnemy : MonoBehaviour {
	protected abstract void move();
}