using System.Collections.Generic;
using UnityEngine;

namespace ZZZ
{
    public class SFX_PoolManager : SingletonMono<SFX_PoolManager>
    {
        [System.Serializable]
        public class SoundItem
        {
            public SoundStyle soundStyle;
            public string soundName;
            public GameObject soundPrefab;
            public int soundCount;
            public bool ApplyBigCenter;
        }

        [SerializeField] private List<SoundItem> soundPools = new();
        private readonly Dictionary<SoundStyle, Queue<GameObject>> soundCenter = new();

        private readonly Dictionary<string, Dictionary<SoundStyle, Queue<GameObject>>> bigSoundCenter = new();

        protected override void Awake()
        {
            base.Awake();
            InitSoundPool();
        }

        private void InitSoundPool()
        {
            if (soundPools.Count == 0)
            {
                return;
            }

            for (int i = 0; i < soundPools.Count; i++)
            {
                if (soundPools[i].ApplyBigCenter)
                {
                    for (int j = 0; j < soundPools[i].soundCount; j++)
                    {
                        var go = Instantiate(soundPools[i].soundPrefab, transform, true);
                        go.SetActive(false);
                        if (!bigSoundCenter.ContainsKey(soundPools[i].soundName))
                        {
                            Debug.Log(soundPools[i].soundName + "加入对象池");
                            bigSoundCenter.Add(soundPools[i].soundName,
                                new Dictionary<SoundStyle, Queue<GameObject>>());
                        }

                        if (!bigSoundCenter[soundPools[i].soundName].ContainsKey(soundPools[i].soundStyle))
                        {
                            bigSoundCenter[soundPools[i].soundName]
                                .Add(soundPools[i].soundStyle, new Queue<GameObject>());
                        }

                        bigSoundCenter[soundPools[i].soundName][soundPools[i].soundStyle].Enqueue(go);
                    }
                }
                else
                {
                    for (int j = 0; j < soundPools[i].soundCount; j++)
                    {
                        var go = Instantiate(soundPools[i].soundPrefab, transform, true);
                        go.SetActive(false);
                        if (!soundCenter.ContainsKey(soundPools[i].soundStyle))
                        {
                            soundCenter.Add(soundPools[i].soundStyle, new Queue<GameObject>());
                            soundCenter[soundPools[i].soundStyle].Enqueue(go);
                        }
                        else
                        {
                            soundCenter[soundPools[i].soundStyle].Enqueue(go);
                        }
                    }
                }
            }
        }

        public void TryGetSoundPool(SoundStyle soundStyle, string soundName, Vector3 position)
        {
            if (bigSoundCenter.ContainsKey(soundName))
            {
                if (bigSoundCenter[soundName].TryGetValue(soundStyle, out var Q))
                {
                    GameObject go = Q.Dequeue();
                    go.transform.position = position;
                    go.gameObject.SetActive(true);
                    Q.Enqueue(go);
                    Debug.Log("播放音乐" + soundName + "类型是" + soundStyle);
                }
                else
                {
                    Debug.LogWarning("找不到" + soundStyle);
                }
            }
            else
            {
                Debug.LogWarning("找不到" + soundName);
            }
        }

        public void TryGetSoundPool(SoundStyle soundStyle, Vector3 position)
        {
            if (soundCenter.TryGetValue(soundStyle, out var sound))
            {
                GameObject go = sound.Dequeue();
                go.transform.position = position;
                go.gameObject.SetActive(true);
                soundCenter[soundStyle].Enqueue(go);
            }
            else
            {
                Debug.Log(soundStyle + "不存在");
            }
        }
    }
}