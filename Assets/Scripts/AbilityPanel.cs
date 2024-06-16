using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPanel : MonoBehaviour
{
    public static AbilityPanel Instance;

    [SerializeField] private GameObject abilityPrefab;
	[SerializeField] private List<AbilityData> abilityData;
    private List<Ability> abilities;
    public int numAbilities;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
        abilities = new List<Ability>();
        numAbilities = 0;
        for(int i = 0; i < GameManager.Instance.unlockedAbilities.Length; i++)
        {
            if(GameManager.Instance.unlockedAbilities[i])
            {
                GameObject abilityGO = Instantiate(abilityPrefab, transform);
                abilityGO.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(105f * numAbilities, 0f);

                Ability ability = abilityGO.GetComponent<Ability>();
                ability.SetupValues(abilityData[i], numAbilities + 1);
                numAbilities++;
                abilities.Add(ability);
            }
        }
    }

    public void RemoveCooldowns()
    {
        foreach(Ability ability in abilities)
        {
            ability.cooldown = 0f;
        }
    }
}
