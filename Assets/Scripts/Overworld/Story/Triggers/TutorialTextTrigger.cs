using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextTrigger : StoryTrigger
{
    [SerializeField] string textToDisplay = "";
    [SerializeField] Image bubbleImage;
    [SerializeField] TextMeshProUGUI tutorialTextMesh;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && CheckConditionsFulfilled() && other.tag == "Player")
        {
            bubbleImage.enabled = true;

            tutorialTextMesh.text = textToDisplay;
            tutorialTextMesh.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        SetHasBeenTriggered();
        SetFlag();

        bubbleImage.enabled = false;
        tutorialTextMesh.enabled = false;
    }
}
