﻿using System;
using UnityEngine;

namespace ZZZ
{
    [Serializable]
    public class PlayerWalkData
    {
        [field: SerializeField][field: Range(0.1f, 80)] public float inputMult { get; private set; } = 1f;

        [field: SerializeField] public float rotationTime { get; private set; } = 0.08f;
    }
}