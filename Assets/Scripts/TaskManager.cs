using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] float minTimeForNextTask;
    [SerializeField] float maxTimeForNextTask;

    public AnimationCurve minTimeCurve;
    public AnimationCurve maxTimeCurve;
    private int spawnedCount;

    public WorldScroller worldScroller;

    public TaskList tasksList;

    public List<Task> activeTasks;
    public HashSet<int> activeTaskIndices = new HashSet<int>();

    public static TaskManager taskManagerSingleton { get; private set; }

    public TMPro.TMP_Text text;
    private int spawnedTasks = 0;

    public int numToSpawn;

    bool initiatedEnding = false;



    private void Awake()
    {
        if (taskManagerSingleton != null && taskManagerSingleton != this)
        {
            Destroy(gameObject);
            return;
        }
        taskManagerSingleton = this;
    }

    void Start()
    {
        StartCoroutine(CreateTask());
    }

    void EndingSequence()
    {
        StartCoroutine(SpawnIcebergs());
        Invoke(nameof(moveToNext), 20f);
        text.text = "Icebergs are approaching! Run to the WHEEL, use (E), then (A/D) to steer around them!";
    }

    void SpawnIceberg()
    {
        worldScroller.SpawnIceberg();
    }

    public IEnumerator CreateTask()
    {
        while (true) 
        {
            if (spawnedTasks >= numToSpawn)
            {
                Debug.Log("Finished spawning tasks.");
                if (!initiatedEnding)
                {
                    initiatedEnding = true;
                    EndingSequence();
                }
                yield break;
            }
            int index = Random.Range(0, tasksList.tasks.Count);
            while (activeTaskIndices.Contains(index))
            {
                index = Random.Range(0, tasksList.tasks.Count);
            }
            var selectedTask = tasksList.tasks[index];
            activeTaskIndices.Add(index);

            AdverseEvent obj = Instantiate(selectedTask.adverseEvent, selectedTask.location, Quaternion.identity);
            obj.setFields(selectedTask.activeTimer, selectedTask.fixRate, selectedTask.fixTime);
            obj.thisTask = selectedTask;
            spawnedCount++;
            minTimeForNextTask = minTimeCurve.Evaluate(spawnedCount);
            maxTimeForNextTask = maxTimeCurve.Evaluate(spawnedCount);
            activeTasks.Add(selectedTask);

            spawnedTasks++;

            yield return new WaitForSeconds(Random.Range(minTimeForNextTask, maxTimeForNextTask));
        }
    }

    public IEnumerator SpawnIcebergs()
    {
        while (true)
        {
            SpawnIceberg();
            yield return new WaitForSeconds(5f);
        }
    }

    void moveToNext()
    {
        ShipManager.shipManager.sceneIndex++;
        string nextScene = ShipManager.shipManager.sceneList[ShipManager.shipManager.sceneIndex];
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    }
}
