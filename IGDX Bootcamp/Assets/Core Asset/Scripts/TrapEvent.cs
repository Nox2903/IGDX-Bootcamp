using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using checkPointsManager.runtime;

public class TrapEvent : MonoBehaviour
{
    public Player_Checkpoint player_Checkpoint;
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator KillEm()
    {
        FadingUI.instance.textDesc.text = "you stepped on a trap";
        StartCoroutine(FadingUI.instance.TestFadeIn());
        yield return new WaitForSeconds(1f);
        player_Checkpoint.teleportToCheckpoint(player_Checkpoint.currentCheckpoint);
        StartCoroutine(FadingUI.instance.TestFadeOut());
    }

    public void Ready()
    {
        anim.SetTrigger("Ready");
    }
}
