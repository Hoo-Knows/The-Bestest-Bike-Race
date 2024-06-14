using UnityEngine;

[CreateAssetMenu(fileName = "BikeData", menuName = "ScriptableObjects/BikeData", order = 1)]
public class BikeData : ScriptableObject
{
	public float MaxSpeed;
	public float Accel;
	public float Decel;
	public float GlideYSpeed = -1f;
	public float JumpSpeed = 10f;
}