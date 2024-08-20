using UnityEngine;

namespace ZZZ
{
    public class PlayerStateReusableData
    {
        public float inputMult;

        public bool shouldWalk;

        /// <summary>
        /// 是否可以Dash
        /// </summary>
        public bool canDash = true;

        public float poseThreshold;

        public Vector2 inputDirection;

        public float rotationTime;

        public float targetAngle;

        //如果本类只是new一次，那么当我获取这个成员的时候，我实际上是获取到了这个成员的引用，当你希望多次new这个类，但还是修改原来的成员，可以用ref+属性封装字段，从而返回这个类型的引用
    }
}