using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ProgressionManager : Singleton<ProgressionManager>
{
	[SerializeField]
	private GameObject[] m_SwordMeshs = null;
	[SerializeField]
	private Material m_PlayerSpriteMat = null;
	[SerializeField]
	private Light2D m_PlayerLight = null;

	public void SetupRarity()
	{
		int rarity = ProgressionData.Instance.PlayerRarity;
		for (int i = 0; i < m_SwordMeshs.Length; i++)
		{
			m_SwordMeshs[i].SetActive(i == rarity - 1);
		}
		Color c = ProgressionData.Instance.GetColorForRarity(rarity);

		m_PlayerSpriteMat.SetColor("_Color", c);
		m_PlayerLight.color = c;
	}

}
