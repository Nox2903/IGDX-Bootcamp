using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RapidPressInteractable : MonoBehaviour, IInteractable
{
    public GameObject popUp;
    public GameObject popUp2;
    public GameObject PopUp => popUp;
    public List<GameObject> nextObjs; // List of objects to be destroyed
    public Slider timerSlider; // Reference to the UI Slider for the timer
    public Slider pressCounterSlider; // Reference to the UI Slider for the press counter

    [SerializeField] private bool isInteracting = false;
    [SerializeField] private bool isOpen = false;
    private int rapidPressCounter = 0;
    private float rapidPressWindow = 3f; // Time window for rapid presses
    private float timer = 0f;
    [SerializeField] private int minPress = 18;

    void Start()
    {
        // Initialize the UI sliders if they are assigned
        if (timerSlider != null)
        {
            timerSlider.maxValue = rapidPressWindow;
            timerSlider.value = 0;
            timerSlider.gameObject.SetActive(false);
        }
        if (pressCounterSlider != null)
        {
            pressCounterSlider.maxValue = minPress;
            pressCounterSlider.value = 0;
            pressCounterSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isInteracting && !isOpen)
        {
            timer += Time.deltaTime;

            // Update the timer slider UI
            if (timerSlider != null)
            {
                timerSlider.value = timer;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                rapidPressCounter++;
                Debug.Log("Pressed R. Count: " + rapidPressCounter);

                // Update the press counter slider UI
                if (pressCounterSlider != null)
                {
                    pressCounterSlider.value = rapidPressCounter;
                }

                if (rapidPressCounter >= minPress) // Required presses to destroy the current object
                {
                    HandleNextObject();
                }
            }

            if (timer >= rapidPressWindow)
            {
                ResetRapidPress();
            }
        }
    }

    public void Interact()
    {
        if (!isOpen && !isInteracting)
        {
            StartRapidPress();
        }
    }

    public void UnInteract()
    {
        // Logic to handle when un-interacting, if necessary
    }

    private void StartRapidPress()
    {
        isInteracting = true;
        timer = 0f;
        rapidPressCounter = 0;
        Debug.Log("Start rapid pressing R!");

        // Reset UI sliders at the start of the interaction
        if (timerSlider != null)
        {
            timerSlider.gameObject.SetActive(true);
            timerSlider.value = 0;
        }
        if (pressCounterSlider != null)
        {
            pressCounterSlider.gameObject.SetActive(true);
            pressCounterSlider.value = 0;
        }

        if (popUp != null) popUp.SetActive(false);
        if (popUp2 != null) popUp2.SetActive(true);
    }

    private void ResetRapidPress()
    {
        isInteracting = false;
        timer = 0f;
        rapidPressCounter = 0;
        Debug.Log("Failed to destroy the object in time.");

        // Reset UI sliders if needed
        if (timerSlider != null)
        {
            timerSlider.gameObject.SetActive(false);
            timerSlider.value = 0;
        }
        if (pressCounterSlider != null)
        {
            pressCounterSlider.gameObject.SetActive(false);
            pressCounterSlider.value = 0;
        }

        // Disable further rapid pressing until player interacts again
        isInteracting = false;
    }

    private void CancelRapidPress()
    {
        isInteracting = false;
        timer = 0f;
        rapidPressCounter = 0;
        Debug.Log("Rapid press interaction canceled.");

        // Reset UI sliders if needed
        if (timerSlider != null)
        {
            timerSlider.gameObject.SetActive(false);
            timerSlider.value = 0;
        }
        if (pressCounterSlider != null)
        {
            pressCounterSlider.gameObject.SetActive(false);
            pressCounterSlider.value = 0;
        }

        // Disable further rapid pressing until player interacts again
        isInteracting = false;
    }

    private void HandleNextObject()
    {
        if (nextObjs.Count > 0)
        {
            GameObject objToDestroy = nextObjs[0];
            nextObjs.RemoveAt(0);
            Destroy(objToDestroy);
            Debug.Log("Object destroyed!");

            // Reset the state to require another interaction for the next object
            isInteracting = false;

            // Disable UI sliders
            if (timerSlider != null) timerSlider.gameObject.SetActive(false);
            if (pressCounterSlider != null) pressCounterSlider.gameObject.SetActive(false);

            if (nextObjs.Count == 0)
            {
                CompleteInteraction();
            }
            else
            {
                // Allow the player to press E again for the next object
                if (popUp != null) popUp.SetActive(true);
                if (popUp2 != null) popUp2.SetActive(false);
            }
        }
    }

    private void CompleteInteraction()
    {
        isOpen = true;
        isInteracting = false;
        Debug.Log("All objects destroyed! Interaction complete.");
        Destroy(this.gameObject);
        if (timerSlider != null) timerSlider.gameObject.SetActive(false);
        if (pressCounterSlider != null) pressCounterSlider.gameObject.SetActive(false);
    }

    void OnTriggerExit()
    {
        popUp2.SetActive(false);
        CancelRapidPress();
    }
}
