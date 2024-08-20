using UnityEngine;
using UnityEngine.InputSystem;

namespace ZZZ
{
    public class PlayerWalkingState : PlayerMovementState
    {
        private int _timerId;
        public override void Enter()
        {
            base.Enter();
            
            _animator.CrossFadeInFixedTime("WalkStart", 0.14f);
            
            _reusableData.rotationTime = _playerMovementData.walkData.rotationTime;

            _animator.SetBool(AnimatorID.HasInputID, true);
            
            _reusableData.inputMult = _playerMovementData.walkData.inputMult;
        }

        protected override void AddInputActionCallBacks()
        {
            base.AddInputActionCallBacks();

            CharacterInputSystem.Instance.inputActions.Player.Movement.canceled += OnBufferToIdle;
        }

        protected override void RemoveInputActionCallBacks()
        {
            base.RemoveInputActionCallBacks();
            CharacterInputSystem.Instance.inputActions.Player.Movement.canceled -= OnBufferToIdle;
        }

        private void OnBufferToIdle(InputAction.CallbackContext context)
        {
            _timerId = TimerManager.Instance.AddTimer(_playerMovementData.bufferToIdleTime, IdleStart);
            CharacterInputSystem.Instance.inputActions.Player.Movement.started += RemoveTimer;
        }

        private void RemoveTimer(InputAction.CallbackContext obj)
        {
            TimerManager.Instance.RemoveTimer(_timerId);
        }

        private void IdleStart()
        {
            CharacterInputSystem.Instance.inputActions.Player.Movement.started -= RemoveTimer;
            _player.movementStateMachine.ChangeState<PlayerIdlingState>();
        }

        protected override void OnWalkStart(InputAction.CallbackContext context)
        {
            base.OnWalkStart(context);
            
            _player.movementStateMachine.ChangeState<PlayerRunningState>();
        }
    }
}