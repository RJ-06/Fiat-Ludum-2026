using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskList", menuName = "Scriptable Objects/TaskList")]
public class TaskList : ScriptableObject
{
    public List<Task> tasks = new List<Task>();

    public Task? GetTaskByID(int taskID) 
    {
        if (taskID < 0 || taskID >= tasks.Count) return null;

        return tasks[taskID];

    }
    
}

[Serializable]
public struct Task
{
    public string name;
    public Vector3 location;
    public float fixTime;
    public float fixRate;
    public float activeTimer;
    public AdverseEvent adverseEvent;
    public Vector2 UICoordinates;
}
