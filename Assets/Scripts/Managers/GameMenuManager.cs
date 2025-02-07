using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using KuroNeko.Utilities.DesignPattern;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private GameObject m_HeartPref;
    [SerializeField] private GameObject m_HeartPanel;
    [SerializeField] private TMP_Text m_ScoreText;
    [SerializeField] private GameObject m_GameOverPanel;
    [SerializeField] private TMP_Text m_GOScoreText;
    [SerializeField] private TMP_Text m_GOHighScoreText;
    [SerializeField] private GameObject m_GameWinPanel;
    [SerializeField] private TMP_Text m_GWScoreText;
    [SerializeField] private TMP_Text m_GWHighScoreText;

    [Header("Game Config")]
    [SerializeField] private PlayerManager m_Player;

    private List<GameObject> m_Hearts = new();

    private void Start()
    {
        StaticEventBus.Register(KeysContainer.AddHeart, CreateHeart);
        StaticEventBus.Register(KeysContainer.ReduceHeart, RemoveHeart);
        StaticEventBus.Register(KeysContainer.GameOver, OnGameOver);
        StaticEventBus.Register(KeysContainer.ScoreUpdated, OnScoreUpdate);
        StaticEventBus.Register(KeysContainer.GameWin, OnGameWin);

        for (int i = 0; i < m_Player.PlayerHealth; i++)
        {
            CreateHeart();
        }
    }

    public void CreateHeart()
    {
        var obj = Instantiate(m_HeartPref, m_HeartPanel.transform);
        obj.transform.SetParent(m_HeartPanel.transform);
        m_Hearts.Add(obj);
    }

    public void RemoveHeart()
    {
        var obj = m_Hearts.Last();
        m_Hearts.Remove(obj);
        Destroy(obj);
    }

    public void OnScoreUpdate() => m_ScoreText.text = $"Score : {m_Player.PlayerScore}";

    public void OnGameOver()
    {
        m_GameOverPanel.SetActive(true);
        m_GOScoreText.text = $"Score\n{m_Player.PlayerScore}";
        m_GOHighScoreText.text = $"Highscore\n{GameManager.Instance.HighestScore}";
    }

    public void OnGameWin()
    {
        m_GameWinPanel.SetActive(true);
        m_GWScoreText.text = $"Score\n{m_Player.PlayerScore}";
        m_GWHighScoreText.text = $"Highscore\n{GameManager.Instance.HighestScore}";
    }

    public void BackToMenu()
    {
        GameManager.Instance.IsGameOver = false;
        GameManager.Instance.IsGameWin = false;
        SceneManager.LoadSceneAsync(0);
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.IsGameOver = false;
            GameManager.Instance.IsGameWin = false;
            SceneManager.LoadSceneAsync(1);
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed) Application.Quit();
    }

    private void OnDestroy()
    {
        StaticEventBus.Unregister(KeysContainer.AddHeart);
        StaticEventBus.Unregister(KeysContainer.ReduceHeart);
        StaticEventBus.Unregister(KeysContainer.ScoreUpdated);
        StaticEventBus.Unregister(KeysContainer.GameOver);
        StaticEventBus.Unregister(KeysContainer.GameWin);
    }
}
