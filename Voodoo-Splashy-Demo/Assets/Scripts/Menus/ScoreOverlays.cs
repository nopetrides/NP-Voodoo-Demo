using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreOverlays : MonoBehaviour
{
	[SerializeField]
	private Text m_PointScoredPopup = null;

	[SerializeField]
	private Text m_CritScoredPopup = null;

	private void Start()
	{
		m_PointScoredPopup.gameObject.SetActive(false);
		m_CritScoredPopup.gameObject.SetActive(false);
	}

	public void ShowScoreAdded(int points)
	{
		m_PointScoredPopup.gameObject.SetActive(true);
		m_PointScoredPopup.text = "+" + points;
	}

	public void ShowCrit()
	{
		m_CritScoredPopup.gameObject.SetActive(true);
	}
}
