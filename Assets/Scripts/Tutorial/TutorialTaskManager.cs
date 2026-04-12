using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialTaskManager : MonoBehaviour
{
    [SerializeField] float minTimeForNextTask;
    [SerializeField] float maxTimeForNextTask;

    public AnimationCurve minTimeCurve;
    public AnimationCurve maxTimeCurve;
    private int spawnedCount;


    public TaskList tasksList;

    public List<Task> activeTasks;
    public HashSet<int> activeTaskIndices = new HashSet<int>();

    public int spawnedTutorialTasks = 0;

    public static TutorialTaskManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(CreateTask());
    }

    public IEnumerator CreateTask()
    {
        while (true)
        {
            if (spawnedTutorialTasks >= 3)
            {
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

            spawnedTutorialTasks++;
            
            yield return new WaitForSeconds(Random.Range(minTimeForNextTask, maxTimeForNextTask));
        }

    }

}
