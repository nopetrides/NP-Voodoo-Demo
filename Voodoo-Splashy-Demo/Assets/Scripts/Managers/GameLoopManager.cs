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

	public enum GameState
	{
		Ready = 0,
		Main = 1,
		Complete,
	}

	private GameState m_State = GameState.Ready;
	public GameState State { get { return m_State; } }

	bool m_Update = false;
    // Start is called before the first frame update
    void Start()
    {
        OnSetup();
        ReadyStateSetup();
		m_ArtBGObject.SetActive(true);
    }


    void OnSetup()
	{
		// Setup/reset player states
		ProgressionManager.Instance.SetupRarity();
		WaitForInput();
	}

    void ReadyStateSetup()
	{
        m_Update = false;
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
	}


	private void OnDisable()
	{
		m_PlayerManager.TapToBegin -= GameStateMainBegin;
	}

	public void BackgroundTapped()
	{
		if ( m_State == GameState.Ready)
		{
			m_State = GameState.Main;
		}
	}
}
