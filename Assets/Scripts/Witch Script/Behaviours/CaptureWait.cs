using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureWait : MonoBehaviour
{
    public Animator animator;
    //private delegate IEnumerator MyFunction();
    //private MyFunction f;

    public void DoCoroutine(float delay)
    {
        StartCoroutine("ChangeState", delay);
    }

    public IEnumerator ChangeState(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Captured Done");
        animator.SetBool("isCapturing", false);
        animator.SetBool("isIdle", true);
        //yield return null;
    }
}


