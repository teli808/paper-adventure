using UnityEngine;
using UnityEngine.UI;

public class DialogueBubbleExtension : MonoBehaviour
{
    [SerializeField] Transform currentSpeakerTransform;
    [SerializeField] Vector3 offset = new Vector3(46f, 0f, 0f); //20f for battle

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        DisableExtension();
    }

    public void DisplayExtension()
    {
        Vector3 pos = cam.WorldToScreenPoint(currentSpeakerTransform.position);

        Vector3 finalPosition;
        Vector3 newRotation;

        if (OnRightSideOfScreen(pos))
        {
            finalPosition = pos + offset;

            newRotation = new Vector3(0f, 0f, 151f);
        }
        else
        {
            finalPosition = pos + -offset;

            newRotation = new Vector3(0f, 180f, 151f);
        }

        if (finalPosition == transform.position) return;

        transform.position = new Vector3(finalPosition.x, transform.position.y, transform.position.z);
        transform.eulerAngles = newRotation;

        EnableImage();

    }

    private bool OnRightSideOfScreen(Vector3 screenpoint)
    {
        return screenpoint.x > Screen.width / 2;
    }

    public void ChangeSpeaker(Transform transform)
    {
        currentSpeakerTransform = transform;
    }

    public void EnableImage()
    {
        GetComponent<Image>().enabled = true;
    }

    public void DisableExtension()
    {
        GetComponent<Image>().enabled = false;
    }
}
