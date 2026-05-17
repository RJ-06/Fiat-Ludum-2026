using UnityEngine;

public class MinigameText : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GameplayModeManager.Instance.currentMode != GameplayModeManager.Mode.PlayerControl)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
