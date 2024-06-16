using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "ScriptableObjects/ShopItemData", order = 1)]
public class ShopItemData : ScriptableObject
{
	public int ItemID;
	public string ItemName;
	public int ItemCost;
	public string ItemDescription;
	public string ItemDetailedDescription;
	public Sprite ItemSprite;
}
