namespace ZZZ
{
    public class PlayerNullState : PlayerComboState
    {
        public override void Enter()
        {
            base.Enter();
            
            _characterCombo.ResetComboInfo();
        }

        public override void OnAnimationTranslateEvent<T>()
        {
            _player.comboStateMachine.ChangeState<T>();
        }
    }
}