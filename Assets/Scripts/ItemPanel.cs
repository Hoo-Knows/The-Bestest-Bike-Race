using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour, IPointerClickHandler
{
	public ShopItemData itemData;
	public Color defaultColor;
	public Color selectedColor;

	private void Start()
	{
		defaultColor = GetComponent<Image>().color;
		selectedColor = new Color(0.6f * defaultColor.r, 0.6f * defaultColor.g, 0.6f * defaultColor.b, defaultColor.a);
		transform.Find("Name").GetComponent<TMP_Text>().text = string.Format("{0} - ${1}", itemData.ItemName, itemData.ItemCost);
		transform.Find("Desc").GetComponent<TMP_Text>().text = itemData.ItemDescription;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ShopManager.Instance.ClickedPanel(this);
	}
}
