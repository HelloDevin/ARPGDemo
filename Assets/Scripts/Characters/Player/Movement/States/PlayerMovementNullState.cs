using UnityEngine;

namespace ZZZ
{
    public class PlayerMovementNullState : PlayerMovementState
    {
        public override void Enter()
        {
            base.Enter();
            _reusableData.rotationTime = _playerMovementData.comboRotationTime;
        }

        public override void Update()
        {
            if (_animator.StateAtTag("ATK"))
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <
                    _playerMovementData.comboRotationPercentage)
                {
                    base.Update();
                }
            }
        }

        public override void OnAnimationExitEvent()
        {
            TimerManager.Instance.AddTimer(0.2f, CheckStateExit);
        }

        private void CheckStateExit()
        {
            if (_animator.StateAtTag("ATK") || _animator.StateAtTag("Skill"))
            {
                return;
            }

            if (PlayerMovementInput == Vector2.zero)
            {
                _player.movementStateMachine.ChangeState<PlayerIdlingState>();
            }
            else
            {
                _player.movementStateMachine.ChangeState<PlayerRunningState>();
            }
        }
    }
}