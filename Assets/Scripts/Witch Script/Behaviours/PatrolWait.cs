using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolWait : MonoBehaviour
{
    public void DoCoroutine(PatrolBehaviour pb)
    {
        StartCoroutine("ChangeState", pb);
    }

    public IEnumerator ChangeState(PatrolBehaviour pb)
    {
        pb.isPausing = true;
        Debug.Log("Pausing");
        yield return new WaitForSeconds(pb.waitTime);
        Debug.Log("Finished Pausing");
        pb.isPausing = false;
    }
}


