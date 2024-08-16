using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoal : MonoBehaviour
{
    [SerializeField] private string questID;

    public enum DetectionMethod
    {
        OnTriggerEnter,
        OnCollisionEnter
    }

    [SerializeField] private DetectionMethod detectionMethod;

    public void FinishingQuest()
    {
        QuestManager.instance.SetActiveQuestStatus(questID, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (detectionMethod == DetectionMethod.OnTriggerEnter && other.CompareTag("Player"))
        {
            FinishingQuest();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (detectionMethod == DetectionMethod.OnCollisionEnter && collision.gameObject.CompareTag("Player"))
        {
            FinishingQuest();
        }
    }
}
