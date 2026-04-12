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

        if (TutorialTaskManager.Instance.spawnedTutorialTasks == 3 && collider.gameObject.CompareTag("Kitchen"))
        {
            tutorialText.text = "";
            Invoke(nameof(BeginSwitchingScenes), 15f);
        }
    }

    void BeginSwitchingScenes()
    {
        GameplayModeManager.Instance.currentMode = GameplayModeManager.Mode.PlayerControl;
        tutorialText.text = "It's time to start spending some money! Head to the shop and buy some items to help you out!";
        tutorialText.fontSize = 18;
        Invoke(nameof(SwitchScenes), 5f);
    }

    void SwitchScenes()
    {
        ShipManager.shipManager.sceneIndex++;
        SceneManager.LoadScene(ShipManager.shipManager.sceneList[ShipManager.shipManager.sceneIndex]);
    }
}
