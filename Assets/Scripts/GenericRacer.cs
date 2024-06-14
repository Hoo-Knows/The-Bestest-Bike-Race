using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRacer : Bike
{
	protected override void Start()
	{
		moveDir = 1;
		base.Start();
	}
}
