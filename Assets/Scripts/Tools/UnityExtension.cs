using UnityEngine;

namespace ZZZ
{
    public static class UnityExtension
    {
        /// <summary>
        /// 检查当前动画片段是否是指定Tag
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="tag"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static bool StateAtTag(this Animator origin, string tag, int layerIndex = 0)
        {
            return origin.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tag);
        }
        
        /// <summary>
        /// 看着目标方向以Y轴为中心
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="target"></param>
        /// <param name="timer">平滑时间(如果是单击某个按键触发那么值最好设置100以上。)</param>
        public static void Look(this Transform transform, Vector3 target,float timer)
        {
            var direction = (target - transform.position).normalized;
            direction.y = 0f;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,UnityUti.UnTetheredLerp(timer));
        }
    }
}