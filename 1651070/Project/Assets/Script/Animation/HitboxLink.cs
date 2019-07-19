using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxLink : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("zx Norm 1st"))
            animator.gameObject.GetComponent<PlayerControl>().hitBox.GetComponent<Animator>().SetInteger("Attacktype", 1);
        else if (stateInfo.IsName("zx Norm 2nd"))
            animator.gameObject.GetComponent<PlayerControl>().hitBox.GetComponent<Animator>().SetInteger("Attacktype", 2);
        else if (stateInfo.IsName("zx Norm 3rd"))
            animator.gameObject.GetComponent<PlayerControl>().hitBox.GetComponent<Animator>().SetInteger("Attacktype", 3);
        else if (stateInfo.IsName("DashAtk"))
            animator.gameObject.GetComponent<PlayerControl>().hitBox.GetComponent<Animator>().SetInteger("Attacktype", 5);
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("JumpAtk"))
        {
             animator.gameObject.GetComponent<PlayerControl>().hitBox.GetComponent<Animator>().SetInteger("Attacktype", 0);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
