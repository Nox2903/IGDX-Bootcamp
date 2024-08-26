using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using checkPointsManager.runtime;

public class TrapEvent : MonoBehaviour
{
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
    public void KillEm()
    {
        FadingUI.instance.textDesc.text = "you stepped on a trap";
        StartCoroutine(FadingUI.instance.Kill());
    }

    public void Ready()
    {
        anim.SetTrigger("Ready");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FadingUI.instance.textDesc.text = "You fall into the void";
            StartCoroutine(FadingUI.instance.Kill());
        }
    }
}
