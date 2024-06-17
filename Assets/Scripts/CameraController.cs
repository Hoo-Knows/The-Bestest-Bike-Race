using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Bike[] bikes;

	public int following;

	private void Start()
	{
		following = 0;
	}

	public void ToggleFollowing()
	{
		following++;
		if(following >= bikes.Length)
		{
			following = 0;
		}
	}

	// Update is called once per frame
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.Tab) && RaceSceneControl.Instance.canSpectate)
		{
			ToggleFollowing();
			RaceSceneControl.Instance.spectatePanel.GetComponentInChildren<TMP_Text>().text = "Spectating " + bikes[following].gameObject.name;
		}
		if(!RaceSceneControl.Instance.canSpectate)
		{
			following = 0;
		}
        transform.position = new Vector3(bikes[following].transform.position.x, bikes[following].transform.position.y, transform.position.z);
    }
}
