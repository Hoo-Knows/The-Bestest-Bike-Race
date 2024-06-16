using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ability : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text keybind;

    private float duration;
	public float cooldown;
	private int abilityID;
    private int index;
    private Slider slider;

    public bool canUse = false;

    public void SetupValues(AbilityData data, int index)
    {
        duration = data.Duration;
        cooldown = data.Cooldown;
        abilityID = data.AbilityID;
        this.index = index;
        keybind.text = "" + index;
        transform.Find("Slider").Find("Background").GetComponent<Image>().sprite = data.Sprite;
    }

    IEnumerator Start()
    {
        slider = GetComponentInChildren<Slider>();
        slider.value = 1f;
        yield return new WaitUntil(() => UIManager.Instance.racing);
        slider.value = 0f;
        canUse = true;
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
        slider.value = 1f;
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
        slider.value = 0f;
        canUse = true;
    }

	public void Update()
	{
		if(canUse && Input.GetKeyDown("" + index))
        {
			canUse = false;
			Player player = GameObject.Find("Player").GetComponent<Player>();
			player.ActivateAbility(abilityID);
			StartCoroutine(Duration());
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(canUse)
		{
			canUse = false;
			Player player = GameObject.Find("Player").GetComponent<Player>();
			player.ActivateAbility(abilityID);
			StartCoroutine(Duration());
		}
	}
}
