using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Bike bike;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(bike.transform.position.x, bike.transform.position.y, transform.position.z);
    }
}
