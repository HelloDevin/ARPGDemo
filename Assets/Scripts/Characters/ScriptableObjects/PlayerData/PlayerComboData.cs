using System;
using UnityEngine;

namespace ZZZ
{
    [Serializable]
    public class PlayerComboData
    {
        [field: SerializeField, Header("招式配置")]
        public PlayerComboSOData comboSOData { get; private set; }

        [field: SerializeField, Header("敌人检测")]
        public PlayerEnemyDetectionData enemyDetectionData { get; private set; }
    }
}