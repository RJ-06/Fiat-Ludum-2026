using UnityEngine;
using TMPro;

public class TutorialTextChanger : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private bool hasReachedAdverseEvent = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<AdverseEvent>())
        {
            if (!hasReachedAdverseEvent)
            {
                tutorialText.text = "PRESS and HOLD (E) to fix. Fix issues before they start causing damage to the ship!";
                hasReachedAdverseEvent = true;
            }

            if (TutorialTaskManager.Instance != null)
            {
                if (TutorialTaskManager.Instance.spawnedTutorialTasks == 3) {
                    tutorialText.text = "Great job! Once you finish, head to the YELLOW MARKED AREAS on the minimap.";
                }
            }
        }
    }


}
