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

    public FireBehavior firePrefab;
    public bool spawnFire = false;
    int firesSpawned = 0;


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
        if (spawnFire)
        {
            text.text = "Grab buckets (E) and take it to the spigot (back of ship) to fill. (Q) to drop buckets on fires.";
            Invoke(nameof(RemoveText), 15f);
            StartCoroutine(SpawnFires());
        }
    }

    void EndingSequence()
    {
        StartCoroutine(SpawnIcebergs());
        Invoke(nameof(moveToNext), 20f);
        text.text = "Icebergs are approaching! Run to the WHEEL, use (E), then (A/D) to steer around them!";
    }

    void RemoveText()
    {
        text.text = "";
    }

    void SpawnIceberg()
    {
        worldScroller.SpawnIceberg();
    }

    // Refactored to call out to SpawnImmediateTask()
    public IEnumerator CreateTask()
    {
        while (!spawnFire)
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

            if (activeTaskIndices.Count >= tasksList.tasks.Count)
            {
                yield return null;
                continue;
            }

            SpawnImmediateTask();

            yield return new WaitForSeconds(Random.Range(minTimeForNextTask, maxTimeForNextTask));
        }
    }

    // NEW METHOD: Spawns a single random available task immediately. 
    // You can call this from KrakenScript via TaskManager.taskManagerSingleton.SpawnImmediateTask()
    public void SpawnImmediateTask()
    {
        if (activeTaskIndices.Count >= tasksList.tasks.Count)
        {
            // All available tasks are already active
            return;
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
    }

    public IEnumerator SpawnFires()
    {
        while (true)
        {
            if (firesSpawned >= numToSpawn)
            {
                moveToNext();
                yield break;
            }

            if (activeTaskIndices.Count >= tasksList.tasks.Count)
            {
                yield return null;
                continue;
            }

            int index = Random.Range(0, tasksList.tasks.Count);
            while (activeTaskIndices.Contains(index))
            {
                index = Random.Range(0, tasksList.tasks.Count);
            }
            var selectedTask = tasksList.tasks[index];
            activeTaskIndices.Add(index);

            FireBehavior fire = Instantiate(firePrefab, selectedTask.location, Quaternion.identity);
            fire.index = index;
            firesSpawned++;
            yield return new WaitForSeconds(15f);
        }
    }

    public IEnumerator SpawnIcebergs()
    {
        while (true)
        {
            SpawnIceberg();
            yield return new WaitForSeconds(10f);
        }
    }

    void moveToNext()
    {
        ShipManager.shipManager.sceneIndex++;
        SceneTransitionManager.Instance.StartTransition(ShipManager.shipManager.sceneList[ShipManager.shipManager.sceneIndex]);
        StopAllCoroutines();
    }
}