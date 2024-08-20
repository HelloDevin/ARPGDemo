using UnityEngine;

namespace ZZZ
{
    public class CharacterMoveBase : MonoBehaviour
    {
        public Animator animator { get; private set; }
        [HideInInspector] public CharacterController characterController;
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }

        protected virtual void OnAnimatorMove()
        {
            animator.ApplyBuiltinRootMotion();
        }

    }
}