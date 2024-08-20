namespace ZZZ
{
    public class PlayerATKingState : PlayerComboState
    {
        public override void Update()
        {
            base.Update();

            _player.characterCombo.UpdateAttackLookAtEnemy();
            _player.characterCombo.CheckMoveInterrupt();
        }


        public override void OnAnimationExitEvent()
        {
            TimerManager.Instance.AddTimer(0.2f, ToNullState);
        }

        private void ToNullState()
        {
            if (_animator.StateAtTag("ATK") || _animator.StateAtTag("Skill"))
            {
                return;
            }

            _player.comboStateMachine.ChangeState<PlayerNullState>();
        }

        public override void OnAnimationTranslateEvent<T>()
        {
            _player.comboStateMachine.ChangeState<T>();
        }
    }
}