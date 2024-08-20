using System;
using UnityEngine;

namespace ZZZ
{
    public class CharacterComboBase
    {
        protected Animator _animator { get; }
        protected Transform _selfTransform { get; }

        protected PlayerComboSOData _comboSOData { get; }

        protected PlayerComboReusableData _reusableData { get; }
        protected PlayerEnemyDetectionData _enemyDetectionData { get; }

        public Player player { get; }

        protected CharacterComboBase(Player player)
        {
            _animator = player.animator;
            _selfTransform = player.transform;
            _comboSOData = player.playerSO.comboData.comboSOData;
            _enemyDetectionData = player.playerSO.comboData.enemyDetectionData;
            _reusableData = player.comboReusableDate;
            _reusableData.cameraTransform = player.tfMainCamera;
            this.player = player;

            if (_comboSOData.lightCombo != null)
            {
                _comboSOData.lightCombo.Init();
            }

            if (_comboSOData.heavyCombo != null)
            {
                _comboSOData.heavyCombo.Init();
            }
        }

        public void AddEventAction()
        {
            _reusableData.currentIndex.OnValueChanged += ResetATKIndex;
            EventManager.Instance.RegisterEvent<Transform>(EventName.QTE, CanQETSkill);
        }

        protected virtual void CanQETSkill(Transform trans)
        {
        }

        public void RemoveEventActon()
        {
            _reusableData.currentIndex.OnValueChanged -= ResetATKIndex;
            EventManager.Instance.RemoveEvent<Transform>(EventName.QTE, CanQETSkill);
        }

        public bool CanComboInput()
        {
            if (!_reusableData.canInput) return false;

            if (_animator.StateAtTag("Hit")) return false;
            if (_animator.StateAtTag("Skill")) return false;

            return true;
        }


        #region 一般攻击

        public void LightComboInput()
        {
            if (_comboSOData.lightCombo == null)
            {
                return;
            }

            if (_reusableData.currentCombo != _comboSOData.lightCombo || _reusableData.currentCombo == null)
            {
                _reusableData.currentCombo = _comboSOData.lightCombo;
                ResetComboInfo();
            }

            _reusableData.currentCombo.ResetComboDatas();

            ExecuteCombo();
        }

        public void ResetComboInfo()
        {
            _reusableData.comboIndex = 0;
            _reusableData.canInput = true;
            _reusableData.canLink = true;
            _reusableData.canMoveInterrupt = false;
            _reusableData.canATK = true;
        }


        protected void ExecuteCombo()
        {
            if (_reusableData.currentCombo == null) return;

            _reusableData.hasATKCommand = true;
            _reusableData.canInput = false;
        }

        protected void NormalDodgeCombo()
        {
            if (_comboSOData.lightCombo == null) return;

            if (_reusableData.currentCombo != _comboSOData.lightCombo || _reusableData.currentCombo == null)
            {
                _reusableData.currentCombo = _comboSOData.lightCombo;
            }


            _reusableData.currentCombo.SwitchDodgeATK();
            ResetComboInfo();
            ResetATKIndex(0);
            ExecuteCombo();
        }

        public void UpdateComboAnimation()
        {
            if (!_reusableData.canATK) return;
            if (!_reusableData.hasATKCommand) return;

            _reusableData.currentIndex.Value = _reusableData.comboIndex;

            string comboName = _reusableData.currentCombo.GetComboName(_reusableData.currentIndex.Value);

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _animator.Play(comboName);
            }
            else
            {
                _animator.CrossFade(comboName, 0.111f, 0);
            }

            PlayCharacterVoice(_reusableData.currentCombo.GetComboData(_reusableData.currentIndex.Value));
            StartPlayWeapon();
            UpdateComboInfo();

            _reusableData.hasATKCommand = false;
            _reusableData.canATK = false;
        }

        private void UpdateComboInfo()
        {
            _reusableData.comboIndex++;
            if (_reusableData.comboIndex >= _reusableData.currentCombo.GetComboMaxCount())
            {
                _reusableData.comboIndex = 0;
            }
        }

        #endregion


        #region 更新伤害点

        /// <summary>
        /// 重置伤害点的计数（一个攻击动画可能有多个ATK事件 需要更新ATKIndex）
        /// </summary>
        protected void ResetATKIndex(int index) //大招每一次需要执行手动清零，而普攻是攻击索引值发生变化而清零，还有闪A也要手动清零，因为闪A的连招索引值始终为0
        {
            _reusableData.ATKIndex = 0;
        }

        public void UpdateATKIndex()
        {
            _reusableData.ATKIndex++;
        }

        #endregion


        #region 动画事件

        public void CanInput()
        {
            _reusableData.canInput = true;
        }

        public void CanATK()
        {
            _reusableData.canATK = true;
        }

        public void CanMoveInterrupt()
        {
            _reusableData.canMoveInterrupt = true;
        }


        /// <summary>
        /// ATK这是攻击触发的核心事件，包括了伤害、受击动画、格挡攻击、攻击者、打击感（震屏、顿帧）、受击音效、受击特效
        /// </summary>
        public void ATK()
        {
            AttackTrigger();
        }

        #endregion

        #region 播放音效

        private void StartPlayWeapon()
        {
            PlayWeaponSound(_reusableData.currentCombo.GetComboData(_reusableData.currentIndex.Value));
        }

        /// <summary>
        /// 角色Voice
        /// </summary>
        /// <param name="comboData"></param>
        protected void PlayCharacterVoice(ComboData comboData)
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.ComboVoice, comboData.comboName,
                _selfTransform.position);
        }

        protected void PlayWeaponSound(ComboData comboData)
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.WeaponSound, comboData.comboName,
                _selfTransform.position);
        }

        #endregion

        public void UpdateAttackLookAtEnemy()
        {
            if (GameBlackboard.Instance.GetEnemy() == null) return;

            //每段攻击前30%人物可以转向
            if (_animator.StateAtTag("ATK") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3f ||
                _animator.StateAtTag("Skill"))

            {
                if (UnityUti.DistanceForTarget(_selfTransform, GameBlackboard.Instance.GetEnemy()) > 6.5f) return;
                if (UnityUti.DistanceForTarget(_selfTransform, GameBlackboard.Instance.GetEnemy()) < 0.09f) return;

                _selfTransform.Look(GameBlackboard.Instance.GetEnemy().position, 60);
            }
        }

        public void CheckMoveInterrupt()
        {
            if (!_reusableData.canMoveInterrupt) return;

            if (CharacterInputSystem.Instance.Move.sqrMagnitude != 0)
            {
                _animator.CrossFadeInFixedTime("Locomotion", 0.155f, 0);
                _reusableData.canMoveInterrupt = false;
            }
        }

        #region 传递伤害

        private void AttackTrigger()
        {
            if (_animator.StateAtTag("ATK"))
            {
                UpdateATKIndex();
                // Debug.Log("此时ATK值为：" + _reusableData.ATKIndex);

                CameraHitFeel.Instance.CameraShake(
                    _reusableData.currentCombo.GetComboShakeForce(_reusableData.currentIndex.Value,
                        _reusableData.ATKIndex));
                Debug.Log(_reusableData.currentCombo);

                if (!AttackDetection(_reusableData.currentCombo)) return;
                EventManager.Instance.DispatchEvent(EventName.TakeDamage,
                    _reusableData.currentCombo.GetComboDamage(_reusableData.currentIndex.Value),
                    _reusableData.currentCombo.GetComboHitName(_reusableData.currentIndex.Value),
                    _selfTransform,
                    GameBlackboard.Instance.GetEnemy(),
                    this);

                #region 触发QTE

                // Debug.Log("QTE的条件：" + _reusableData.canQTE);
                // Debug.Log("人物的ATK索引值：" + _reusableData.ATKIndex);
                // Debug.Log("设置的最大ATK数量为：" + _reusableData.currentCombo.GetComboATKCount(_reusableData.currentIndex.Value));


                if (_reusableData.canQTE && _reusableData.ATKIndex >=
                    _reusableData.currentCombo.GetComboATKCount(_reusableData.currentIndex.Value))
                {
                    TriggerQETSkill();
                }

                #endregion
                
                //TODO 顿帧

                Debug.Log("触发伤害");
            }
            else if (_animator.StateAtTag("Skill"))
            {
                UpdateATKIndex();

                if (!SkillDetection(_reusableData.currentSkill)) return;
                EventManager.Instance.DispatchEvent(EventName.TakeDamage,
                    _reusableData.currentSkill.comboDamage,
                    _reusableData.currentSkill.hitName,
                    _selfTransform,
                    GameBlackboard.Instance.GetEnemy(),
                    this);

                #region 触发QTE

                if (_reusableData.canQTE && _reusableData.ATKIndex >= _reusableData.currentSkill.ATKCount)
                {
                    TriggerQETSkill();
                }

                #endregion

                #region 震屏

                if (_reusableData.currentSkill.shakeForce != null &&
                    _reusableData.ATKIndex - 1 < _reusableData.currentSkill.shakeForce.Length)
                {
                    CameraHitFeel.Instance.CameraShake(
                        _reusableData.currentSkill.shakeForce[_reusableData.ATKIndex - 1]);
                }

                #endregion
                
                //TODO 顿帧
            }
        }

        protected virtual void TriggerQETSkill()
        {
        }

        #endregion

        #region 伤害检测

        private bool AttackDetection(ComboContainerData comboContainerData)
        {
            //敌人
            //距离
            //角度
            if (GameBlackboard.Instance.GetEnemy() == null)
            {
                return false;
            }

            // Debug.Log("敌人条件满足");
            if (UnityUti.DistanceForTarget(GameBlackboard.Instance.GetEnemy(), _selfTransform) >
                comboContainerData.GetComboDistance(_reusableData.currentIndex.Value))
            {
                return false;
            }

            // Debug.Log("距离满足");

            if (UnityUti.GetAngleForTargetDirection(GameBlackboard.Instance.GetEnemy(), _selfTransform) > 100)
            {
                return false;
            }

            // Debug.Log("角度条件满足");
            return true;
        }

        private bool SkillDetection(ComboData comboData)
        {
            if (GameBlackboard.Instance.GetEnemy() == null)
            {
                return false;
            }

            if (UnityUti.DistanceForTarget(GameBlackboard.Instance.GetEnemy(), _selfTransform) >
                comboData.attackDistance)
            {
                return false;
            }

            if (UnityUti.GetAngleForTargetDirection(GameBlackboard.Instance.GetEnemy(), _selfTransform) > 45)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}