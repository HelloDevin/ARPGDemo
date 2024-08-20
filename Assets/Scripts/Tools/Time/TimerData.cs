
using System;

namespace TimeModule
{
    public class TimerData
    {
        public int id; // 唯一key
        public float duration;
        public float delay;
        public int repeatTimes;
        public bool complete;

        public float durationTimer;
        public float delayTimer;
        public int repeatTimesCountor;

        public Action<float> onUpdate;
        public Action onComplete;

        /// <summary>
        /// 对象池管理重置方法<see cref="Poolable.Reset"/>
        /// </summary>
        public void Reset()
        {
            duration = 0.0f;
            durationTimer = 0.0f;
            delay = 0.0f;
            delayTimer = 0.0f;
            repeatTimes = 1;
            repeatTimesCountor = 0;

            onUpdate = null;
            onComplete = null;
            complete = false;
            id = 0;
        }

        /// <summary>
        /// 对象池管理销毁方法<see cref="Poolable.Destroy"/>
        /// </summary>
        public void Destroy()
        {
            Reset();
        }
    }
}
