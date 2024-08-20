using UnityEngine;

namespace ZZZ
{
    [System.Serializable]
    public class PlayerRunData
    {
        [field: SerializeField][field: Range(0.1f, 80)] public float inputMult { get; private set; } = 2f;

        [field: SerializeField] public float rotationTime { get; private set; } = 0.08f;
    }
}