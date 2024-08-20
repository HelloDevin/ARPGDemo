using UnityEngine;
using UnityEngine.InputSystem;

namespace ZZZ
{
    /// <summary>
    /// 冲刺状态
    /// </summary>
    public class PlayerSprintingState : PlayerMovementState
    {
        private int _timerId;

        float turnDeltaAngle;

        public override void Enter()
        {
            base.Enter();

            _reusableData.rotationTime = _playerMovementData.sprintData.rotationTime;

            _animator.SetBool(AnimatorID.HasInputID, true);

            _reusableData.inputMult = _playerMovementData.sprintData.inputMult;
        }

        public override void Update()
        {
            base.Update();

            turnDeltaAngle = Mathf.DeltaAngle(_player.transform.eulerAngles.y, _reusableData.targetAngle);

            // var targetDir = Quaternion.Euler(0, _reusableDate.targetAngle, 0) * Vector3.forward;
            // turnDeltaAngle = UnityUti.GetDeltaAngle(_player.transform, targetDir);

            
            if (Mathf.Abs(turnDeltaAngle) >= _playerMovementData.turnBackAngle)
            {
                _animator.SetBool(AnimatorID.TurnBackID, true);
            }
        }

        protected override void AddInputActionCallBacks()
        {
            base.AddInputActionCallBacks();

            //监听Sprinting->Idle
            CharacterInputSystem.Instance.inputActions.Player.Movement.canceled += OnBufferToIdle;
        }

        protected override void RemoveInputActionCallBacks()
        {
            base.RemoveInputActionCallBacks();
            CharacterInputSystem.Instance.inputActions.Player.Movement.canceled -= OnBufferToIdle;
            CharacterInputSystem.Instance.inputActions.Player.Movement.started -= RemoveTimer;
        }

        private void OnBufferToIdle(InputAction.CallbackContext context)
        {
            _timerId = TimerManager.Instance.AddTimer(_playerMovementData.bufferToIdleTime, IdleStart);
            //_playerMovementData.bufferToIdleTime时间内按下移动键，取消Sprinting->Idle
            CharacterInputSystem.Instance.inputActions.Player.Movement.started += RemoveTimer;
        }

        private void IdleStart()
        {
            _player.movementStateMachine.ChangeState<PlayerIdlingState>();
        }

        private void RemoveTimer(InputAction.CallbackContext context)
        {
            TimerManager.Instance.RemoveTimer(_timerId);
        }
    }
}