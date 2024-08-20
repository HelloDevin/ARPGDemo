using System;
using UnityEngine;

namespace ZZZ
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    public class Player : CharacterMoveBase, IStateMachineOwner
    {
        public CharacterNameList characterName;

        public PlayerSO playerSO;

        [Header("Player状态")] public string movementStateName;

        //显示当前ComboState
        public string comboStateName;

        public Transform enemy;

        public PlayerStateReusableData stateReusableData;

        public PlayerComboReusableData comboReusableDate;

        public CharacterCombo characterCombo;

        public StateMachine movementStateMachine { get; private set; }
        public StateMachine comboStateMachine { get; private set; }

        public Transform tfMainCamera { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            tfMainCamera = Camera.main.transform;
            stateReusableData = new PlayerStateReusableData();
            comboReusableDate = new PlayerComboReusableData();
            characterCombo = new CharacterCombo(this);
            movementStateMachine = new StateMachine(this);
            comboStateMachine = new StateMachine(this);
        }

        private void OnEnable()
        {
            movementStateMachine.currentState.OnValueChanged += OnMovementStateChanged;
            comboStateMachine.currentState.OnValueChanged += OnComboStateChanged;
            GameBlackboard.Instance.enemy.OnValueChanged += OnEnemyChanged;
        }

        private void OnEnemyChanged(Transform t)
        {
            enemy = t;
        }

        public void OnDisable()
        {
            movementStateMachine.currentState.OnValueChanged -= OnMovementStateChanged;
            comboStateMachine.currentState.OnValueChanged -= OnComboStateChanged;
            GameBlackboard.Instance.enemy.OnValueChanged -= OnEnemyChanged;
        }


        private void OnMovementStateChanged(IState state)
        {
            movementStateName = state.GetType().Name;
        }

        private void OnComboStateChanged(IState state)
        {
            comboStateName = state.GetType().Name;
        }

        protected override void Start()
        {
            base.Start();
            movementStateMachine.ChangeState<PlayerIdlingState>();
            comboStateMachine.ChangeState<PlayerNullState>();
        }

        protected override void Update()
        {
            base.Update();

            movementStateMachine.Update();
            comboStateMachine.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            movementStateMachine.FixedUpdate();
            comboStateMachine.FixedUpdate();
        }

        #region 动画事件音效

        public void PlayVFX(string value)
        {
            VFX_PoolManager.Instance.TryGetVFX(characterName, value);
        }

        public void PlayFootSound()
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.Foot, transform.position);
        }

        public void PlayFootBackSound()
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.FootBack, transform.position);
        }

        public void PlayWeaponBackSound()
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.WeaponBack, characterName.ToString(),
                transform.position);
        }

        public void PlayWeaponEndSound()
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.WeaponEnd, characterName.ToString(),
                transform.position);
        }

        #endregion

        #region 相关动画进入或退出触发的方法

        public void OnAnimationTranslateEvent(OnEnterAnimationPlayerState playerState)
        {
            switch (playerState)
            {
                case OnEnterAnimationPlayerState.TurnBack:
                    movementStateMachine.OnAnimationTranslateEvent<PlayerReturnRunState>();
                    break;
                case OnEnterAnimationPlayerState.Dash:
                    movementStateMachine.OnAnimationTranslateEvent<PlayerDashingState>();
                    comboStateMachine.OnAnimationTranslateEvent<PlayerNullState>();
                    break;
                case OnEnterAnimationPlayerState.DashBack:
                    movementStateMachine.OnAnimationTranslateEvent<PlayerDashingState>();
                    comboStateMachine.OnAnimationTranslateEvent<PlayerNullState>();
                    break;
                case OnEnterAnimationPlayerState.ATK:
                    movementStateMachine.OnAnimationTranslateEvent<PlayerMovementNullState>();
                    comboStateMachine.OnAnimationTranslateEvent<PlayerATKingState>();
                    break;
            }
        }

        public void OnAnimationExitEvent()
        {
            movementStateMachine.OnAnimationExitEvent();

            comboStateMachine.OnAnimationExitEvent();
        }

        #endregion

        public void PlayDodgeSound()
        {
            SFX_PoolManager.Instance.TryGetSoundPool(SoundStyle.DodgeSound, transform.position);
        }

        #region 连招动画事件

        public void EnablePreInput()
        {
            characterCombo.CanInput();
        }

        public void CancelAttackColdTime()
        {
            characterCombo.CanATK();
        }

        public void DisableLinkCombo()
        {
        }

        public void EnableMoveInterrupt()
        {
            characterCombo.CanMoveInterrupt();
        }

        public void ATK()
        {
            characterCombo.ATK();
        }
        
        #endregion
    }
}