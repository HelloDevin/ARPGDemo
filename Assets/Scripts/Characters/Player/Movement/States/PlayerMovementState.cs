using UnityEngine;
using UnityEngine.InputSystem;

namespace ZZZ
{
    public class PlayerMovementState : IState
    {
        protected Player _player;

        protected Animator _animator;

        protected PlayerMovementData _playerMovementData;

        protected PlayerStateReusableData _reusableData;

        private float _currentVelocity;

        public Vector2 PlayerMovementInput => CharacterInputSystem.Instance.Move;

        public virtual void Init(IStateMachineOwner stateMachineOwner)
        {
            _player = (Player) stateMachineOwner;
            _animator = _player.animator;
            _playerMovementData = _player.playerSO.movementData;
            _reusableData = _player.stateReusableData;
        }

        public virtual void Enter()
        {
            Debug.Log($"Enter->{GetType().Name}");
            AddInputActionCallBacks();
        }

        #region 输入回调

        protected virtual void AddInputActionCallBacks()
        {
            //角色walk委托  
            CharacterInputSystem.Instance.inputActions.Player.Walk.started += OnWalkStart;
            CharacterInputSystem.Instance.inputActions.Player.Run.started += OnDashStart;
            // CharacterInputSystem.Instance.inputActions.Player.SwitchCharacter.started += OnSwitchCharacterStart;
            // CharacterInputSystem.Instance.inputActions.Player.Movement.canceled += OnMovementCanceled;
            // CharacterInputSystem.Instance.inputActions.Player.Movement.performed += OnMovementPerformed;
            //  CharacterInputSystem.Instance.inputActions.Player.CameraLook.started += OnMouseMovementStarted;
        }

        protected virtual void RemoveInputActionCallBacks()
        {
        }

        #endregion

        public void CharacterRotation(Vector2 movementDirection)
        {
            if (PlayerMovementInput == Vector2.zero)
            {
                return;
            }

            //Mathf.Atan2 返回正切值为y/x的弧度角
            _reusableData.targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.y) * Mathf.Rad2Deg
                                        + _player.tfMainCamera.eulerAngles.y;

            _player.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_player.transform.eulerAngles.y,
                _reusableData.targetAngle, ref _currentVelocity, _reusableData.rotationTime);
        }


        protected virtual void OnWalkStart(InputAction.CallbackContext context)
        {
            Debug.Log(_reusableData.shouldWalk);
            //为什么要用bool，确保在idle状态记录walk的切换
            _reusableData.shouldWalk = !_reusableData.shouldWalk;
        }

        private void OnDashStart(InputAction.CallbackContext context)
        {
            // if (_player.comboStateMachine.currentState.Value == movementStateMachine.player.comboStateMachine.SkillState) { return; }
            if (_reusableData.canDash)
            {
                Debug.Log("进入闪避状态");
                if (PlayerMovementInput != Vector2.zero)
                {
                    _animator.CrossFadeInFixedTime(_playerMovementData.dashData.frontDashAnimationName,
                        _playerMovementData.dashData.fadeTime);
                }
                else
                {
                    _animator.CrossFadeInFixedTime(_playerMovementData.dashData.backDashAnimationName,
                        _playerMovementData.dashData.fadeTime);
                }
            }
        }

        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            // UpdateCameraRecenteringState(GetPlayerMovementInputDirection());
        }


        public virtual void Exit()
        {
            RemoveInputActionCallBacks();
        }

        public virtual void Update()
        {
            _animator.SetFloat(AnimatorID.MovementID,
                PlayerMovementInput.sqrMagnitude * _reusableData.inputMult, 0.35f, Time.deltaTime);
            CharacterRotation(PlayerMovementInput);
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnAnimationTranslateEvent<T>() where T : IState, new()
        {
            _player.movementStateMachine.ChangeState<T>();
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void ResetDash()
        {
            _reusableData.canDash = true;
        }
    }
}