using System.Collections;
using System.Collections.Generic;
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

    void SpawnIceberg()
    {
        worldScroller.SpawnIceberg();
    }

    public IEnumerator CreateTask()
    {
        while (true) 
        {
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

            yield return new WaitForSeconds(Random.Range(minTimeForNextTask, maxTimeForNextTask));
        }
        
    }

}
