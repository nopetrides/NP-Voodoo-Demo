using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionData : Singleton<ProgressionData>
{
	[SerializeField]
	private int[] RarityUpgradeCosts = null;
	[SerializeField]
	[ColorUsage(true, true)]
	private Color[] RarityColor = null;
	[SerializeField]
	private Sprite[] m_SwordSprites = null;
	public Sprite GetNextRarityImage()
	{
		return m_SwordSprites[PlayerRarity - 1];
	}

	const string PLAYER_SWORD_RARITY_PREF_KEY = "PLAYER_SWORD_RARITY_PREF";

	public int PlayerRarity { get { return PlayerPrefs.GetInt(PLAYER_SWORD_RARITY_PREF_KEY, 1); } } // this could also come from the server instead of stored in player prefs

	public void UpgradeRarity()
	{
		PlayerPrefs.SetInt(PLAYER_SWORD_RARITY_PREF_KEY, PlayerRarity + 1);
	}

	public void ResetRarity()
	{
		PlayerPrefs.SetInt(PLAYER_SWORD_RARITY_PREF_KEY, 1);
	}

	public int NextRarityCost()
	{
		int nextTier = PlayerRarity;
		if (RarityUpgradeCosts != null && RarityUpgradeCosts.Length >= nextTier)
		{
			return RarityUpgradeCosts[nextTier-1];
		}
		else
		{
			return -1;
		}
	}

	public Color GetColorForRarity(int rarity)
	{
		return RarityColor[rarity-1];
	}
}
