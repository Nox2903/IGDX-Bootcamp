using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter()
    {
        anim.SetTrigger("Gottem");
    }

    public void OnTriggerExit()
    {
    }
}
