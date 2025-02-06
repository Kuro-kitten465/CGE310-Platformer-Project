using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string m_GameScene = "null";

    public void OnStartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadSceneAsync(m_GameScene);
    }
}
