using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private Camera mainCamera;

    [SerializeField] private Transform boat;
    [SerializeField] private GameObject tradingShipPrefab;
    [SerializeField] private GameObject island;
    [SerializeField] private TMP_Text transitionText;

    private bool isTransitioning;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    public void StartTransition(string nextScene)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        if (mainCamera != null)
        {
            var camScript = mainCamera.GetComponent<CameraScript>();
            if (camScript != null)
                camScript.enabled = false;
        }

        StartCoroutine(TransitionRoutine(nextScene));
    }

    private IEnumerator TransitionRoutine(string nextScene)
    {
        if (nextScene == "TradingScene")
        {
            Vector3 spawnPos = boat.position;

            GameObject ship = Instantiate(
                tradingShipPrefab,
                spawnPos + new Vector3(0, 0, 20f),
                Quaternion.Euler(270f, 0f, 0f)
            );

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

                if (ship != null)
                    ship.transform.position -= Vector3.forward * 5f * Time.deltaTime;

                if (mainCamera != null)
                {
                    mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, p);
                    mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, p);
                }

                yield return null;
            }
        }
        else
        {
            Vector3 spawnPos = boat.position + new Vector3(0, -20f, -300f);
            GameObject land = Instantiate(island, spawnPos, Quaternion.identity);

            Vector3 startPos = mainCamera.transform.position;
            Quaternion startRot = mainCamera.transform.rotation;
            Vector3 targetPos = startPos + new Vector3(0, 10f, 0f);

            if (transitionText != null)
                transitionText.text = "Land Ahoy! It's time to raid the island for supplies. Prepare the crew!";

            float t = 0f;
            float duration = 5f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float p = t / duration;

                if (land != null)
                    land.transform.position += Vector3.forward * Time.deltaTime * 20f;

                if (mainCamera != null)
                {
                    Vector3 lookTarget = land != null
                        ? land.transform.position + new Vector3(0, 20f, 0)
                        : mainCamera.transform.position + mainCamera.transform.forward;

                    Vector3 direction = lookTarget - mainCamera.transform.position;

                    mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, p);

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(direction);
                        mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, p);
                    }
                }

                yield return null;
            }
        }

        yield return new WaitForSeconds(1.5f);

        StopAllCoroutines(); // prevent coroutine running into scene load
        SceneManager.LoadScene(nextScene);
    }
}