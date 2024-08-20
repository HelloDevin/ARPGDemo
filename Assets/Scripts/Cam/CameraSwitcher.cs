using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace ZZZ
{
    public class CameraSwitcher : SingletonMono<CameraSwitcher>
    {
        private CinemachineBrain _brain;

        [Serializable]
        public class CharacterStateCameraInfo
        {
            public CharacterNameList characterName;
            public List<StateCameraInfo> stateCameraList = new List<StateCameraInfo>();
        }

        [Serializable]
        public class StateCameraInfo
        {
            public AttackStyle AttackStyle;
            public CinemachineStateDrivenCamera stateCamera;
        }

        [SerializeField, Header("技能相机组")]
        private List<CharacterStateCameraInfo> _stateCameraInfoList = new List<CharacterStateCameraInfo>();

        private Dictionary<CharacterNameList, Dictionary<AttackStyle, CinemachineStateDrivenCamera>> _stateCameraPool =
            new Dictionary<CharacterNameList, Dictionary<AttackStyle, CinemachineStateDrivenCamera>>();
        //这里有两种方式可以实现：字典里面再写一个字典；字典里面写自定义的数据结构；第二种更灵活,但是没有字典省性能

        protected override void Awake()
        {
            base.Awake();
            _brain = Camera.main.GetComponent<CinemachineBrain>();
        }

        private void Start()
        {
            InitSkillCamera();
        }

        private void InitSkillCamera()
        {
            if (_stateCameraInfoList.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _stateCameraInfoList.Count; i++)
            {
                if (_stateCameraInfoList[i].stateCameraList.Count == 0)
                {
                    continue;
                } //跳过当前元素 

                _stateCameraPool.Add(_stateCameraInfoList[i].characterName,
                    new Dictionary<AttackStyle, CinemachineStateDrivenCamera>());
                foreach (StateCameraInfo stateCameraInfo in _stateCameraInfoList[i].stateCameraList)
                {
                    stateCameraInfo.stateCamera.gameObject.SetActive(false);
                    stateCameraInfo.stateCamera.Priority = 0;
                    //加入到字典里面
                    _stateCameraPool[_stateCameraInfoList[i].characterName]
                        .Add(stateCameraInfo.AttackStyle, stateCameraInfo.stateCamera);
                }
            }
        }

        public void ActiveStateCamera(CharacterNameList characterName, AttackStyle attackStyle)
        {
            if (_stateCameraPool.TryGetValue(characterName, out var stateCameraList))
            {
                //然后在列表里面查找符合要求的元素类
                if (stateCameraList.TryGetValue(attackStyle, out var stateDrivenCamera))
                {
                    stateDrivenCamera.gameObject.SetActive(true);
                    stateDrivenCamera.Priority = 20;
                }
            }
        }

        public void UnActiveStateCamera(CharacterNameList characterName, AttackStyle attackStyle)
        {
            if (_stateCameraPool.TryGetValue(characterName, out var stateCameraList))
            {
                //然后在列表里面查找符合要求的元素类
                if (stateCameraList.TryGetValue(attackStyle, out var stateDrivenCamera))
                {
                    stateDrivenCamera.Priority = 0;
                    stateDrivenCamera.gameObject.SetActive(false);
                }
            }
        }
    }
}