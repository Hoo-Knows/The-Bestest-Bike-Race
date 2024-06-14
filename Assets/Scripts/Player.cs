using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Bike
{
	protected override void Update()
	{
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			moveDir = -1;
		}
		else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			moveDir = 1;
		}
		else moveDir = 0;

		jumpPressed = Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Space);

		base.Update();
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Finish"))
		{
			UIManager.Instance.EndRace();
		}
		base.OnTriggerEnter2D(collision);
	}

	public void ActivateAbility(int abilityID)
	{
		switch(abilityID)
		{
			case 1:
				StartCoroutine(RocketBooster());
				break;
			case 2:
				StartCoroutine(Glider());
				break;
			default:
				break;
		}
	}

	private IEnumerator RocketBooster()
	{
		maxSpeedMultiplier *= 4f;
		accelMultiplier *= 3f;
		decelMultiplier *= 0.3f;
		yield return new WaitForSeconds(5f);
		maxSpeedMultiplier /= 4f;
		accelMultiplier /= 3f;
		decelMultiplier /= 0.3f;
	}

	private IEnumerator Glider()
	{
		rb.gravityScale = 0f;
		gliding = true;
		yield return new WaitForSeconds(10f);
		rb.gravityScale = GravityScale;
		gliding = false;
	}
}
