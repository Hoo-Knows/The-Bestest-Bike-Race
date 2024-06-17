using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public GameObject[] clouds;
    public float speed;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            if(t != transform)
            {
                t.localScale = Vector2.one * Random.Range(1f, 1.5f);
            }
        }
    }

	private void Update()
	{
		transform.position += speed * Vector3.left * Time.deltaTime;
	}
}
