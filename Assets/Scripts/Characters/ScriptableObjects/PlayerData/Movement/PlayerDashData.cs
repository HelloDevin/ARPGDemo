using System;
using UnityEngine;

namespace ZZZ
{
    [Serializable]
    public class PlayerDashData
    {
        [field: SerializeField] public string frontDashAnimationName { get; private set; } = "Dodge_Front";

        [field: SerializeField] public string backDashAnimationName { get; private set; } = "Dodge_Back";

        /// <summary>
        /// 动画播放时间
        /// </summary>
        [field: SerializeField]
        public float fadeTime { get; private set; } = 0.1555f;

        /// <summary>
        /// 旋转时间
        /// </summary>
        [field: SerializeField]
        public float rotationTime { get; private set; } = 0.09f;

        [field: SerializeField] public bool dodgeBackApplyRotation { get; private set; } = false;


        [field: SerializeField] public float coldTime { get; private set; } = 0.5f;
    }
}