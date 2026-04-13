using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void OnPlay()
    {
         UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene");
    }
}
