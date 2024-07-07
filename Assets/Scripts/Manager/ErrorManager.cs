using TMPro;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
    public TextMeshProUGUI errorText;

    private void Start()
    {
        if (errorText == null)
        {
            Debug.LogError("Hata Metni isnpector'de atanmamistir.");
        }
        else
        {
            errorText.text = "";
        }
    }

    public void ShowErrorMessage(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
        }
    }
}
