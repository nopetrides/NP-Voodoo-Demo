using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField]
    private PlayerBounceManager m_Player = null;
    [SerializeField]
    private  ScoreUIView m_ScoreUI = null;

    const string BEST_SCORE_PREF_KEY = "BEST_SCORE_PREF";
    const string CURRENT_MONEY_PREF_KEY = "CURRENT_MONEY_PREF";

    public int BestScore { get { return PlayerPrefs.GetInt(BEST_SCORE_PREF_KEY, 0); } } // this could come from the server instead of stored in player prefs
    public int CurrentMoney { get { return PlayerPrefs.GetInt(CURRENT_MONEY_PREF_KEY, 0); ; } } // this could also come from the server after a login step

    private int m_CurrentScore;
    public int CurrentScore { get { return m_CurrentScore; } }

    private int m_CurrentMultiplier = 1;
    public int CurrentMultiplier { get { return m_CurrentMultiplier; } }

    public static event Action MultiplierScored;

    void Start()
    {
        m_CurrentScore = 0;
    }

    public void AddScore(int pointsEarned)
	{
        pointsEarned *= CurrentMultiplier;
        m_CurrentScore += pointsEarned;
        UpdateScoreUI(pointsEarned);
        CheckForHighScore();
    }
    public void AddMoney(int moneyEarned)
    {
        SaveMoney(moneyEarned);
    }

    public void CheckForHighScore()
	{
        if (m_CurrentScore > BestScore)
		{
            PlayerPrefs.SetInt(BEST_SCORE_PREF_KEY, m_CurrentScore);
        }
	}

    public void ResetScore()
	{
        m_CurrentScore = 0;
        m_CurrentMultiplier = 1;
        UpdateScoreUI();
    }

	public bool TrySpendMoney(int cost)
	{
		if (CurrentMoney >= cost)
		{
			SaveMoney(cost *-1);
			return true; // purchase success
		}
		else
		{
            // not enough money
            return false;
		}
	}

    private void SaveMoney(int moneyAdjustment)
	{
        PlayerPrefs.SetInt(CURRENT_MONEY_PREF_KEY, CurrentMoney + moneyAdjustment);
        UpdateMoneyUI();
    }

    private void UpdateScoreUI(int earned = -1)
	{
        if (earned > 0)
		{
            m_ScoreUI.ScoreOverlayText(earned);
        }
        m_ScoreUI.ChangeScoreText(m_CurrentScore.ToString());
	}

    private void UpdateMoneyUI()
	{
        m_ScoreUI.ChangeMoneyText(CurrentMoney.ToString());
	}

    public void ResetBestAndMoney()
	{
        PlayerPrefs.SetInt(CURRENT_MONEY_PREF_KEY, 0);
        PlayerPrefs.SetInt(BEST_SCORE_PREF_KEY, 0);
    }

    public void AddMultiplier()
	{
        m_CurrentMultiplier += 1;
        MultiplierScored?.Invoke();
        DoCritUI();
    }

    private void DoCritUI()
	{
        m_ScoreUI.ScoreMultiplierText();
    }
}
