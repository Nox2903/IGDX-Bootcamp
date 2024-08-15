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
            StartCoroutine(Kills());
        }
    }

    public IEnumerator Kills()
    {
        FadingUI.instance.textDesc.text = "you got wrecked";
        StartCoroutine(FadingUI.instance.TestFadeIn());
        yield return new WaitForSeconds(1f);
        player_Checkpoint.teleportToCheckpoint(player_Checkpoint.currentCheckpoint);
        StartCoroutine(FadingUI.instance.TestFadeOut());
    }
}
