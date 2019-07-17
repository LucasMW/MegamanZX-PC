using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAtkWindup : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Dash", false);
        animator.SetBool("DashAtk", false);

        soundManager = SoundManager._instance;
        if (soundManager == null)
        {
            Debug.LogError("No SoundManager found in Scene!!!!!");
        }
        if (animator.GetBool("Debug"))
        {
            timestart = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1);
           /* Debug.Log("After Slash start" + timestart);
            Debug.Log("After Slash length" + animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            GameObject debug = new GameObject();
            debug.name = "After Slash start" + timestart;
            debug.transform.SetParent(animator.gameObject.transform);
            xoffset = animator.GetFloat("XOffset");
            xoffset -= 0.4f;
            debug.transform.localPosition = new Vector3(xoffset, 0, 0);
            animator.SetFloat("XOffset", xoffset);
            debug.transform.localPosition = new Vector3(xoffset, 0, 0);
            debug.transform.localScale = new Vector3(1, 1, 1);
            debug.AddComponent<SpriteRenderer>().sprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
            debug.GetComponent<SpriteRenderer>().sortingLayerName = "Character";*/
            animator.SetBool("RunAtk", false);
            animator.SetBool("Debug", false);
        }
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public float time;
    float xoffset = 0;
    public float timestart;
    private SoundManager soundManager;
    
override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1);
        if (animator.GetBool("Debug"))
        {
            /*GameObject debug = new GameObject();
            debug.name = "After Slash " + time;
            debug.transform.SetParent(animator.gameObject.transform);
            xoffset = animator.GetFloat("XOffset");
            xoffset -= 0.4f;
            debug.transform.localPosition = new Vector3(xoffset, 0, 0);
            animator.SetFloat("XOffset", xoffset);
            debug.transform.localPosition = new Vector3(xoffset, 0, 0);
            debug.transform.localScale = new Vector3(1, 1, 1);
            debug.AddComponent<SpriteRenderer>().sprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
            debug.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
            

            Debug.Break();*/
            animator.SetBool("RunAtk", false);
            animator.SetBool("Debug", false);
        }
        
        
        animator.SetFloat("TimeDebug", animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1));
        
        if (animator.GetBool("RunAtk"))
        {

            soundManager.PlaySound("NormAtk1");
            /*Debug.Log("Run length" + animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            GameObject debug = new GameObject();
            debug.transform.SetParent(animator.gameObject.transform);*/
            time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * Mathf.Repeat(stateInfo.normalizedTime, 1);

            /*debug.name = "Before Windup " + time + " " + animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            
            xoffset = animator.GetFloat("XOffset");
            xoffset -= 0.4f;
            debug.transform.localPosition = new Vector3(xoffset, 0, 0);
            animator.SetFloat("XOffset", xoffset);
            debug.transform.localPosition = new Vector3(xoffset, 0, 0);
            debug.transform.localScale = new Vector3(1, 1, 1);
            debug.AddComponent<SpriteRenderer>().sprite = animator.gameObject.GetComponent<SpriteRenderer>().sprite;
            debug.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
            Debug.Log("Debug1");*/

            time = Mathf.Repeat(time /(37f/22f), animator.GetCurrentAnimatorClipInfo(0)[0].clip.length/ (37f / 22f));
            animator.SetFloat("RunWindupTime", time+0.03f);
            animator.PlayInFixedTime("RunAtkWindup", 0, time);
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
