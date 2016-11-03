using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (SpriteRenderer))]
public abstract class BaseEnemy : MonoBehaviour {
	protected abstract void move();
}