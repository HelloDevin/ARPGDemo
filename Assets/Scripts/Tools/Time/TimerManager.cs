using System;
using System.Collections.Generic;
using TimeModule;
using UnityEngine;

namespace ZZZ
{
    public class TimerManager : SingletonMono<TimerManager>
    {
        private int _timerID = 1;
        private readonly List<TimerData> _activeTimers = new();
        private readonly Dictionary<int, TimerData> _activeTimerDict = new();
        private readonly List<TimerData> _removeTimers = new();
        private readonly Queue<TimerData> _timerDataQueue = new();

        private readonly OneListener _tickListener = new();

        public event Action TICK
        {
            add => _tickListener.Add(value);
            remove => _tickListener.Remove(value);
        }

        private void Update()
        {
            Tick();
        }

        public int AddTimer(float duration, Action onComplete)
        {
            TimerData timerData = SpawnTimerData();
            timerData.id = _timerID++;
            timerData.duration = duration;
            timerData.onComplete = onComplete;
            timerData.complete = false;
            _activeTimers.Add(timerData);
            _activeTimerDict.Add(timerData.id, timerData);

            return timerData.id;
        }

        public int AddTimer(float duration, Action<float> onUpdate, Action onComplete)
        {
            return AddTimer(duration, 0.0f, 1, onUpdate, onComplete);
        }

        public int AddTimer(float duration, float delay, Action<float> onUpdate, Action onComplete)
        {
            return AddTimer(duration, delay, 1, onUpdate, onComplete);
        }

        public int AddTimer(float duration, int repeatTimes, Action<float> onUpdate, Action onComplete)
        {
            return AddTimer(duration, 0.0f, repeatTimes, onUpdate, onComplete);
        }

        public int AddTimer(float duration, float delay, int repeatTimes, Action<float> onUpdate, Action onComplete)
        {
            TimerData timerData = SpawnTimerData();
            timerData.id = _timerID++;
            timerData.duration = duration;
            timerData.delay = delay;
            timerData.repeatTimes = repeatTimes;
            timerData.onUpdate = onUpdate;
            timerData.onComplete = onComplete;
            timerData.complete = false;
            _activeTimers.Add(timerData);
            _activeTimerDict.Add(timerData.id, timerData);

            return timerData.id;
        }

        public void Release()
        {
            _activeTimers.Clear();
            _removeTimers.Clear();
            _activeTimerDict.Clear();
        }

        public void RemoveTimer(int id)
        {
            if (_activeTimerDict.TryGetValue(id, out var timerData) && !_removeTimers.Contains(timerData))
            {
                _removeTimers.Add(timerData);
            }
        }

        public void SetTimerCompleteAtOnce(int id)
        {
            if (_activeTimerDict.TryGetValue(id, out var timerData))
            {
                timerData.complete = true;
                timerData.onComplete?.Invoke();
            }
        }

        public void Tick()
        {
            _tickListener.Invoke();

            foreach (TimerData timerData in _removeTimers)
            {
                _activeTimers.Remove(timerData);
                _activeTimerDict.Remove(timerData.id);
                RecycleTimerData(timerData);
            }

            _removeTimers.Clear();

            for (int i = 0, count = _activeTimers.Count; i < count; i++)
            {
                TimerData timerData = _activeTimers[i];
                if (timerData.complete)
                {
                    // 说明被立即完成了，不能重复触发
                    RemoveTimer(timerData.id);
                    continue;
                }

                if (timerData.delayTimer >= timerData.delay)
                {
                    if (timerData.durationTimer >= timerData.duration)
                    {
                        timerData.onComplete?.Invoke();

                        if (timerData.repeatTimes == -1) // repeatTimes值为-1时无限循环
                        {
                            timerData.durationTimer = 0.0f;
                        }
                        else
                        {
                            timerData.repeatTimesCountor++;
                            if (timerData.repeatTimesCountor >= timerData.repeatTimes)
                            {
                                timerData.complete = true;
                                RemoveTimer(timerData.id);
                            }
                            else
                            {
                                timerData.durationTimer = 0.0f;
                            }
                        }
                    }
                    else
                    {
                        timerData.durationTimer += Time.deltaTime;
                        timerData.onUpdate?.Invoke(timerData.durationTimer);
                    }
                }
                else
                {
                    timerData.delayTimer += Time.deltaTime;
                }
            }
        }

        private TimerData SpawnTimerData()
        {
            if (_timerDataQueue.TryPeek(out TimerData peek))
            {
                peek = _timerDataQueue.Dequeue();
            }
            else
            {
                peek = new TimerData();
            }

            return peek;
        }

        private void RecycleTimerData(TimerData timerData)
        {
            if (timerData == null)
            {
                return;
            }

            if (_timerDataQueue.Contains(timerData))
            {
                Debug.LogError("同一个对象被回收了多次，请检查");
            }
            else
            {
                timerData.Reset();
                _timerDataQueue.Enqueue(timerData);
            }

            return;
        }
    }
}