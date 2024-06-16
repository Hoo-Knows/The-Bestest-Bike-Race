using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public static ShopManager Instance;

	private ItemPanel selectedPanel;

	[SerializeField] private TMP_Text balanceText;
	[SerializeField] private Image infoPanelImage;
	[SerializeField] private TMP_Text infoPanelText;
	[SerializeField] private GameObject infoPanelOwned;
	[SerializeField] private GameObject infoPanelPurchase;
	[SerializeField] private GameObject infoPanelRicher;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		balanceText.text = "$" + GameManager.Instance.balance;
		ClearInfoPanel();
	}

	public void ClickedPanel(ItemPanel panel)
	{
		if(selectedPanel != null) selectedPanel.GetComponent<Image>().color = panel.defaultColor;
		if(selectedPanel == panel)
		{
			selectedPanel = null;
			ClearInfoPanel();
			return;
		}
		selectedPanel = panel;
		UpdateInfoPanel(panel);
		panel.GetComponent<Image>().color = panel.selectedColor;
	}

	private void UpdateInfoPanel(ItemPanel panel)
	{
		Debug.Log("Updating shop with " + panel.itemData.ItemName);
		infoPanelImage.sprite = panel.itemData.ItemSprite;
		infoPanelImage.gameObject.SetActive(true);
		infoPanelText.text = panel.itemData.ItemDetailedDescription;
		if(GameManager.Instance.unlockedAbilities[panel.itemData.ItemID])
		{
			infoPanelOwned.SetActive(true);
			infoPanelPurchase.SetActive(false);
			infoPanelRicher.SetActive(false);
		}
		else
		{
			infoPanelOwned.SetActive(false);
			infoPanelPurchase.SetActive(GameManager.Instance.balance >= panel.itemData.ItemCost);
			infoPanelRicher.SetActive(GameManager.Instance.balance < panel.itemData.ItemCost);
		}
	}

	private void ClearInfoPanel()
	{
		infoPanelImage.gameObject.SetActive(false);
		infoPanelText.text = "";
		infoPanelOwned.SetActive(false);
		infoPanelPurchase.SetActive(false);
		infoPanelRicher.SetActive(false);
	}

	public void PurchaseSelected()
	{
		GameManager.Instance.unlockedAbilities[selectedPanel.itemData.ItemID] = true;
		GameManager.Instance.balance -= selectedPanel.itemData.ItemCost;
		balanceText.text = "$" + GameManager.Instance.balance;
		UpdateInfoPanel(selectedPanel);
	}

	public void ChangeScene(string name)
	{
		GameManager.Instance.ChangeScene(name);
	}
}
