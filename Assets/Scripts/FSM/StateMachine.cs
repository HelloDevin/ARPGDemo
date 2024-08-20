using System;
using System.Collections.Generic;

namespace ZZZ
{
    public class StateMachine
    {
        protected IStateMachineOwner _stateMachineOwner;

        //这是一个继承了BindableProperty类型的IState字段，获取IState要通过.Value
        public BindableProperty<IState> currentState = new();

        private readonly Dictionary<Type, IState> _dictStates = new();

        public StateMachine(IStateMachineOwner stateMachineOwner)
        {
            _stateMachineOwner = stateMachineOwner;
        }

        /// <summary>
        /// 切换状态的接口API
        /// </summary>
        public void ChangeState<T>() where T : IState, new()
        {
            //可能为空用？逻辑符
            currentState.Value?.Exit();

            currentState.Value = LoadState<T>();

            currentState.Value.Enter();
        }

        private IState LoadState<T>() where T : IState, new()
        {
            if (!_dictStates.TryGetValue(typeof(T), out var state))
            {
                state = new T();
                state.Init(_stateMachineOwner);
                _dictStates.Add(typeof(T), state);
            }


            return state;
        }


        /// <summary>
        /// 更新非物理逻辑的接口API
        /// </summary>
        public void Update()
        {
            currentState.Value?.Update();
        }

        public void FixedUpdate()
        {
            currentState.Value?.FixedUpdate();
        }

        /// <summary>
        /// 执行动画事件的接口API
        /// </summary>
        public void OnAnimationTranslateEvent<T>() where T : IState, new()
        {
            currentState.Value?.OnAnimationTranslateEvent<T>();
        }

        public void OnAnimationExitEvent()
        {
            currentState.Value?.OnAnimationExitEvent();
        }

        public bool IsState<T>()
        {
            return currentState.Value.GetType() == typeof(T);
        }
    }
}