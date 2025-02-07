using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string m_GameScene = "null";
    [SerializeField] string m_LevelName = "The Jungle";
    [SerializeField] GameObject m_LoadingPanel;
    [SerializeField] private TMP_Text m_LevelText;
    [SerializeField] private TMP_Text m_ScoreText;

    private void Start()
    {
        GameManager.Instance.LoadSavedData();
    }

    public void OnStartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
            StartCoroutine(LoadScene());
    }

    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.performed)
            Application.Quit();
    }

    IEnumerator LoadScene()
    {
        m_LoadingPanel.SetActive(true);
        m_LevelText.text = m_LevelName;
        m_ScoreText.text = $"Hightest score : {GameManager.Instance.HighestScore}";

        yield return new WaitForSeconds(3f);

        var operation = SceneManager.LoadSceneAsync(m_GameScene, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
