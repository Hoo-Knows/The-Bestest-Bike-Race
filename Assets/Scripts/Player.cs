using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Bike
{
	[SerializeField] private GameObject glider;
	[SerializeField] private GameObject rocket;
	[SerializeField] private GameObject domain;
	[SerializeField] private GameObject blueShellPrefab;

	public AudioClip gliderSound;
	public AudioClip rocketSound;
	public AudioClip warpSound;
	public AudioClip domainExpansionSound;

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

		jumpPressed = Input.GetKey(KeyCode.Space);

		base.Update();
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Finish"))
		{
			moveDir = 0;
			racing = false;
			rb.velocity = Vector3.zero;
			//rb.isKinematic = true;
			rb.freezeRotation = true;
			RaceSceneControl.Instance.AddFinishedRacer(this, true);
		}
	}

	public void ActivateAbility(int abilityID)
	{
		if(!canUseAbilities) return;
		switch(abilityID)
		{
			case 0:
				Debug.Log("Using glider");
				GliderAbility();
				break;
			case 1:
				Debug.Log("Using drag chute");
				DragChuteAbility();
				break;
			case 2:
				Debug.Log("Using rocket booster");
				StartCoroutine(RocketBoosterAbility());
				break;
			case 3:
				Debug.Log("Using shell cannon");
				ShellCannonAbility();
				break;
			case 4:
				Debug.Log("Using warp engine");
				WarpEngineAbility();
				break;
			case 5:
				Debug.Log("Using domain expansion");
				DomainExpansionAbility();
				break;
			default:
				break;
		}
	}

	private Coroutine gliderCoro;
	private void GliderAbility()
	{
		if(gliderCoro != null) StopCoroutine(gliderCoro);
		gliderCoro = StartCoroutine(GliderAbilityCoro());
	}

	private IEnumerator GliderAbilityCoro()
	{
		audioSource.PlayOneShot(gliderSound);
		glider.SetActive(true);
		rb.gravityScale = 0f;
		gliding = true;
		yield return new WaitForSeconds(2.5f);
		rb.gravityScale = GravityScale;
		gliding = false;
		glider.SetActive(false);
	}

	private void DragChuteAbility()
	{
		Bike[] bikes = GameObject.FindObjectsOfType<Bike>();
		Bike nearestBike = null;
		foreach(Bike bike in bikes)
		{
			if(bike.gameObject != gameObject)
			{
				if(nearestBike == null) nearestBike = bike;
				else if(Vector3.Distance(nearestBike.transform.position, transform.position)
					> Vector3.Distance(bike.transform.position, transform.position))
				{
					nearestBike = bike;
				}
			}
		}
		StartCoroutine(nearestBike.DragChute());
	}

	private int rocketCounter = 0;
	private IEnumerator RocketBoosterAbility()
	{
		audioSource.PlayOneShot(rocketSound);
		rocketCounter++;
		rocket.SetActive(true);
		maxSpeedMultiplier *= 2f;
		accelMultiplier *= 2f;
		decelMultiplier *= 0.3f;
		yield return new WaitForSeconds(3f);
		maxSpeedMultiplier /= 2f;
		accelMultiplier /= 2f;
		decelMultiplier /= 0.3f;
		rocketCounter--;
		if(rocketCounter == 0) rocket.SetActive(false);
	}

	private void ShellCannonAbility()
	{
		GameObject blueShell = Instantiate(blueShellPrefab, transform.position, Quaternion.identity);
		blueShell.GetComponent<BlueShell>().target = GetFirstPlaceBike();
	}

	private void WarpEngineAbility()
	{
		Bike firstBike = GetFirstPlaceBike();
        if(firstBike.transform.position.x > transform.position.x)
        {
			StartCoroutine(WarpEngineCoro(firstBike));
        }
    }

	private IEnumerator WarpEngineCoro(Bike firstBike)
	{
		audioSource.PlayOneShot(warpSound);
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		float timer = 0f;
		while(timer < 1f)
		{
			sr.color = new Color(1f - timer / 2f, sr.color.g, sr.color.b, 1f - timer / 2f);
			timer += Time.deltaTime;
			yield return null;
		}
		transform.position = firstBike.transform.position;
		sr.color = Color.white;
	}

	private void DomainExpansionAbility()
	{
		AbilityPanel.Instance.RemoveCooldowns();
		domain.SetActive(true);
		domain.transform.Find("Barrier").gameObject.SetActive(false);
		StartCoroutine(DomainExpansionCoro());
	}

	private IEnumerator DomainExpansionCoro()
	{
		audioSource.PlayOneShot(domainExpansionSound);
		float timer = 0f;
		while(timer <= 0.5f)
		{
			float scale = 14f * (-Mathf.Pow(2, -10 * timer) + 1) + 1f;
			domain.transform.localScale = new Vector3(scale, scale, 0f);
			timer += Time.deltaTime;
			yield return null;
		}
		domain.transform.Find("Barrier").gameObject.SetActive(true);
		yield return new WaitWhile(() => racing);
		domain.SetActive(false);
	}

	private Bike GetFirstPlaceBike()
	{
		Bike[] bikes = GameObject.FindObjectsOfType<Bike>();
		Bike firstBike = null;
		foreach(Bike bike in bikes)
		{
			if(bike.gameObject == gameObject) continue;

			if(firstBike == null) firstBike = bike;
			else if(firstBike.transform.position.x < bike.transform.position.x)
			{
				firstBike = bike;
			}
		}
		return firstBike;
	}
}
