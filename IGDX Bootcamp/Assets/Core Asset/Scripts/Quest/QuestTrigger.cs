using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private string questID;

    public enum DetectionMethod
    {
        OnTriggerEnter,
        OnCollisionEnter
    }

    [SerializeField] private DetectionMethod detectionMethod;

    public void TriggeringQuest()
    {
        QuestManager.instance.AddActiveQuest(questID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (detectionMethod == DetectionMethod.OnTriggerEnter && other.CompareTag("Player"))
        {
            TriggeringQuest();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (detectionMethod == DetectionMethod.OnCollisionEnter && collision.gameObject.CompareTag("Player"))
        {
            TriggeringQuest();
        }
    }
}
