using UnityEngine;
using KuroNeko.Utilities.DesignPattern;

public class GameManager : MonoSingleton<GameManager>
{
    private int m_HighestScore = 0;
    private int m_CurrentScore = 0;
    public int HighestScore
    {
        get => m_HighestScore;
        set => m_HighestScore = value;
    }

    public bool IsGameOver = false;
    public bool IsGameWin = false;

    public void OnGameWin()
    {
        IsGameWin = true;

        StaticEventBus.Invoke(KeysContainer.GameWin);
        SaveData();
    }

    public void OnGameOver(PlayerManager player)
    {
        IsGameOver = true;

        m_CurrentScore = player.PlayerScore;

        if (m_CurrentScore > m_HighestScore)
            HighestScore = m_CurrentScore;

        StaticEventBus.Invoke(KeysContainer.GameOver);

        SaveData();
    }

    protected override void Awake()
    {
        base.Awake();
        LoadSavedData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("HighestScore", m_HighestScore);
        Debug.Log("Data saved!");
    }

    public void LoadSavedData()
    {
        m_HighestScore = PlayerPrefs.GetInt("HighestScore", 0);
        Debug.Log("Data loaded!");
    }
}
