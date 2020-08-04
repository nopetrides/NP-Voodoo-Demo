using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private enum MenuState
    {
        Main = 0,
        About = 1,
        Loading,
    }

    [SerializeField]
    private List<GameObject> MenuLayouts = new List<GameObject>();
    [SerializeField]
    private AudioClip MenuMusic = null;

    MenuState m_State = MenuState.Main;

    private void Start()
    {
        Setup();

        AudioManager.Instance.PlayLoopingMusic(MenuMusic);
    }
    private void SetMenuState(MenuState state)
    {
        m_State = state;
        switch (m_State)
        {
            case MenuState.Main:
                MainMenuState();
                break;
            case MenuState.About:
                AboutMenuState();
                break;
            case MenuState.Loading:
                LoadingState();
                break;
            default:
                Debug.LogError("Menu state \"" + state + "\" is not supported!");
                break;
        }
    }

    // Loop through the layouts and turn them all off except the one that matches the menu state
    void SetLayoutStates(int menuStateAsInt)
    {
        for (int i = 0; i < MenuLayouts.Count; i++)
        {
            if (MenuLayouts[i] != null)
            {
                MenuLayouts[i].SetActive(i == menuStateAsInt);
            }
        }
    }
    void Setup()
    {
        SetMenuState(MenuState.Main);
    }
    void MainMenuState()
    {
        SetLayoutStates((int)MenuState.Main);
    }

    void AboutMenuState()
    {
        SetLayoutStates((int)MenuState.About);
    }

    void LoadingState()
	{
        SetLayoutStates((int)MenuState.Loading);
    }

    public void ReturnToMainMenu()
    {
        SetMenuState(MenuState.Main);
    }

    public void ButtonAboutMenu()
    {
        SetMenuState(MenuState.About);
    }
    public void ButtonStartGame()
    {
        SetMenuState(MenuState.Loading);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void PlayButtonPressedSound()
    {
        AudioManager.PlayButtonPressedSFX();
    }
    public void ButtonExitPressed()
    {
        QuitGame();
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}
