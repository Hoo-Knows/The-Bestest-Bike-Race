using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRacer : Bike
{
	protected override void Start()
	{
		StartCoroutine(WaitForRaceStart());
		base.Start();
	}

	private IEnumerator WaitForRaceStart()
	{
		yield return new WaitUntil(() => RaceSceneControl.Instance.racing);
		moveDir = 1;
		yield break;
	}
}
