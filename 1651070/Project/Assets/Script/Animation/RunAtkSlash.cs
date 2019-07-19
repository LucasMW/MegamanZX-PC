using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAtkSlash : StateMachineBehaviour
{

    public float time;
    public float timestart;
    private bool first;
    public float xoffset = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.gameObject.GetComponent<PlayerControl>().hitBox.GetComponent<Animator>().SetInteger("Attacktype", 6);
        first = true;
        timestart = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1) + 0.03f;
        if (timestart >= 0.30f)
        {
            animator.SetBool("RunAtkLoop", true);
            timestart = Mathf.Repeat(timestart, 0.30f);
        }
        /*GameObject debug = new GameObject();
        
        debug.name = "Windup start" + timestart + " " + animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        debug.transform.SetParent(animator.gameObject.transform);
        debug.transform.position = animator.gameObject.transform.position;
        xoffset = animator.GetFloat("XOffset");
        xoffset -= 0.4f;
        debug.transform.localPosition = new Vector3(xoffset, 0, 0);
        animator.SetFloat("XOffset", xoffset);
        debug.transform.localPosition = new Vector3(xoffset, 0, 0);
        debug.transform.localScale = new Vector3(1, 1, 1);
        debug.AddComponent<SpriteRenderer>().sprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
        debug.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
        animator.SetFloat("TimeDebug2", time);
        Debug.Log("Debug2");*/
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (first)
        {
            //Debug.Log("Windup length" + animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            if (timestart + 0.03f > animator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
            {
                animator.SetBool("RunAtkLoop", true);
            }
            first = false;
        }
        time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1);
        /*GameObject debug = new GameObject();

        debug.name = "Windup " + time + " " + animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        debug.transform.SetParent(animator.gameObject.transform);
        debug.transform.position = animator.gameObject.transform.position;
        xoffset = animator.GetFloat("XOffset");
        xoffset -= 0.4f;
        debug.transform.localPosition = new Vector3(xoffset, 0, 0);
        animator.SetFloat("XOffset", xoffset);
        debug.transform.localPosition = new Vector3(xoffset, 0, 0);
        debug.transform.localScale = new Vector3(1, 1, 1);
        debug.AddComponent<SpriteRenderer>().sprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
        debug.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
        animator.SetFloat("TimeDebug2", time);
        Debug.Log("Debug2");*/
        if (time >= Mathf.Repeat(timestart, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length) && !animator.GetBool("RunAtkLoop"))
        {

            time = Mathf.Repeat(time*2 + 0.06f, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length*2);
            animator.SetFloat("RunSlashTime", time);
            animator.PlayInFixedTime("RunAtkSlash", 0, time);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
       
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
