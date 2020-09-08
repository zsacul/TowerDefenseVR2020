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
    [SerializeField]
    private GameObject resetScoreboardButton;

    private void Start()
    {
        textfield.GetComponent<TextMeshProUGUI>().text = "";
        transform.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        resetScoreboardButton.gameObject.SetActive(true);
    }

    public void TextfieldSubmit()
    {
        Scoreboard.Instance.SetUsername(textfield.GetComponent<TextMeshProUGUI>().text);
        transform.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        resetScoreboardButton.gameObject.SetActive(false);
    }

    public void ResetScoreboardClicked()
    {
        Scoreboard.Instance.ResetScoreboard();
    }
}
