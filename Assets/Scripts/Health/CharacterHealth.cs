namespace ZZZ
{
    public class CharacterHealth : CharacterHealthBase
    {
        protected override void CharacterHitAction(float damage, string hitName)
        {
            base.CharacterHitAction(damage, hitName);
            _healthInfo.TakeDamage(damage);
            _animator.CrossFadeInFixedTime(hitName, 0.1f, 0);
            _healthInfo.TakeDefenseValue(damage);
        }

        protected override void OnUpdateDefenseValue(float value)
        {
            base.OnUpdateDefenseValue(value);

            if (_currentEnemy == null) return;

            if (value <= 0)
            {
                EventManager.Instance.DispatchEvent(EventName.QTE, _currentEnemy);
                _healthInfo.ResetDefenseValue();
            }
        }

        protected override void SetHitSFX(CharacterNameList characterNameList)
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.Hit, characterNameList.ToString(), transform.position);
        }
    }
}