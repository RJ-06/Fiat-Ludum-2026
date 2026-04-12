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

    public static TaskManager taskManagerSingleton;
    public TaskList tasksList;

    public List<Task> activeTasks;

    void Start()
    {
        taskManagerSingleton = this;
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
            var selectedTask = tasksList.tasks[Random.Range(0,tasksList.tasks.Count)];
            Debug.Log("Spawning task: " + selectedTask.name);

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
