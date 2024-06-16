using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData", order = 1)]
public class AbilityData : ScriptableObject
{
	public float Duration;
	public float Cooldown;
	public int AbilityID;
	public Sprite Sprite;
}
