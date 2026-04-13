using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private Camera mainCamera;
    [SerializeField] private Transform boat;
    [SerializeField] private GameObject tradingShipPrefab;
    [SerializeField] private GameObject island;
    [SerializeField] TMPro.TMP_Text transitionText;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    public void StartTransition(string nextScene)
    {
        mainCamera.GetComponent<CameraScript>().enabled = false;
        StartCoroutine(TransitionRoutine(nextScene));
    }

    private IEnumerator TransitionRoutine(string nextScene)
    {
        if (nextScene == "TradingScene")
        {
            // 1. Spawn trading ship near boat
            Vector3 spawnPos = boat.position;
            GameObject ship = Instantiate(tradingShipPrefab, spawnPos + new Vector3(0, 0, 20f), Quaternion.Euler(new Vector3(270, 0, 0)));

            // 2. Camera pan to ship
            Vector3 startPos = mainCamera.transform.position;
            Quaternion startRot = mainCamera.transform.rotation;

            Vector3 targetPos = spawnPos + new Vector3(20f, 10f, 0f);
            Quaternion targetRot = Quaternion.Euler(0f, -90f, 0f);

            float t = 0f;
            float duration = 5f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float p = t / duration;

                ship.transform.position -= Vector3.forward * 5f * Time.deltaTime;

                mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, p);
                mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, p);

                yield return null;
            }
        } else
        {
            // 1. Spawn island in distance
            Vector3 spawnPos = boat.position + new Vector3(0, -20f, -300f);
            GameObject land = Instantiate(island, spawnPos, Quaternion.identity);

            Vector3 startPos = mainCamera.transform.position;
            Quaternion startRot = mainCamera.transform.rotation;

            Vector3 targetPos = startPos + new Vector3(0, 10f, 0f);
            


            float t = 0f;
            float duration = 5f;

            transitionText.text = "Land Ahoy! It's time to raid the island for supplies. Prepare the crew!";

            while (t < duration)
            {
                t += Time.deltaTime;
                float p = t / duration;

                land.transform.position += Vector3.forward * Time.deltaTime * 20f;

                Vector3 lookTarget = land.transform.position + new Vector3(0, 20f, 0);
                Quaternion targetRot = Quaternion.LookRotation(lookTarget - mainCamera.transform.position);

                mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, p);
                mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, p);

                yield return null;
            }
        }

        // 3. Hold briefly
        yield return new WaitForSeconds(1.5f);
        mainCamera.GetComponent<CameraScript>().enabled = true;

        // 4. Load next scene
        SceneManager.LoadScene(nextScene);
    }
}