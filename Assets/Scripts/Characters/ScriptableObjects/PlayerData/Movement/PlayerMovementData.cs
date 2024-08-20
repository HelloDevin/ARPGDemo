﻿using System;
using UnityEngine;

namespace ZZZ
{
    [Serializable]
    public class PlayerMovementData
    {
        [field: SerializeField] public float bufferToIdleTime { get; private set; } = 0.15f;
        [field: SerializeField] public PlayerIdleData idleData { get; private set; }
        [field: SerializeField] public PlayerWalkData walkData { get; private set; }
        [field: SerializeField] public PlayerRunData runData { get; private set; }
        [field: SerializeField] public PlayerDashData dashData { get; private set; }

        [field: SerializeField] public PlayerSprintData sprintData { get; private set; }
        
        [field: SerializeField] public PlayerReturnRunData returnRunData { get; private set; }

        /// <summary>
        /// 转身跑阈值
        /// </summary>
        [field: SerializeField]
        public float turnBackAngle { get; private set; } = 135f;
        
        [field: SerializeField] public float comboRotationPercentage { get; private set; }
        
        [field: SerializeField] public float comboRotationTime { get; private set; }
    }
}