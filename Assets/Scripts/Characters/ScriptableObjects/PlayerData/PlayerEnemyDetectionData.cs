﻿using UnityEngine;

namespace ZZZ
{
    [System.Serializable]
    public class PlayerEnemyDetectionData
    {
        [field: SerializeField, Header("敌人检测")]
        public float detectionRadius { get; private set; }

        [field: SerializeField] public float detectionLength { get; private set; }

        [field: SerializeField] public LayerMask WhatIsEnemy { get; private set; }
    }
}