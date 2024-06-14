using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private Transform waypointParent;
    [SerializeField] private float speed;

	[SerializeField] private Transform bikeParent;
    private List<Transform> bikes;

	private Transform targetWaypoint;
    private int currentTarget = 0;

    // Start is called before the first frame update
    void Start()
    {
		targetWaypoint = waypointParent.GetChild(0);
        bikes = new List<Transform>();
    }

	private void FixedUpdate()
	{
		transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.fixedDeltaTime);
		bikeParent.position = Vector2.MoveTowards(bikeParent.position, targetWaypoint.position, speed * Time.fixedDeltaTime);
        if(Vector2.Distance(transform.position, targetWaypoint.position) < 0.05f)
        {
            currentTarget++;
            if(currentTarget >= waypointParent.childCount) currentTarget = 0;
            targetWaypoint = waypointParent.GetChild(currentTarget);
        }
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.GetComponent<Bike>())
        {
			//Debug.Log("collision enter");
			Transform bike = collider.gameObject.transform;
            bike.SetParent(bikeParent);
            bikes.Add(bike);
        }
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.gameObject.GetComponent<Bike>())
		{
			//Debug.Log("collision exit");
			Transform bike = collider.gameObject.transform;
			bike.SetParent(null);
			bikes.Remove(bike);
		}
	}
}
