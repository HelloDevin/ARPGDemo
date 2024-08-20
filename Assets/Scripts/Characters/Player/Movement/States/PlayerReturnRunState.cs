using UnityEngine;

namespace ZZZ
{
    public class PlayerReturnRunState : PlayerMovementState
    {
        public override void Enter()
        {
            base.Enter();

            Debug.Log($"PlayerReturnRunState Enter=>{_player.transform.eulerAngles.y}");
            _animator.SetBool(AnimatorID.TurnBackID, false);
            _animator.SetBool(AnimatorID.HasInputID, true);

            _reusableData.inputMult = _playerMovementData.returnRunData.inputMult;
            _reusableData.rotationTime = _playerMovementData.returnRunData.rotationTime;
        }

        public override void Update()
        {
            if (_animator.StateAtTag("TurnRun") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.08f)
            {
                return;
            }
            
            CharacterRotation(PlayerMovementInput);
        }

        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(AnimatorID.TurnBackID, false);
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();
            Debug.Log(
                $"PlayerReturnRunState OnAnimationExitEvent=>{_player.transform.eulerAngles.y} {Vector3.up * _reusableData.targetAngle}");
            if (PlayerMovementInput == Vector2.zero)
            {
                _player.movementStateMachine.ChangeState<PlayerIdlingState>();
                return;
            }

            _player.movementStateMachine.ChangeState<PlayerSprintingState>();
        }
    }
}