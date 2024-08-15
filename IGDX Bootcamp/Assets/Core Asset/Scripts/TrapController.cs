using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public Animator anim;
    Collider col;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponentInChildren<Animator>();
        col = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            anim.SetTrigger("Closed");
            col.enabled = false;
        }
        else if (other.gameObject.tag == "Player")
        {
            anim.SetTrigger("Gottem");
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            col.enabled = true;
        }
    }
}
