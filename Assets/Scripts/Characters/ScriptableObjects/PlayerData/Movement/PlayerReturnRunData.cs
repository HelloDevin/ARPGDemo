using System;
using UnityEngine;

namespace ZZZ
{
    [Serializable]
    public class PlayerReturnRunData
    {
        [field: SerializeField]
        [field: Range(0.1f, 80f)]
        public float inputMult { get; private set; } = 2.3f;

        [field: SerializeField] public float rotationTime { get; private set; } = 0.5f;
    }
}