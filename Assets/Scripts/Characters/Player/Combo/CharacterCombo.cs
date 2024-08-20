using UnityEngine;

namespace ZZZ
{
    public class CharacterCombo : CharacterComboBase
    {
        public CharacterCombo(Player player) : base(player)
        {
        }

        #region 闪A处理

        public void DodgeComboInput()
        {
            NormalDodgeCombo();
        }

        #endregion


        #region 技能处理

        public bool CanFinishSkillInput()
        {
            if (_animator.StateAtTag("Skill")) return false;
            if (_animator.StateAtTag("ATK")) return false;

            if (_comboSOData.finishSkillCombo == null) return false;

            return true;
        }

        public bool CanSkillInput()
        {
            if (_animator.StateAtTag("Skill")) return false;
            if (_animator.StateAtTag("ATK")) return false;

            if (_comboSOData.skillCombo == null) return false;

            return true;
        }

        public void FinishSkillInput()
        {
            if (_comboSOData.finishSkillCombo == null) return;

            if (_reusableData.currentSkill == null || _reusableData.currentSkill != _comboSOData.finishSkillCombo)
            {
                _reusableData.currentSkill = _comboSOData.finishSkillCombo;
            }

            ExecuteSkill();
        }

        public void SkillInput()
        {
            if (_comboSOData.skillCombo == null) return;

            if (_reusableData.currentSkill == null || _reusableData.currentSkill != _comboSOData.skillCombo)
            {
                _reusableData.currentSkill = _comboSOData.skillCombo;
            }

            ExecuteSkill();
        }


        /// <summary>
        /// 释放技能
        /// </summary>
        private void ExecuteSkill()
        {
            ResetATKIndex(0);
            //播放语音
            PlayCharacterVoice(_reusableData.currentSkill);
            //播放武器音效
            PlayWeaponSound(_reusableData.currentSkill);
            _animator.CrossFadeInFixedTime(_reusableData.currentSkill.comboName, 0.1f);
        }

        #endregion

        #region 敌人检测

        private void UpdateDetectionDir()
        {
            Vector3 camForwardDir = Vector3.zero;

            var v3Forward = _reusableData.cameraTransform.forward;
            camForwardDir.Set(v3Forward.x, 0, v3Forward.z);

            camForwardDir.Normalize();

            _reusableData.detectionDir = camForwardDir * CharacterInputSystem.Instance.Move.y +
                                         _reusableData.cameraTransform.right * CharacterInputSystem.Instance.Move.x;
            _reusableData.detectionDir.Normalize();
        }

        public void UpdateEnemy()
        {
            UpdateDetectionDir();

            var position = _selfTransform.position;

            _reusableData.detectionOrigin = new Vector3(position.x, position.y + 0.7f, position.z);

            if (Physics.SphereCast(_reusableData.detectionOrigin, _enemyDetectionData.detectionRadius,
                    _reusableData.detectionDir, out var hit, _enemyDetectionData.detectionLength,
                    _enemyDetectionData.WhatIsEnemy))
            {
                if (GameBlackboard.Instance.GetEnemy() != hit.collider.transform ||
                    GameBlackboard.Instance.GetEnemy() == null)
                {
                    GameBlackboard.Instance.SetEnemy(hit.collider.transform);
                }
            }
        }

        #endregion

        protected override void TriggerQETSkill()
        {
            //禁用人物所有输入
            CharacterInputSystem.Instance.inputActions.Player.Disable();

            ResetComboInfo();

            _reusableData.canQTE = false;

            StartQTESkill();
        }

        private void StartQTESkill()
        {
            //改技能
            _reusableData.currentSkill = _comboSOData.switchSkill;

            //播放切人动画
            _animator.Play(_reusableData.currentSkill.comboName);

            _animator.Update(0f);

            player.comboStateMachine.ChangeState<PlayerSkillState>();

            //播放语音
            PlayCharacterVoice(_reusableData.currentSkill);
            //播放武器音效
            PlayWeaponSound(_reusableData.currentSkill);

            TimerManager.Instance.AddTimer(2, EndQTESkill);
        }

        private void EndQTESkill()
        {
            //恢复输入
            CharacterInputSystem.Instance.inputActions.Player.Enable();
        }

        protected override void CanQETSkill(Transform trans)
        {
            if (_selfTransform != trans) return;
            _reusableData.canQTE = true;
        }
    }
}