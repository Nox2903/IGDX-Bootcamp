using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    private bool UIIsAlreadyActive = false;
    // private void OnColisionEnter(Collision col)
    // {
    //     if(col.gameObject.CompareTag("Player"))
    //     UI.SetActive(true);
    // }

     private void OnColisionExit(Collision col)
    {
          if(col.gameObject.CompareTag("Player") && !UIIsAlreadyActive)
          {
        UI.SetActive(false);
          }
    }
}
