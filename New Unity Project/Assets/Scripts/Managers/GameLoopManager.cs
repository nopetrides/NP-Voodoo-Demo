using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CompleteMenu = null;
	[SerializeField]
	private float m_ReadyDuration = 1.0f;
	[SerializeField]
	private GameObject m_ReadyParent = null;
	[SerializeField]
	private Text m_ScoreText = null;
	[SerializeField]
	private PlayerBounceManager m_PlayerManager = null;

	public enum GameState
	{
		Ready = 0,
		Main = 1,
		Complete,
	}

	public GameState m_State = GameState.Ready;

	bool m_Update = false;
    // Start is called before the first frame update
    void Start()
    {
        OnSetup();
        ReadyStateSetup();
    }


    void OnSetup()
	{
		// Setup/reset player states
		WaitForInput();
	}

    void ReadyStateSetup()
	{
        m_Update = false;
		m_State = GameState.Ready;
		m_CompleteMenu.SetActive(false);
		m_ReadyParent.SetActive(true);
		m_ScoreText.text = "0";
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
		m_State = GameState.Main;
		m_ReadyParent.SetActive(false);
	}

	void GameStateOver()
	{
		m_State = GameState.Complete;
		m_CompleteMenu.SetActive(true);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDisable()
	{
		m_PlayerManager.TapToBegin -= GameStateMainBegin;
	}
}
