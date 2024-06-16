using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

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
	private bool stopTimer;
	private int placement;
	private List<Bike> finishedRacers;

	public enum DialogueID
	{
		None,
		Start,
		Loss,
		ShadyGuyLore,
		ShadyGuyLore2,
		AbilityTutorial,
		Fourth,
		Third,
		Second,
		First,
		PrizeInfo,
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		dialoguePanel.SetActive(false);
		countdownPanel.SetActive(false);
		clockPanel.SetActive(false);
		StartCoroutine(RaceRoutine());
	}

	private IEnumerator RaceRoutine()
	{
		// Init values
		racing = false;
		finishedRacers = new List<Bike>();

		// Play starting dialogue if necessary
		if(!GameManager.Instance.seenStartingDialogue)
		{
			yield return DialogueRoutine(DialogueID.Start);
			GameManager.Instance.seenStartingDialogue = true;
		}
		if(!GameManager.Instance.seenAbilityTutorialDialogue && AbilityPanel.Instance.numAbilities > 0)
		{
			yield return DialogueRoutine(DialogueID.AbilityTutorial);
			GameManager.Instance.seenAbilityTutorialDialogue = true;
		}

		// Begin race with countdown
		yield return CountdownRoutine();

		// Wait for all racers to finish
		yield return new WaitUntil(() => finishedRacers.Count == 4);
		racing = false;

		// Play any post race dialogue
		if(!GameManager.Instance.seenLossDialogue)
		{
			yield return DialogueRoutine(DialogueID.Loss);
			GameManager.Instance.seenLossDialogue = true;
		}
		else
		{
			switch(placement)
			{
				case 1:
					yield return DialogueRoutine(DialogueID.First);
					if(!GameManager.Instance.seenShadyGuyLore2Dialogue)
					{
						yield return DialogueRoutine(DialogueID.ShadyGuyLore2);
						GameManager.Instance.seenShadyGuyLore2Dialogue = true;
					}
					break;
				case 2:
					yield return DialogueRoutine(DialogueID.Second);
					if(!GameManager.Instance.seenShadyGuyLoreDialogue && !GameManager.Instance.seenShadyGuyLore2Dialogue && AbilityPanel.Instance.numAbilities > 0)
					{
						yield return DialogueRoutine(DialogueID.ShadyGuyLore);
						GameManager.Instance.seenShadyGuyLoreDialogue = true;
					}
					break;
				case 3:
					yield return DialogueRoutine(DialogueID.Third);
					if(!GameManager.Instance.seenShadyGuyLoreDialogue && !GameManager.Instance.seenShadyGuyLore2Dialogue && AbilityPanel.Instance.numAbilities > 0)
					{
						yield return DialogueRoutine(DialogueID.ShadyGuyLore);
						GameManager.Instance.seenShadyGuyLoreDialogue = true;
					}
					break;
				case 4:
					yield return DialogueRoutine(DialogueID.Fourth);
					break;
			}
		}
		yield return DialogueRoutine(DialogueID.PrizeInfo);

		// Scene change to shop
		GameManager.Instance.ChangeScene("iBuy");
	}

	// Called when a racer reaches the finish line
	public void AddFinishedRacer(Bike bike, bool isPlayer)
	{
		finishedRacers.Add(bike);
		if(isPlayer)
		{
			stopTimer = true;
			placement = finishedRacers.Count;
			GameManager.Instance.AddBalanceBasedOnPlacement(placement);
		}
	}

	// Stupid ass dialogue switch
	public string[] GetDialogue(DialogueID id)
	{
		switch(id)
		{
			case DialogueID.Start:
				return "Jacob: Good luck, big bro!<br>You: No need to thank me, I'm doing this to avenge you.<br>Jacob: Richard's gonna regret making fun of my bike!<br>You: It's ok, he's just trying to flex that his rich parents bought him a new motorbike. Can't believe the judges even allow that thing in what's supposed to be a 'casual race.'<br>You: Then again, they're letting me, a high schooler, participate in a middle school race...<br>Jacob: What's wrong with my Barbie tricycle anyway?<br>You: Nothing, he's just a hater.<br>You: Anyway, it still shocks me that this has such a big prize pool. Guess that's where the district's money's been going, instead of towards teachers' paychecks. Hey, Maybe we can use the prize money to get you a better bike?<br>Jacob: NO! ONLY THIS ONE!<br>You: Uhhh...OK. Well, I'm just saying, I'll do my best to beat Richard for ya, but it might be a long shot with what we've got here.<br>Jacob: I believe in you!<br>Richard: Hahaha, Jacob couldn't win last time so he brought his brother to try and beat me. What an idiot!<br>Richard: And even he's stuck using that sad scooter that you're still using. Grow up already!<br>Jacob: Hmph! I know he'll win for sure.<br>Richard: We'll see about that!<br>Use A and D or left and right arrow keys to move. Space to do a short hop.".Split("<br>");
			case DialogueID.Loss:
				return "Richard: Heh, looks like not even your brother can save that sad Barbie tricycle. Admit it!<br>Jacob: Grr... we’ll get it one day! In fact, I know of just how we’ll do it. You’ll be beaten before you know it!<br>Richard: Go ahead and try! I’ll see you at the next race where your brother will be in last place again.<br>Jacob: Fine! We’ll meet again!<br>You: Hey, what were you talking about when you said you’ll know just how to beat Richard?<br>Jacob: Well, during the race, some guy came out of a white van and told me about his online bike store. He said if I bought some things from there I could improve my Barbiecycle!<br>You: That sounds incredibly shady.<br>Jacob: What about it?<br>You: ...Never mind. Anyway, I thought you didn’t want to buy anything else?<br>Jacob: No new bikes. But I want to make my Barbiecycle better!<br>You: ...I feel like it’d be better to get a new bike...<br>Jacob: Well, wait ‘til you see these Barbiecycle upgrades! The guy said he’s taking a big risk by selling them to me, so they gotta be good!<br>You: ...<br>You: ...Sure then, I’ll check it out. Looks like I got some consolation prizing, so might as well make the most of it.".Split("<br>");
			case DialogueID.AbilityTutorial:
				return new string[] { "Click on the icons in the top left to activate abilities, or press the corresponding number key" };
			case DialogueID.Fourth:
				return "You: Dang it, last place again.<br>Jacob: It’s ok, we just need to get a few more of those upgrades. You’ll be first in no time!<br>You: Good thing there’s very convenient consolation prizing.".Split("<br>");
			case DialogueID.Third:
				return "You: 3rd place this time. Still got a bit to go, but we’re making progress.<br>Jacob: That’s my brother!".Split("<br>");
			case DialogueID.Second:
				return "You: 2nd place! We’re almost there!<br>Jacob: Yeah! Richard, better watch out!".Split("<br>");
			case DialogueID.First:
				return "You: I did it!<br>Jacob: Haha! Take that, Richard!".Split("<br>");
			case DialogueID.ShadyGuyLore:
				return "You: Hmm...more money to work with. This site sure ships its stuff fast.<br>You: But with items like ‘Teleportation Engine’ and ‘Domain Expansion’ I’m surprised the government hasn’t shut it down yet.<br>You: How did some random guy in a white van just get his hands on them?<br>Jacob: Don’t know, don’t care. Richard’s got it coming for him, insulting my tricycle like that.".Split("<br>");
			case DialogueID.ShadyGuyLore2:
				return "Richard: What the hell was that??? How did the judges allow this stuff in a bike race?<br>Jacob: How did they allow a motorbike in the race?<br>Richard: What you’re doing is completely different! Where did you even get these upgrades in the first place?<br>Shady Guy: That would be from me.<br>You: Woah, thanks for setting up iBuy! Wouldn’t’ve won the race without it. And yeah, how’d you get these items?<br>You: I’m not even really cycling at this point, it’s just your stuff doing all the work for me.<br>Shady Guy: I’m not the one who has the items. I don’t even exist, I’m just a random character the developer added to try and explain the store. The items exist only because the developer thought letting players throw shells at Richard would be funny. Same reason he’s writing this dialogue right now.<br>You: What?<br>Shady Guy: What?<br>You: ...<br>Jacob: ...<br>Richard: ...<br>Shady Guy: ...<br>Shady Guy: Well, back to my white van I go. I need to go find more aspiring bikers to sell illegal upgrades to.<br>You: Sure?<br>You: Well, that was weird. I guess so are these upgrades, so everything checks out.<br>Richard: This is unfair!<br>Jacob: Deal with it.".Split("<br>");
			case DialogueID.PrizeInfo:
				string[] suffix = new string[] { "1st", "2nd", "3rd", "4th" };
				return new string[] { string.Format("You won ${0} for placing {1}", GameManager.Instance.GetPrizeForPlacement(placement), suffix[placement - 1]) };
			default:
				break;
		}
		return new string[] { };
	}

	// Coroutine for displaying lines of dialogue
	private IEnumerator DialogueRoutine(DialogueID id)
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

	// Speedrun timer
	private IEnumerator RaceClockRoutine()
	{
		clockPanel.SetActive(true);
		Stopwatch watch = new Stopwatch();
		watch.Start();
		TimeSpan ts;
		while(!stopTimer)
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
