using UnityEngine;

namespace ZZZ
{
    public class CharacterHealthBase : MonoBehaviour
    {
        [SerializeField] private float _currentHP;
        [SerializeField] private float _currentDefenseValue;
        [SerializeField] protected CharacterHealthInfo _characterHealthInfo;
        protected Transform _currentEnemy;
        protected CharacterHealthInfo _healthInfo;
        protected Animator _animator;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _healthInfo = Instantiate(_characterHealthInfo);
        }

        protected virtual void Start()
        {
            _healthInfo.InitHealthData();
        }

        protected virtual void Update()
        {
            LookAtAttacker();
        }


        private void OnEnable()
        {
            EventManager.Instance.RegisterEvent<float, string, Transform, Transform, CharacterComboBase>(
                EventName.TakeDamage, OnCharacterHitEventHandler);
            _healthInfo.currentHP.OnValueChanged += OnUpdateHP;
            _healthInfo.currentDefenseValue.OnValueChanged += OnUpdateDefenseValue;
        }

        private void OnUpdateHP(float value)
        {
            Debug.Log($"敌人的血量：{value}");
            _currentHP = value;
            if (value > 0)
            {
                _healthInfo.onDead.Value = false;
                //TODO 刷新血条UI
                return;
            }

            _healthInfo.onDead.Value = true;
            Debug.Log("敌人死亡");
        }

        protected virtual void OnUpdateDefenseValue(float value)
        {
            _currentDefenseValue = value;
        }


        private void OnDisable()
        {
            EventManager.Instance.RemoveEvent<float, string, Transform, Transform, CharacterComboBase>(
                EventName.TakeDamage, OnCharacterHitEventHandler);
            _healthInfo.currentHP.OnValueChanged -= OnUpdateHP;
        }

        private void OnCharacterHitEventHandler(float damage, string hitName, Transform attacker,
            Transform bearer, CharacterComboBase characterCombo)
        {
            if (bearer != transform)
            {
                return;
            }

            SetEnemy(attacker);
            CharacterHitAction(damage, hitName);
            OnCharacterDamageAction(damage);
            SetHitVFX(attacker, bearer);
            SetHitSFX(characterCombo.player.characterName);
        }

        protected void OnCharacterDamageAction(float damage)
        {
        }

        protected virtual void CharacterHitAction(float damage, string hitName)
        {
        }

        protected virtual void SetEnemy(Transform attacker)
        {
            if (_currentEnemy != attacker || _currentEnemy == null)
            {
                _currentEnemy = attacker;
            }
        }

        public void SetHitVFX(Transform attacker, Transform hitter)
        {
            Vector3 hitDir = (attacker.position - hitter.position).normalized;

            Vector3 targetPos = hitter.position + hitDir * 0.8f + Vector3.up * 1f;

            VFX_PoolManager.Instance.GetVFX(CharacterNameList.Enemy, "Hit", targetPos);
        }

        protected virtual void SetHitSFX(CharacterNameList characterNameList)
        {
        }

        public void LookAtAttacker()
        {
            if (_currentEnemy == null) return;

            if (_animator.StateAtTag("Hit") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3f)
            {
                transform.Look(_currentEnemy.position, 50);
            }
        }
    }
}