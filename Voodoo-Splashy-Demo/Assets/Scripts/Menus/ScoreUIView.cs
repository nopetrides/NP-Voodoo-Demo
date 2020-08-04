using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIView : MonoBehaviour
{
	[SerializeField]
	private Text m_ScoreField = null;
	[SerializeField]
	private Text m_MoneyField = null;
	[SerializeField]
	private ScoreOverlays m_ScoringOverlay = null;

	private void Start()
	{
		ChangeMoneyText(ScoreManager.Instance.CurrentMoney.ToString());
		ShowBestScore();
	}

	public void ChangeScoreText(string toText)
	{
		m_ScoreField.text = toText;
	}

	public void ChangeMoneyText(string toText)
	{
		m_MoneyField.text = toText;
	}

	public void ScoreOverlayText(int points) 
	{
		m_ScoringOverlay.ShowScoreAdded(points);

	}
	public void ScoreMultiplierText()
	{
		m_ScoringOverlay.ShowCrit();

	}

	private void ShowBestScore()
	{
		ChangeScoreText(string.Format("Best:{0}", ScoreManager.Instance.BestScore));
	}
}
