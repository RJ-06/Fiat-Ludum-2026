using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskList taskList;
    public List<Task> activeTasks = new List<Task>();
    public ShipUI shipUI;

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
        shipUI.updateTasksUI();
    }
}
