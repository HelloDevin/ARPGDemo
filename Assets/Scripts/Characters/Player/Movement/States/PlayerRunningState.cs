using UnityEngine.InputSystem;

namespace ZZZ
{
    public class PlayerRunningState : PlayerMovementState
    {
        private int _timerId;

        public override void Enter()
        {
            base.Enter();

            _animator.CrossFadeInFixedTime("WalkStart", 0.14f);

            _reusableData.rotationTime = _playerMovementData.runData.rotationTime;

            _animator.SetBool(AnimatorID.HasInputID, true);

            _reusableData.inputMult = _playerMovementData.runData.inputMult;
        }

        protected override void AddInputActionCallBacks()
        {
            base.AddInputActionCallBacks();

            //取消Movement按钮 开始变为idle
            CharacterInputSystem.Instance.inputActions.Player.Movement.canceled += OnBufferToIdle;
            CharacterInputSystem.Instance.inputActions.Player.Movement.started += OnKeepRunning;
        }


        private void OnBufferToIdle(InputAction.CallbackContext context)
        {
            _timerId = TimerManager.Instance.AddTimer(_playerMovementData.bufferToIdleTime, IdleStart);
        }

        private void IdleStart()
        {
            _player.movementStateMachine.ChangeState<PlayerIdlingState>();
        }

        private void OnKeepRunning(InputAction.CallbackContext context)
        {
            TimerManager.Instance.RemoveTimer(_timerId);
        }

        protected override void RemoveInputActionCallBacks()
        {
            base.RemoveInputActionCallBacks();
            CharacterInputSystem.Instance.inputActions.Player.Movement.canceled -= OnBufferToIdle;
            CharacterInputSystem.Instance.inputActions.Player.Movement.started -= OnKeepRunning;
        }

        #region 转换Walking

        protected override void OnWalkStart(InputAction.CallbackContext context)
        {
            base.OnWalkStart(context);

            _player.movementStateMachine.ChangeState<PlayerWalkingState>();
        }

        #endregion
    }
}