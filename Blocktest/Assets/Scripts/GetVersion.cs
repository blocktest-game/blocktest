using UnityEngine;
using UnityEngine.UI;

public class GetVersion : MonoBehaviour
{
    /// <summary> The text box to update with the latest application version. </summary>
    [SerializeField] private Text textToChange;
    /// <summary> The format of the version to replace the text with. Put "%V" where the version number should go. </summary>
    [SerializeField] private string format = "%V";

    void Start()
    {
        if(textToChange == null) {
            textToChange = GetComponent<Text>();
        }
        string result = format.Replace("%V", Application.version);
        textToChange.text = result;
    }
}
