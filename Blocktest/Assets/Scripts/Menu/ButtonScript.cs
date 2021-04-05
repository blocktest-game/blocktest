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

    [SerializeField] private InputField worldSeed;

    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Image characterPreview;
    [SerializeField] private int currentCharacterSprite = 0;

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
        Globals.characterPrefab = characterPrefabs[currentCharacterSprite];
        Globals.characterColor = characterPreview.color;
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void SetWorldSeed(string inputSeed) {
        float newSeed;
        if (float.TryParse(inputSeed, out newSeed)) {
            Globals.worldSeed = Mathf.Clamp(newSeed, 0.0f, 1000000.0f);
        }
    } 

    public void ChangeCharSprite(int delta = 1) {
        currentCharacterSprite += delta;
        if (currentCharacterSprite >= characterSprites.Length) {
            currentCharacterSprite = 0;
        } else if (currentCharacterSprite < 0) {
            currentCharacterSprite = characterSprites.Length - 1;
        }
        characterPreview.sprite = characterSprites[currentCharacterSprite];
    }

}
