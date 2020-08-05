using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : Singleton<GameLoopManager>
{
    [SerializeField]
    private GameObject m_CompleteMenu = null;
	[SerializeField]
	private GameObject m_ReadyParent = null;
	[SerializeField]
	private PlayerBounceManager m_PlayerManager = null;
	[SerializeField]
	private GameObject m_ArtBGObject = null;
	[SerializeField]
	private int m_SecondsUntilReset = 5;

	public enum GameState
	{
		Ready = 0,
		Main = 1,
		Complete,
	}

	private GameState m_State = GameState.Ready;
	public GameState State { get { return m_State; } }

	public static event Action Reset;

    // Start is called before the first frame update
    void Start()
	{
		m_ArtBGObject.SetActive(true);
		OnSetup();
        ReadyStateSetup();
	}


    void OnSetup()
	{
		// Setup/reset player states
		ResetGameManager();
		WaitForInput();
	}

    void ReadyStateSetup()
	{
		m_State = GameState.Ready;
		m_CompleteMenu.SetActive(false);
		m_ReadyParent.SetActive(true);
		AudioManager.Instance.StopAnyCurrentMusic();
	}

    void WaitForInput()
	{
		// flag to set that waits for tap/drag in start area
		m_PlayerManager.TapToBegin += GameStateMainBegin;
		m_PlayerManager.PlayerLose += GameStateOver;
	}

	void GameStateMainBegin()
	{
		m_ReadyParent.SetActive(false);
	}

	void GameStateOver()
	{
		m_State = GameState.Complete;
		m_CompleteMenu.SetActive(true);
		StartCoroutine(GameOverWait());
	}

	IEnumerator GameOverWait()
	{
		yield return new WaitForSeconds(m_SecondsUntilReset);
		Reset?.Invoke();
		ResetGameManager();
	}

	private void ResetGameManager()
	{
		ReadyStateSetup();
		ProgressionManager.Instance.SetupRarity();
	}


	private void OnDisable()
	{
		m_PlayerManager.TapToBegin -= GameStateMainBegin; 
		m_PlayerManager.PlayerLose -= GameStateOver;
	}

	public void BackgroundTapped()
	{
		if ( m_State == GameState.Ready)
		{
			m_State = GameState.Main;
		}
	}
}
