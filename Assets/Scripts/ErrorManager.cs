using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
    public TextMeshProUGUI errorText;

    private void Start()
    {
        if (errorText == null)
        {
            Debug.LogError("Error Text is not assigned in the inspector.");
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
