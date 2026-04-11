using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskList taskList;
    public List<Task> activeTasks = new List<Task>();
    public ShipUI shipUI;
    public AdverseEvent adverseEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipUI = FindAnyObjectByType<ShipUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTask() 
    {
        int selectedTask = Random.Range(0, taskList.tasks.Count);
        activeTasks.Add(taskList.tasks[selectedTask]);
        AdverseEvent obj = Instantiate(adverseEvent, taskList.tasks[selectedTask].location, Quaternion.identity);
        obj.setFields(taskList.tasks[selectedTask].activeTimer, taskList.tasks[selectedTask].fixRate, taskList.tasks[selectedTask].fixTime);
        //shipUI.updateTasksUI();
    }
}
