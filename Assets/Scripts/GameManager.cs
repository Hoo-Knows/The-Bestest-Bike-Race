using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

	public bool[] unlockedAbilities = new bool[6];
	public int balance;

	public bool seenStartingDialogue;
	public bool seenAbilityTutorialDialogue;
	public bool seenLossDialogue;
	public bool seenShadyGuyLoreDialogue;
	public bool seenShadyGuyLore2Dialogue;

	private void Awake()
	{
		if(Instance != null)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public int GetPrizeForPlacement(int placement)
	{
		switch(placement)
		{
			case 1:
				return 1000;
			case 2:
				return 300;
			case 3:
				return 150;
			case 4:
				return 50;
		}
		return 0;
	}

	public void AddBalanceBasedOnPlacement(int placement)
	{
		balance += GetPrizeForPlacement(placement);
	}

	public void ChangeScene(string sceneName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}
}
