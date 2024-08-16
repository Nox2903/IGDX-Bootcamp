using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [System.Serializable]
    public class Quest
    {
        public string title;
        public string id;
        public string description;
        public bool isCompleted;
    }

    [SerializeField] List<Quest> questBank;
    public List<Quest> currentActiveQuest;

    public void Awake()
    {
        instance = this;
    }

    Quest GetQuestInBank(string questID)
    {
        Quest tempQuest = null;

        for (int i = 0; i < questBank.Count; i++)
        {
            if (questID == questBank[i].id)
            {
                tempQuest = questBank[i];
            }
        }

        if (tempQuest == null)
            Debug.Log("Quest with ID Targeted not available");

        return tempQuest;
    }

    public void AddActiveQuest(string questID)
    {
        bool status = true;
        Quest targetAddedQuest = GetQuestInBank(questID);

        if (targetAddedQuest != null) {
            for (int i = 0; i < currentActiveQuest.Count; i++)
            {
                if (currentActiveQuest[i] == targetAddedQuest)
                    status = false;
            }

            if (status)
            {
                currentActiveQuest.Add(targetAddedQuest);
                UpdatingQuestUI();
            }
            else
                Debug.Log("Quest with these ID is already Active");
        }
    }

    public int GetActiveQuestIndex(string questID)
    {
        int tempIndex = -1;

        for (int i = 0; i < currentActiveQuest.Count; i++)
        {
            if (questID == currentActiveQuest[i].id)
            {
                tempIndex = i;
            }
        }

        return tempIndex;
    }

    public bool IsQuestActive(string questID)
    {
        bool status = false;

        if (GetActiveQuestIndex(questID) != -1)
            status = true;

        return status;
    }

    public void SetActiveQuestStatus(string questID, bool status)
    {
        if (GetActiveQuestIndex(questID) != -1)
        {
            currentActiveQuest[GetActiveQuestIndex(questID)].isCompleted = status;
            UpdatingQuestUI();
        }
        else
            Debug.Log("Active Quest with ID Targeted not available");
    }

    public void UpdatingQuestUI()
    {
        string tempQuestString = "";
        for (int i = currentActiveQuest.Count - 1; i >= 0; i--)
        {
            string tempQuestStatus = "X";
            if (currentActiveQuest[i].isCompleted)
                tempQuestStatus = "V";

            tempQuestString += currentActiveQuest[i].title + "(" + tempQuestStatus + ")";

            if (i > 0)
                tempQuestString += "\n";
        }
        QuestPanel.instance.UpdatingQuestText(tempQuestString);
    }

    public void UpdatingQuestUIOne()
    {
        string tempQuestString = "";
        for (int i = currentActiveQuest.Count - 1; i >= 0; i--)
        {
            string tempQuestStatus = "X";
            if (!currentActiveQuest[i].isCompleted)
            {
                tempQuestString += currentActiveQuest[i].title;
                if (i > 0)
                    tempQuestString += "\n";
            }
        }
        QuestPanel.instance.UpdatingQuestText(tempQuestString);
    }
}
