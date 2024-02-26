using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CampaignNPC : MonoBehaviour
{
    private bool isPlayerInRange;
    private CameraController cameraController;
    public GameObject InteractionIndicator;
    public GameObject DialogueUI;
    public TMP_Text dialogueText; // The UI Text element that displays the dialogue
    public Dialogue dialogue;
    private int currentLineIndex = 0;

    private void Start()
    {
        HideInteractButton();
        DialogueUI.SetActive(false);
        isPlayerInRange = false;
        cameraController = FindObjectOfType<CameraController>(); // Get the CameraController
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the "F" key was pressed and the player is in range
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            // If dialogue UI is not active, show it and hide the interact prompt
            if (!DialogueUI.activeSelf)
            {
                cameraController.ZoomIn(); // Call ZoomIn when starting the dialogue
                StartDialogue(); // Start the dialogue
                DialogueUI.SetActive(true); // Show the dialogue UI
                HideInteractButton();
                Debug.Log("Starting dialogue.");
            }
            else
            {
                // If dialogue UI is already active, show the next line of dialogue
                DisplayNextLine();

                // If dialogue has ended, reactivate the interact prompt
                if (!IsDialogueActive())
                {
                    Debug.Log("Dialogue has ended.");
                    DialogueUI. SetActive(false);
                    currentLineIndex = 0;
                    cameraController.ZoomOut(); // Call ZoomOut when dialogue ends
                    ShowInteractButton();
                }
            }
        }
    }

    public void Interact(){

    }
    public void ShowInteractButton()
    {
        if (InteractionIndicator != null)
        {
            InteractionIndicator.SetActive(true); // Show the indicator
        }
    }
    public void HideInteractButton()
    {
        if (InteractionIndicator != null)
        {
            InteractionIndicator.SetActive(false); // Hide the indicator
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Only show the interact prompt if the dialogue UI isn't currently active
            Debug.Log("Player entered the trigger area.");
            ShowInteractButton();
            isPlayerInRange = true; // Set the flag to true since the player is in range
        }
    }

    // This method is automatically called when something exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Hide both the interact prompt and the dialogue UI when the player exits the trigger
            Debug.Log("Player exited the trigger area.");
            HideInteractButton();
            isPlayerInRange = false; // Set the flag to false since the player is out of range
            cameraController.ZoomOut(); // Zoom out when the player leaves the interaction area
            DialogueUI. SetActive(false);
            currentLineIndex = 0; 
        }
    }

  
    public void StartDialogue()
    {
        if (dialogue.dialogueLines.Length > 0)
        {
            currentLineIndex = 0; // Start from the first line
            dialogueText.text = dialogue.dialogueLines[currentLineIndex]; // Display it
            dialogueText.gameObject.SetActive(true); // Make sure the text is visible
        }
    }

    // Call this method to show the next line of dialogue, or end the dialogue if there are no more lines
    public void DisplayNextLine()
    {
        currentLineIndex++; // Move to the next line

        if (currentLineIndex < dialogue.dialogueLines.Length)
        {
            dialogueText.text = dialogue.dialogueLines[currentLineIndex]; // Display the next line
        }
        else
        {
            DialogueUI. SetActive(false); // No more lines, hide the text
            // Optionally, reset the dialogue to the first line or handle the end of the dialogue
            EndDialogue();
        }
    }

    // Call this method to handle what happens when the dialogue ends
    private void EndDialogue()
    {
        // For example, you could reset the dialogue or trigger other game events
        currentLineIndex = 0; // Reset the dialogue index if you want to start from the beginning next time
    }

    public bool IsDialogueActive()
    {
        // Check if the dialogue index is beyond the last line
        return currentLineIndex < dialogue.dialogueLines.Length;
    }
    

}