using UnityEngine;

public class ShowMinigameTutorial : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tutorialText;
    public TMPro.TextMeshProUGUI conflictText;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Kitchen") || other.gameObject.CompareTag("Fishing"))
        {
            if (conflictText.text == "")
            {
                if (other.gameObject.CompareTag("Kitchen"))
                {
                    tutorialText.text = "This is the cooking minigame to gain food! (SPACE) to cut at the dotted lines; (Q) to quit.";
                }
                else if (other.gameObject.CompareTag("Fishing"))
                {
                    tutorialText.text = "This is the fishing minigame to gain loot!. (E) to hook a fish when the icon appears.";
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Kitchen") || other.gameObject.CompareTag("Fishing"))
        {
            tutorialText.text = "";
        }
    }
}
