using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using checkPointsManager.runtime;

public class RopeTrap : MonoBehaviour
{
    public Player_Checkpoint player_Checkpoint;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Kills();
        }
    }

    public void Kills()
    {
        FadingUI.instance.textDesc.text = "you got wrecked";
        StartCoroutine(FadingUI.instance.Kill());
    }
}
