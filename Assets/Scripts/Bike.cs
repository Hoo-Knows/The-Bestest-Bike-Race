using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bike : MonoBehaviour
{
	[SerializeField] protected BikeData bikeData;
	protected readonly float RotationSpeed = 45f;
	protected readonly float GravityScale = 4f;

	[SerializeField] private LayerMask terrain;

	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public BoxCollider2D col;

	protected float maxSpeedMultiplier = 1f;
	protected float accelMultiplier = 1f;
	protected float decelMultiplier = 1f;

	protected int moveDir;
	protected bool gliding;
	protected bool racing;
	protected bool jumpPressed;
	protected bool jumpCooldownDone;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<BoxCollider2D>();
		rb.gravityScale = GravityScale;
		rb.isKinematic = true;
		rb.freezeRotation = true;
		jumpCooldownDone = true;

		// Wait to set racing
		StartCoroutine(StartRace());
	}

	private IEnumerator StartRace()
	{
		yield return new WaitUntil(() => UIManager.Instance.racing);
		rb.isKinematic = false;
		rb.freezeRotation = false;
		racing = true;
	}

	protected virtual void Update()
	{
		if(moveDir > 0) transform.localScale = new Vector2(1f, 1f);
		if(moveDir < 0) transform.localScale = new Vector2(-1f, 1f);
	}

	protected virtual void FixedUpdate()
	{
		if(racing)
		{
			rb.velocity = new Vector2(MoveX(moveDir), MoveY());

			if(!Grounded())
			{
				if(rb.rotation < 15f) rb.MoveRotation(rb.rotation += RotationSpeed * Time.deltaTime);
				else if(rb.rotation > 15f) rb.MoveRotation(rb.rotation -= RotationSpeed * Time.deltaTime);
			}
		}
	}

	private float MoveX(int dir)
	{
		float xVel = rb.velocity.x;
		if(dir != 0)
		{
			if(Mathf.Abs(xVel) < maxSpeedMultiplier * bikeData.MaxSpeed) xVel += dir * accelMultiplier * bikeData.Accel * Time.fixedDeltaTime;
		}
		else
		{
			if(Grounded()) xVel -= Mathf.Sign(xVel) * decelMultiplier * bikeData.Decel * Time.fixedDeltaTime;
		}
		return xVel;
	}

	private float MoveY()
	{
		float yVel = rb.velocity.y;
		if(jumpPressed && CanJump())
		{
			yVel += bikeData.JumpSpeed;
			StartCoroutine(JumpCooldown());
		}
		if(gliding) yVel = bikeData.GlideYSpeed;
		return yVel;
	}

	private bool Grounded()
	{
		Vector3 center = col.bounds.center;
		Vector3 size = col.bounds.size;
		size.y = 0.05f;

		return Physics2D.BoxCast(center, size, rb.rotation, Vector2.down, col.bounds.extents.y, terrain);
	}

	private IEnumerator JumpCooldown()
	{
		jumpCooldownDone = false;
		yield return new WaitForSeconds(1.25f);
		jumpCooldownDone = true;
	}
	
	private bool CanJump()
	{
		Vector3 center = col.bounds.center;
		Vector3 extents = col.bounds.extents;

		return jumpCooldownDone && Physics2D.Raycast(center, 
			new Vector2(Mathf.Sin(Mathf.Deg2Rad * rb.rotation), -Mathf.Cos(Mathf.Deg2Rad * rb.rotation)), extents.y + 0.1f, terrain);
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
			rb.freezeRotation = true;
		}
	}
}
