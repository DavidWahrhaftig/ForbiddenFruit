using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditSceneEnd : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.FindObjectOfType<SceneChanger>().goToScene(0, false);    
    }

}
