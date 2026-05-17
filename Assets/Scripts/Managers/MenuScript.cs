using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void OnPlay()
    {
        Debug.Log("clicked!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene");
        
    }
}
