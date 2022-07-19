using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public bool isIdle;
    public Animator anim;
    public float interactionTime;

    private IEnumerator taskRoutine;
    private Coroutine taskCoroutine; 
    // Start is called before the first frame update
    public void StartInteraction()
    {
        taskRoutine = TaskRoutine(interactionTime);
        taskCoroutine = StartCoroutine(taskRoutine);
        

    }

    public void CancelTask()
    {
        if (taskRoutine.MoveNext())
        {
            StopCoroutine(taskCoroutine);
        }
    }

    IEnumerator TaskRoutine(float time)
    {
        print("Task Started");

        print("Task Ended");
        yield return null; 
    }
}
