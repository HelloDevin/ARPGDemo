using UnityEngine;

namespace ZZZ
{
    [CreateAssetMenu(fileName = "CharacterHealthInfo", menuName = "Create/Asset/CharacterHealthInfo")]
    public class CharacterHealthInfo : ScriptableObject
    {
        [SerializeField] private CharacterHealthData healthData;

        public BindableProperty<float> currentHP = new BindableProperty<float>();
        public BindableProperty<bool> onDead = new BindableProperty<bool>();
        public BindableProperty<float> currentDefenseValue = new BindableProperty<float>();

        public void InitHealthData()
        {
            currentHP.Value = healthData.maxHP;
            currentDefenseValue.Value = healthData.maxDefenseValue;
            Debug.Log("敌人初始化的血量为" + currentHP.Value);
        }

        public void TakeDamage(float Damage)
        {
            currentHP.Value = TakeHealthValue(currentHP.Value, -1f * Damage, healthData.maxHP);
        }

        public void TakeDefenseValue(float Damage)
        {
            currentDefenseValue.Value =
                TakeHealthValue(currentDefenseValue.Value, -1f * Damage, healthData.maxDefenseValue);
        }

        private float TakeHealthValue(float currentValue, float offsetValue, float maxValue)
        {
            return Mathf.Clamp(currentValue + offsetValue, 0, maxValue);
        }

        /// <summary>
        /// 回满防御值
        /// </summary>
        public void ResetDefenseValue()
        {
            currentDefenseValue.Value = healthData.maxDefenseValue;
        }
    }
}