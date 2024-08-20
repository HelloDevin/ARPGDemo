using UnityEngine;

namespace ZZZ
{
    public class PlayerSkillState : PlayerComboState
    {
        public override void Enter()
        {
            base.Enter();

            _player.movementStateMachine.ChangeState<PlayerMovementNullState>();
            CameraSwitcher.Instance.ActiveStateCamera(_player.characterName, _reusableData.currentSkill.attackStyle);
        }

        public override void Exit()
        {
            CameraSwitcher.Instance.UnActiveStateCamera(_player.characterName, _reusableData.currentSkill.attackStyle);
            base.Exit();
        }

        public override void Update()
        {
            _characterCombo.UpdateAttackLookAtEnemy();
        }

        public override void OnAnimationTranslateEvent<T>()
        {
            _player.comboStateMachine.ChangeState<T>();
        }

        public override void OnAnimationExitEvent()
        {
            _player.comboStateMachine.ChangeState<PlayerNullState>();
        }
    }
}