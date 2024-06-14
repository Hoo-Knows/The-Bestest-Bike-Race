using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [SerializeField] private float duration;
	[SerializeField] private float cooldown;
	[SerializeField] private int abilityID;
    private Slider slider;

    public bool canUse = false;

    IEnumerator Start()
    {
        slider = GetComponentInChildren<Slider>();
        slider.value = 1f;
        yield return new WaitUntil(() => UIManager.Instance.racing);
        StartCoroutine(Cooldown());
    }

    private IEnumerator Duration()
    {
		float timer = 0f;
		while(timer < duration)
		{
			slider.value = Mathf.Clamp(timer / duration, 0f, 1f);
			timer += Time.deltaTime;
			yield return null;
		}
        StartCoroutine(Cooldown());
	}

    private IEnumerator Cooldown()
    {
        float timer = 0f;
        while(timer < cooldown)
        {
            slider.value = Mathf.Clamp(1f - timer / cooldown, 0f, 1f);
            timer += Time.deltaTime;
            yield return null;
        }
        canUse = true;
    }

	private void Update()
	{
		if(canUse && Input.GetKeyDown("" + abilityID))
        {
			canUse = false;
			Player player = GameObject.Find("Player").GetComponent<Player>();
			player.ActivateAbility(abilityID);
			StartCoroutine(Duration());
		}
	}
}
