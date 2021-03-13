using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private string currentState = "main";

    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject singleplayerScreen;
    [SerializeField] GameObject multiplayerScreen;
    [SerializeField] GameObject loadingScreen;

    [SerializeField] Sprite[] logoBG;
    [SerializeField] Image logo;

    private void Start() {
        int i = Random.Range(0, logoBG.Length);
        logo.sprite = logoBG[i];
    }

    public void switchState(string newState = "main") {
        switch(currentState) {
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
        switch (newState){
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

    public void exitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void startSingleplayer() {
        switchState(null);
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("MainScene");
    }

}
