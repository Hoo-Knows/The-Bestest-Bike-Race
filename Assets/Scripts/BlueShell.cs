using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShell : MonoBehaviour
{
    public Bike target;
    private float speed = 15f;

	private AudioSource audioSource;
	public AudioClip chaseSound;
	public AudioClip explodeSound;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        transform.SetParent(target.transform);
		audioSource = GetComponent<AudioSource>();
		yield return Chase();
		yield return Float();
		yield return Explode();
    }

	private IEnumerator Chase()
	{
		audioSource.PlayOneShot(chaseSound);
		while(Vector2.Distance(transform.localPosition, Vector3.up) > 0.05f)
		{
			transform.localPosition = Vector2.MoveTowards(transform.localPosition, Vector3.up, speed * Time.fixedDeltaTime);
			yield return null;
		}
		Debug.Log("Blue shell chase end");
	}

	private IEnumerator Float()
	{
		float timer = 0f;
		while(timer < 1f)
		{
			float xOffset = Mathf.Sin(2f * Mathf.PI * timer);
			float yOffset = 1f + timer / 2f;
			transform.localPosition = new Vector3(xOffset, yOffset);
			timer += Time.deltaTime;
			yield return null;
		}
		Debug.Log("Blue shell float end");
	}

	private IEnumerator Explode()
	{
		audioSource.PlayOneShot(explodeSound);
		while(Vector2.Distance(transform.localPosition, Vector2.zero) > 0.05f)
		{
			transform.localPosition = Vector2.MoveTowards(transform.localPosition, Vector2.zero, speed * Time.fixedDeltaTime);
			yield return null;
		}
		Bike[] bikes = FindObjectsOfType<Bike>();
		foreach(Bike bike in bikes)
		{
			if(Vector2.Distance(bike.transform.position, transform.position) < 1f)
			{
				bike.BlueShell();
			}
		}
		GetComponent<SpriteRenderer>().enabled = false;
		GameObject aura = transform.Find("Circle").gameObject;
		aura.SetActive(true);
		SpriteRenderer aurasr = aura.GetComponent<SpriteRenderer>();
		float timer = 0f;
		while(timer < 0.5f)
		{
			aurasr.color = new Color(aurasr.color.r, aurasr.color.g, aurasr.color.b, 1.5f * (0.5f - timer));
			timer += Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);
	}
}
