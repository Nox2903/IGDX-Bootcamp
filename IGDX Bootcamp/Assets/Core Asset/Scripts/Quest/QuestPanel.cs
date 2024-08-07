using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    public static QuestPanel instance;
    public TextMeshProUGUI questText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdatingQuestText(string questString)
    {
        questText.text = questString;
    }
}
