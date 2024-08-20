using UnityEngine;

namespace ZZZ
{
    public static class UnityUti
    {
        /// <summary>
        /// 获取增量角
        /// </summary>
        /// <param name="curTransform">当前移动方向</param>
        /// <param name="targetDirection">目标移动方向</param>
        /// <returns></returns>
        public static float GetDeltaAngle(Transform curTransform, Vector3 targetDirection)
        {
            //当前角色朝向的角度
            //不完全等同于欧拉角的y，因为单纯的欧拉角在斜坡并不是我们想要的
            float angleCurrent = Mathf.Atan2(curTransform.forward.x, curTransform.forward.z) * Mathf.Rad2Deg;
            //目标方向的角度也就是希望角色转过去的那个方向的角度
            float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

            return Mathf.DeltaAngle(angleCurrent, targetAngle);
        }
        
        /// <summary>
        /// 计算当前朝向于目标方向之间的夹角
        /// </summary>
        /// <param name="target"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float GetAngleForTargetDirection(Transform target, Transform self)
        {
            return Vector3.Angle((target.position - self.position).normalized, self.forward);
        }

        /// <summary>
        /// 返回于目标之间的距离
        /// </summary>
        /// <param name="target"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float DistanceForTarget(Transform target, Transform self)
        {
            return Vector3.Distance(self.position, target.position);
        }

        /// <summary>
        /// 不受帧数影响的Lerp
        /// </summary>
        /// <param name="time">平滑时间(尽量设置为大于10的值)</param>
        public static float UnTetheredLerp(float time = 10f)
        {
            return 1 - Mathf.Exp(-time * Time.deltaTime);
        }
    }
}