using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTrigger : MonoBehaviour, IInteractable
{
    public GameObject player;
    public Transform nextRoom;
    public Rigidbody rb;
    public GameObject popUp;
    public bool isRight;

    public GameObject PopUp
    {
        get { return popUp; }
    }

    public void Interact()
    {
        if (isRight == false)
        {
            rb.isKinematic = false;
            Debug.Log("ded");
        }
        else if (isRight)
        {
            TeleportPlayer();
            Debug.Log("teleported");
        }
    }

    public void UnInteract()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TeleportPlayer()
    {
        
        FadingUI.instance.textDesc.text = "going to next room";
        StartCoroutine(FadingUI.instance.Teleport(nextRoom));
    }
}
