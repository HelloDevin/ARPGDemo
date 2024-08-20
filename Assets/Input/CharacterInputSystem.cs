using UnityEngine;

namespace ZZZ
{
    public class CharacterInputSystem : SingletonMono<CharacterInputSystem>
    {
        public CharacterInput inputActions;

        protected override void Awake()
        {
            if (inputActions == null)
            {
                inputActions = new CharacterInput();
            }
        }
        
        private void OnEnable()
        {
            inputActions?.Enable();
        }
        private void OnDisable()
        {
            inputActions?.Disable();
        }

        public Vector2 Move => inputActions.Player.Movement.ReadValue<Vector2>();
        public bool Run => inputActions.Player.Run.triggered;
    }
}