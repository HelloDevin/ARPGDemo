using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZZZ
{
    public interface IState
    {
        public void Init(IStateMachineOwner stateMachineOwner);

        public void Enter();

        public void Exit();

        public void Update();

        public void FixedUpdate();

        public void OnAnimationTranslateEvent<T>() where T : IState, new();

        public void OnAnimationExitEvent();
    }
}