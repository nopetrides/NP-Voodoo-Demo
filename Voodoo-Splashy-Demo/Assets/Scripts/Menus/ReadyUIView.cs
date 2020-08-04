using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUIView : MonoBehaviour
{
	[SerializeField]
	private ScoreUIView m_ScoreUI = null;

	[SerializeField]
	private Button m_BuyUpgradeButton = null;
	[SerializeField]
	private Image m_NextRarityWeaponImage = null;
	[SerializeField]
	private Text m_BuyButtonText = null;
	[SerializeField]
	private Button m_ExitButton = null;

	private void OnEnable()
	{
		// Setup buttons
		SetBuyButtonText();
		m_BuyUpgradeButton.onClick.AddListener(BuyButtonPressed);
		m_ExitButton.onClick.AddListener(ExitButton);
	}

	private void OnDisable()
	{
		m_BuyUpgradeButton.onClick.RemoveAllListeners();
		m_ExitButton.onClick.RemoveAllListeners();
	}

	private void BuyButtonPressed()
	{
		m_BuyUpgradeButton.enabled = false;
		if (ScoreManager.Instance.TrySpendMoney(ProgressionData.Instance.NextRarityCost()))
		{
			ProgressionData.Instance.UpgradeRarity();
			ProgressionManager.Instance.SetupRarity();
			//Debug.Log("Subtracted money and buying");
		}
		else
		{
			//Debug.Log("Not Enough Dosh");
		}

		SetBuyButtonText();
		m_BuyUpgradeButton.enabled = true;
	}

	private void SetBuyButtonText()
	{
		if (ProgressionData.Instance.NextRarityCost() > 0)
		{
			m_NextRarityWeaponImage.sprite = ProgressionData.Instance.GetNextRarityImage();
			m_BuyButtonText.text = ProgressionData.Instance.NextRarityCost().ToString();
		}
		else
		{
			m_NextRarityWeaponImage.gameObject.SetActive(false);
			m_BuyButtonText.text = "Done";
		}
	}

	private void ExitButton()
	{
		ProgressionData.Instance.ResetRarity();
		ScoreManager.Instance.ResetBestAndMoney();
	}
}
