using UnityEngine;
using UnityEngine.Animations;

namespace Code.Runtime.AnimationState
{
    public class ResetPlayerAction : StateMachineBehaviour
    {
        public string targetBool;
        public bool status;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            animator.SetBool(targetBool, status);
        }
    }
}