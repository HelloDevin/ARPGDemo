using UnityEngine;

namespace ZZZ
{
    public enum OnEnterAnimationPlayerState
    {
        Null,
        Dash,
        DashBack,
        TurnBack,
        ATK
    }

    public class OnAnimationTranslation : StateMachineBehaviour
    {
        [SerializeField] public OnEnterAnimationPlayerState onEnterAnimationState;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onEnterAnimationState == OnEnterAnimationPlayerState.Null)
            {
                return;
            }

            if (animator.TryGetComponent<Player>(out var player))
            {
                player.OnAnimationTranslateEvent(onEnterAnimationState);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (animator.TryGetComponent<Player>(out var player))
            {
                player.OnAnimationExitEvent();
            }
        }
    }
}