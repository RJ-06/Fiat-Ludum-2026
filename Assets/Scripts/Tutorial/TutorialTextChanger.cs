using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class TutorialTextChanger : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private bool hasReachedAdverseEvent = false;

    public GameObject minigameObject1;
    public GameObject minigameObject2;
    public GameObject minigameText1;
    public GameObject minigameText2;

    private bool sceneSwitchScheduled = false;

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
                    tutorialText.text = "Great job! Head to the YELLOW AREAS on the minimap. These MINIGAMES are always active, and can earn you buffs and prizes.";
                    tutorialText.fontSize = 18;
                    minigameObject1.gameObject.SetActive(true);
                    minigameObject2.gameObject.SetActive(true);
                    minigameText1.gameObject.SetActive(true);
                    minigameText2.gameObject.SetActive(true);
                }
            }
        }

        if (TutorialTaskManager.Instance.spawnedTutorialTasks == 3 && (collider.gameObject.CompareTag("Kitchen") || collider.gameObject.CompareTag("Fishing")))
        {
            if (collider.gameObject.CompareTag("Kitchen"))
            {
                tutorialText.text = "This is the cooking minigame to gain food! (SPACE) to cut at the dotted lines; (Q) to quit.";
            }
            else if (collider.gameObject.CompareTag("Fishing"))
            {
                tutorialText.text = "This is the fishing minigame to gain loot!. (E) to hook a fish when the icon appears.";
            }

            if (!sceneSwitchScheduled)
            {
                Invoke(nameof(BeginSwitchingScenes), 25f);
            }
            sceneSwitchScheduled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TutorialTaskManager.Instance.spawnedTutorialTasks == 3 && (other.gameObject.CompareTag("Kitchen") || other.gameObject.CompareTag("Fishing")))
        {
            tutorialText.text = "";
        }
    }

    void BeginSwitchingScenes()
    {
        if (GameplayModeManager.Instance.currentMode == GameplayModeManager.Mode.Cooking)
        {
            FindAnyObjectByType<SliceMinigameController>().ExitMinigame();
        }

        GameplayModeManager.Instance.currentMode = GameplayModeManager.Mode.PlayerControl;
        ShipManager.shipManager.sceneIndex++;
        SceneTransitionManager.Instance.StartTransition(ShipManager.shipManager.sceneList[ShipManager.shipManager.sceneIndex]);
        tutorialText.text = "It's time to start spending some money! Head to the shop and buy some items to help you out!";
        tutorialText.fontSize = 18;
    }
}
