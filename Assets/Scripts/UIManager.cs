using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	
	[SerializeField] private GameObject dialoguePanel;
	[SerializeField] private TMP_Text dialogueText;
	[SerializeField] private GameObject countdownPanel;
	[SerializeField] private TMP_Text countdownText;
	[SerializeField] private GameObject clockPanel;
	[SerializeField] private TMP_Text clockText;

	public bool racing;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		dialoguePanel.SetActive(false);
		countdownPanel.SetActive(false);
		clockPanel.SetActive(false);
		StartCoroutine(PreRaceRoutine());
	}

	private IEnumerator PreRaceRoutine()
	{
		racing = false;
		int dialogueID = 0;
		yield return DialogueRoutine(dialogueID);
		yield return CountdownRoutine();
	}

	public void EndRace() => StartCoroutine(EndRaceRoutine());

	private IEnumerator EndRaceRoutine()
	{
		racing = false;
		int dialogueID = 0;
		yield return DialogueRoutine(dialogueID+100);
		// Scene change to upgrade
		GameManager.Instance.ChangeScene("iBuy");
	}

	public string[] GetDialogue(int id)
	{
		switch(id)
		{
			case 0:
				return new string[]
				{
					"Jacob: Good luck, big bro!",
					"You: No need to thank me, I'm doing this to avenge you.",
					"Jacob: Richard's gonna regret making fun of my bike!",
					"You: It's ok, he's just trying to flex that his rich parents bought him a new motorbike.",
					"You: Can't believe the judges even allow that thing in what's supposed to be a 'casual race.'",
					"You: Although then again, they're letting me, a high schooler, participate in a middle school race...",
					"Jacob: What's wrong with my Barbie bike anyway?",
					"You: Nothing, he's just a hater. Anyway, it still shocks me that this has such a big prize pool.",
					"You: Guess that's where the district's money's been going, instead of teachers' paychecks.",
					"You: Hey, Maybe we can use the prize money to get you a new bike?",
					"Jacob: NO! ONLY THIS ONE!",
					"You: Uhhh...OK. Well, I'm just saying, I'll do my best to beat Richard for ya, but it might be a long shot with what we've got here.",
					"Jacob: I believe in you!",
					"Richard: Hahaha, Jacob couldn't win last time so he brought his brother to try and beat me. What an idiot!",
					"Richard: And even he's stuck using that sad scooter that you're still using even in middle school.",
					"Jacob: Hmph! I know he'll win for sure.",
					"Richard: We'll see about that!"
				};
			default:
				break;
		}
		return new string[] { };
	}

	private IEnumerator DialogueRoutine(int id)
	{
		string[] dialogue = GetDialogue(id);
		if(dialogue.Length == 0) yield break;

		dialoguePanel.SetActive(true);
		foreach(string message in dialogue)
		{
			dialogueText.text = message;
			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0));
			yield return null;
		}
		dialoguePanel.SetActive(false);
	}

	private IEnumerator CountdownRoutine()
	{
		countdownText.text = "3";
		countdownPanel.SetActive(true);
		yield return new WaitForSeconds(1f);
		countdownText.text = "2";
		yield return new WaitForSeconds(1f);
		countdownText.text = "1";
		yield return new WaitForSeconds(1f);
		countdownText.text = "START";
		racing = true;
		StartCoroutine(RaceClockRoutine());
		yield return new WaitForSeconds(1f);
		countdownPanel.SetActive(false);
	}

	private IEnumerator RaceClockRoutine()
	{
		clockPanel.SetActive(true);
		Stopwatch watch = new Stopwatch();
		watch.Start();
		TimeSpan ts;

		while(racing)
		{
			ts = watch.Elapsed;
			clockText.text = string.Format("{0:00}:{1:00}.{2:000}",
				ts.Minutes, ts.Seconds,
				ts.Milliseconds / 10);
			yield return null;
		}
		watch.Stop();
		ts = watch.Elapsed;
		clockText.text = string.Format("{0:00}:{1:00}.{2:000}",
			ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
	}
}
