using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bike : MonoBehaviour
{
	[SerializeField] private float maxSpeed = 10f;
	[SerializeField] private float accel = 60f;
	[SerializeField] private float deccel = 40f;
	private float GRAVITY_SCALE = 1f;

	[SerializeField] private LayerMask terrain;

	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public BoxCollider2D col;

	private float speedMultiplier = 1f;

	protected int moveDir;
	protected bool racing;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<BoxCollider2D>();
		rb.gravityScale = GRAVITY_SCALE;

		// Wait to set racing
		StartCoroutine(StartRace());
	}

	private IEnumerator StartRace()
	{
		yield return new WaitUntil(() => UIManager.Instance.racing);
		racing = true;
	}

	protected virtual void Update()
	{
		if(moveDir > 0) transform.localScale = new Vector2(1f, 1f);
		if(moveDir < 0) transform.localScale = new Vector2(-1f, 1f);
	}

	protected virtual void FixedUpdate()
	{
		if(racing) rb.velocity = new Vector2(MoveX(moveDir), MoveY());
	}

	private float MoveX(int dir)
	{
		float xVel = rb.velocity.x;
		if(dir != 0)
		{
			xVel += dir * accel * Time.fixedDeltaTime;
			if(Mathf.Abs(xVel) > speedMultiplier * maxSpeed) xVel = Mathf.Sign(xVel) * speedMultiplier * maxSpeed;
		}
		else
		{
			if(Grounded()) xVel -= Mathf.Sign(xVel) * deccel * Time.fixedDeltaTime;
			if(xVel * rb.velocity.x <= 0f) xVel = 0f;
		}
		return xVel;
	}

	private float MoveY()
	{
		float yVel = rb.velocity.y;
		return yVel;
	}

	private bool Grounded()
	{
		Vector3 center = col.bounds.center;
		Vector3 size = col.bounds.size;
		size.y = 0.05f;

		return Physics2D.BoxCast(center, size, rb.rotation, Vector2.down, col.bounds.extents.y, terrain);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(Grounded())
		{
			rb.velocity = new Vector2(rb.velocity.x, 0f);
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Finish"))
		{
			moveDir = 0;
			racing = false;
			rb.velocity = Vector3.zero;
			rb.isKinematic = true;
		}
	}
}
