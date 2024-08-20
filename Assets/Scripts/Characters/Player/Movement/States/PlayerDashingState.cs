using UnityEngine;

namespace ZZZ
{
    public class PlayerDashingState : PlayerMovementState
    {
        public override void Enter()
        {
            base.Enter();

            _reusableData.rotationTime = _playerMovementData.dashData.rotationTime;

            _reusableData.canDash = false;

            TimerManager.Instance.AddTimer(_playerMovementData.dashData.coldTime, ResetDash);
            
            _player.PlayDodgeSound();
        }

        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();

            if (PlayerMovementInput == Vector2.zero)
            {
                _player.movementStateMachine.ChangeState<PlayerIdlingState>();
                return;
            }
            
            _player.movementStateMachine.ChangeState<PlayerSprintingState>();
        }
    }
}