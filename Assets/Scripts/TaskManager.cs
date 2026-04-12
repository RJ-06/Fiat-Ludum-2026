using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskList taskList;
    [SerializeField] float minTimeForNextTask;
    [SerializeField] float maxTimeForNextTask;

    public AnimationCurve minTimeCurve;
    public AnimationCurve maxTimeCurve;
    private int spawnedCount;

    public WorldScroller worldScroller;
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 5f, 5f);
    }

    void Spawn()
    {
        worldScroller.SpawnIceberg();
    }
    public IEnumerator CreateTask() 
    {
        while (true) 
        {
            var selectedTask = taskList.tasks[Random.Range(0,taskList.tasks.Count)];

            AdverseEvent obj = Instantiate(selectedTask.adverseEvent, selectedTask.location, Quaternion.identity);
            obj.setFields(selectedTask.activeTimer, selectedTask.fixRate, selectedTask.fixTime);
            spawnedCount++;
            minTimeForNextTask = minTimeCurve.Evaluate(spawnedCount);
            maxTimeForNextTask = maxTimeCurve.Evaluate(spawnedCount);

            yield return new WaitForSeconds(Random.Range(minTimeForNextTask, maxTimeForNextTask));
        }
        
    }

}
