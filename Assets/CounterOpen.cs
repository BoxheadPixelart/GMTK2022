using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterOpen : MonoBehaviour
{
    public Animator anim;
    public string var; 
    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool(var, true);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool(var, false);
    }

}
