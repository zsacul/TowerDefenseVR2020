using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextfieldInputTransfer : MonoBehaviour
{
    [SerializeField]
    private GameObject textfield;
    [SerializeField]
    private GameObject submitButton;

    private void Start()
    {
        textfield.GetComponent<TextMeshProUGUI>().text = "";
        transform.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
    }

    public void TextfieldSubmit()
    {
        FindObjectOfType<Scoreboard>().SetUsername(textfield.GetComponent<TextMeshProUGUI>().text);
        transform.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
    }
}
