using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Bike
{
	[SerializeField] private string[] controls;
	[SerializeField] private KeyCode[] keys;
	private Dictionary<string, KeyCode> keybinds;

	protected override void Start()
    {
		keybinds = new Dictionary<string, KeyCode>();
		for(int i = 0; i < controls.Length; i++)
		{
			keybinds.Add(controls[i], keys[i]);
		}

		base.Start();
    }

	protected override void Update()
	{
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			moveDir = -1;
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			moveDir = 1;
		}
		else moveDir = 0;

		base.Update();
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Finish"))
		{
			UIManager.Instance.EndRace();
		}
		base.OnTriggerEnter2D(collision);
	}
}
