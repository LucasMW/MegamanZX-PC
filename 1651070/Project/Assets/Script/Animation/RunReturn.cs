using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunReturn : StateMachineBehaviour
{
    public float time;
    public float timestart;
    private bool loopback;
    private bool first;
    public float xoffset = 0f;
    public GameObject Zsaber;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        first = true;
        Zsaber = animator.gameObject.GetComponent<Linker>().Zsaber;
        timestart = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1) + 12f/60f;
        //timestart = 0.6825249f;
        if (timestart >= 0.65f)
        {
            animator.SetBool("RunAtkLoop", true);
            timestart = Mathf.Repeat(timestart, 0.65f);
        }
        /*GameObject debug = new GameObject();

        debug.name = "Slash start" + timestart + " " + animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        debug.transform.SetParent(animator.gameObject.transform);
        debug.transform.position = animator.gameObject.transform.position;
        xoffset = animator.GetFloat("XOffset");
        xoffset -= 0.4f;
        debug.transform.localPosition = new Vector3(xoffset, 0, 0);
        animator.SetFloat("XOffset", xoffset);
        debug.transform.localScale = new Vector3(1, 1, 1);
        debug.AddComponent<SpriteRenderer>().sprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
        debug.GetComponent<SpriteRenderer>().sortingLayerName = "Character";

        Debug.Log("Debug3 start:" + time);*/
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (first)
        {
            //Debug.Log("Slash length" + animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            Zsaber.SetActive(true);
            first = false;
        }
        time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1);
        /*GameObject debug = new GameObject();

        debug.name = "Slash " + time + " " + animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        debug.transform.SetParent(animator.gameObject.transform);
        debug.transform.position = animator.gameObject.transform.position;
        xoffset = animator.GetFloat("XOffset");
        xoffset -= 0.4f;
        debug.transform.localPosition = new Vector3(xoffset, 0, 0);
        animator.SetFloat("XOffset", xoffset);
        debug.transform.localScale = new Vector3(1, 1, 1);
        debug.AddComponent<SpriteRenderer>().sprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
        debug.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
        
        Debug.Log("Debug3:" + time);*/
        if (time >=  timestart && !animator.GetBool("RunAtkLoop"))
        {
            time = Mathf.Repeat(time/(44f/37f) , 37f/60f);
            animator.SetBool("RunAtk", false);
            animator.SetFloat("RunReturn", time);
            animator.SetBool("Debug", true);

            animator.ResetTrigger("Attack");
            animator.PlayInFixedTime("Run", 0, time);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    /*override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        /*time = Mathf.Repeat(time + 0.06f, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        animator.SetBool("RunAtk", false);
        animator.SetFloat("RunReturn", time);
        animator.SetBool("Debug", true);

        animator.ResetTrigger("Attack");
        animator.PlayInFixedTime("Run", 0, time);
    }*/

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
