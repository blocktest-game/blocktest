using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private string currentState = "main";

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject singleplayerScreen;
    [SerializeField] private GameObject multiplayerScreen;
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private Sprite[] logoBG;
    [SerializeField] private Image logo;

    private void Start()
    {
        int i = Random.Range(0, logoBG.Length);
        logo.sprite = logoBG[i];
    }

    public void SwitchState(string newState = "main")
    {
        switch (currentState) {
            case "main":
                titleScreen.SetActive(false);
                break;
            case "single":
                singleplayerScreen.SetActive(false);
                break;
            case "multi":
                multiplayerScreen.SetActive(false);
                break;
        }
        currentState = newState;
        switch (newState) {
            case "main":
                titleScreen.SetActive(true);
                break;
            case "single":
                singleplayerScreen.SetActive(true);
                break;
            case "multi":
                multiplayerScreen.SetActive(true);
                break;
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartSingleplayer()
    {
        SwitchState(null);
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("MainScene");
    }

}
