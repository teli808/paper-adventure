using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueBubble;
    [SerializeField] TextMeshProUGUI dialogueText;

    [SerializeField] DialogueBubbleExtension dialogueBubbleExtension;

    [SerializeField] DialogueChoicesBox choicesBox;

    [Header("Dialogue Preferences")]
    [SerializeField] float typingSpeed = 0.01f;

    InkVariableObserver inkVariableObserver;

    Story currentStory;

    CanvasGroup canvasGroup;

    public bool DialogueIsPlaying { get; private set; } = false;
    bool canContinueToNextLine = true;
    bool dialogueSubmitted = false;

    public bool StoryContainsDialogueChoiceVar { get; private set; } = false;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
        inkVariableObserver = GetComponentInChildren<InkVariableObserver>();
    }

    private void OnEnable()
    {
        EventManager.Instance.OnDialogueSubmitted += DialogueSubmitted;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDialogueSubmitted -= DialogueSubmitted;
    }

    private void Start()
    {
        //dialogueBubble.SetActive(false);
        ShowDialogueBubbleImages(false);
    }

    private void Update()
    {
        //returns right away if dialogue isn't playing
        if (!DialogueIsPlaying) return;

        //handle continuing to the next line in the dialogue when submit is pressed
        if (dialogueSubmitted && canContinueToNextLine)
        {
            dialogueSubmitted = false;
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, string optionalKnot = null)
    {
        SetUpStory(inkJSON);

        if (optionalKnot != null)
        {
            currentStory.ChoosePathString(optionalKnot);
        }

        EventManager.Instance.DialogueStarted();
        ContinueStory();
    }

    public void EnterDialogueForItemEvent(TextAsset inkJSON, string itemName)
    {
        SetUpStory(inkJSON);

        currentStory.variablesState["itemName"] = itemName;

        EventManager.Instance.DialogueStarted();
        ContinueStory();

    }

    private void SetUpStory(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        DialogueIsPlaying = true;
        dialogueSubmitted = false;
        canContinueToNextLine = true;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "BattleScene") //add functionality for battlescene
            {
                SetDialogueChoiceVar();
            }

            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayDialogue()
    {
        string line = currentStory.Continue();
        canContinueToNextLine = false;
        dialogueText.text = "";

        yield return ManageTags();

        foreach (char letter in line.ToCharArray())
        {
            if (dialogueSubmitted)
            {
                dialogueText.text = line;
                dialogueSubmitted = false;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (SceneManager.GetActiveScene().name != "BattleScene") //add functionality for battlescene
        {
            yield return DisplayChoices();
        }
        
        canContinueToNextLine = true;
    }

    private IEnumerator ManageTags()
    {
        //tag will always be a character's name in ink file
        //Player is #Player

        List<string> currentTags = currentStory.currentTags;

        if (currentTags.Count == 0) //No dialogue bubble extension should be shown
        {
            //dialogueBubble.SetActive(true);
            ShowDialogueBubbleImages(true);
            //dialogueBubbleExtension.DisableExtension();
            yield break;
        }

        string nameOfSpeaker = currentTags[0];
        GameObject speakerObject = GameObject.Find(nameOfSpeaker);

        if (speakerObject == null)
        {
            Debug.Log("Cannot find object by name: " + nameOfSpeaker); //Tag in the file must match with an object in scene
        }
        else
        {
            //dialogueBubble.SetActive(false);
            ShowDialogueBubbleImages(false);
            dialogueBubbleExtension.DisableExtension();
            dialogueBubbleExtension.ChangeSpeaker(speakerObject.transform);
            yield return new WaitForSeconds(0.25f);

            //dialogueBubble.SetActive(true);
            ShowDialogueBubbleImages(true);
            dialogueBubbleExtension.DisplayExtension();
        }

        yield return null;
    }

    private IEnumerator DisplayChoices()
    {
        if (currentStory.currentChoices.Count > 0)
        {
            choicesBox.DisplayChoices(currentStory.currentChoices);

            yield return new WaitForSeconds(0.25f);

            dialogueSubmitted = false;

            yield return new WaitUntil(() => dialogueSubmitted);

            choicesBox.DisableChoicesBox();

            int choiceChosenIndex = choicesBox.GetChoiceChosen();
            currentStory.ChooseChoiceIndex(choiceChosenIndex);
        }
    }

    private void ExitDialogueMode()
    {
        ShowDialogueBubbleImages(false);
        dialogueBubbleExtension.DisableExtension();

        dialogueText.text = "";
        DialogueIsPlaying = false;

        EventManager.Instance.DialogueOver();

        print("Exiting dialogue mode");
    }

    private void DialogueSubmitted()
    {
        dialogueSubmitted = true;
    }

    private void SetDialogueChoiceVar()
    {
        if (inkVariableObserver.ContainsDialogueChoiceVar(currentStory))
        {
            StoryContainsDialogueChoiceVar = true;


        }
        else
        {
            StoryContainsDialogueChoiceVar = false;
        }
    }

    public bool SearchDialogueChoiceVar()
    {
        return inkVariableObserver.SearchDialogueChoiceVar(currentStory);
    }

    private void ShowDialogueBubbleImages(bool shouldEnable)
    {
        Behaviour[] bubbleComponents = dialogueBubble.GetComponentsInChildren<Behaviour>();

        foreach (Behaviour bubbleComponent in bubbleComponents)
        {
            bubbleComponent.enabled = shouldEnable;
        }

    }

}
