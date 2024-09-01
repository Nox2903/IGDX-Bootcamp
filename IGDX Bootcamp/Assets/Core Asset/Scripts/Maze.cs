using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Rigidbody[] childArray;
    // Start is called before the first frame update
    void Start()
    {
        childArray = this.GetComponentsInChildren<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BreakPlane()
    {
        for (int i = 0; i < childArray.Length; i++)
        {
            childArray[i].isKinematic = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BreakPlane();
            FadingUI.instance.textDesc.text = "You fall into the void";
            StartCoroutine(FadingUI.instance.Kill());
        }
    }
}
