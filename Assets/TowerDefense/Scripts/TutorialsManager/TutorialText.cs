using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI textHolder;
    public string TextAftertutorial;

    public void SetText()
    {
        textHolder.text = TextAftertutorial;
    }
}
