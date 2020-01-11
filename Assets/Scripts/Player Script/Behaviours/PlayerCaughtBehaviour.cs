using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCaughtBehaviour : StateMachineBehaviour
{
    PlayerLogic playerLogic;
    PlayerController playerController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerLogic = animator.transform.GetComponent<PlayerLogic>();
        playerController = animator.transform.GetComponent<PlayerController>();
        //playerLogic.gotCaught();
        //playerController.levitate();
        playerController.setIsLevitating(true);
    }

}
