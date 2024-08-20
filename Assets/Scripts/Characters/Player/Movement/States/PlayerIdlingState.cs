using UnityEngine;
using UnityEngine.InputSystem;

namespace ZZZ
{
    public class PlayerIdlingState : PlayerMovementState
    {
        private int _timerId;

        public override void Enter()
        {
            base.Enter();
            _reusableData.rotationTime = _playerMovementData.idleData.rotationTime;
            _animator.SetBool(AnimatorID.HasInputID, false);
            _reusableData.inputMult = _playerMovementData.idleData.inputMult;
        }

        protected override void AddInputActionCallBacks()
        {
            base.AddInputActionCallBacks();
            CharacterInputSystem.Instance.inputActions.Player.Movement.started += OnBufferToRun;
        }

        private void OnBufferToRun(InputAction.CallbackContext context)
        {
            _timerId = TimerManager.Instance.AddTimer(0.11f, CheckMoveInput);
        }

        private void CheckMoveInput()
        {
            //视为轻击角色没有Walk或者Run而是Run_Start_End
            if (CharacterInputSystem.Instance.Move == Vector2.zero)
            {
                //按照实际时间进行混合
                _animator.CrossFadeInFixedTime("Run_Start_End", 0.13f);
            }
            else
            {
                Move();
            }
        }

        private void Move()
        {
            if (_player.stateReusableData.shouldWalk)
            {
                //切换到Walk状态
                _player.movementStateMachine.ChangeState<PlayerWalkingState>();
                return;
            }

            //否则执行Run移动
            _player.movementStateMachine.ChangeState<PlayerRunningState>();
        }

        protected override void RemoveInputActionCallBacks()
        {
            base.RemoveInputActionCallBacks();
            CharacterInputSystem.Instance.inputActions.Player.Movement.started -= OnBufferToRun;
            TimerManager.Instance.RemoveTimer(_timerId);
        }


        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}