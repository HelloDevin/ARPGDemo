using UnityEngine;
using UnityEngine.InputSystem;

namespace ZZZ
{
    public class PlayerComboState : IState
    {
        protected Player _player;

        protected Animator _animator;

        protected PlayerComboReusableData _reusableData;

        protected CharacterCombo _characterCombo;

        public virtual void Init(IStateMachineOwner stateMachineOwner)
        {
            _player = (Player) stateMachineOwner;
            _animator = _player.animator;
            _reusableData = _player.comboReusableDate;
            _characterCombo = _player.characterCombo;
        }

        public virtual void Enter()
        {
            Debug.Log($"Enter->{GetType().Name}");
            AddInputActionCallBacks();
        }

        #region 输入回调

        protected virtual void AddInputActionCallBacks()
        {
            CharacterInputSystem.Instance.inputActions.Player.L_AtK.started += OnAttackInput;
            CharacterInputSystem.Instance.inputActions.Player.FinishSkill.started += OnFinishSkill;
            CharacterInputSystem.Instance.inputActions.Player.Execute.started += OnSkill;
            _characterCombo.AddEventAction();
        }

        protected virtual void RemoveInputActionCallBacks()
        {
            CharacterInputSystem.Instance.inputActions.Player.L_AtK.started -= OnAttackInput;
            CharacterInputSystem.Instance.inputActions.Player.FinishSkill.started -= OnFinishSkill;
            CharacterInputSystem.Instance.inputActions.Player.Execute.started -= OnSkill;
            _characterCombo.RemoveEventActon();
        }

        private void OnSkill(InputAction.CallbackContext context)
        {
            if (_characterCombo.CanSkillInput())
            {
                _characterCombo.SkillInput();
                _player.comboStateMachine.ChangeState<PlayerSkillState>();
            }
        }

        private void OnFinishSkill(InputAction.CallbackContext context)
        {
            if (_characterCombo.CanFinishSkillInput())
            {
                _characterCombo.FinishSkillInput();
                _player.comboStateMachine.ChangeState<PlayerSkillState>();
            }
        }

        #endregion


        protected virtual void OnAttackInput(InputAction.CallbackContext context)
        {
            if (_characterCombo.CanComboInput())
            {
                if (_player.movementStateMachine.IsState<PlayerSprintingState>() || _animator.StateAtTag("Dodge"))
                {
                    Debug.Log("闪避攻击");
                    _characterCombo.DodgeComboInput();
                }
                else
                {
                    _characterCombo.LightComboInput();
                }
            }
        }


        public virtual void Exit()
        {
            RemoveInputActionCallBacks();
        }

        public virtual void Update()
        {
            _characterCombo.UpdateComboAnimation();
            _characterCombo.UpdateEnemy();
        }

        public void FixedUpdate()
        {
        }


        public virtual void OnAnimationTranslateEvent<T>() where T : IState, new()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }
    }
}