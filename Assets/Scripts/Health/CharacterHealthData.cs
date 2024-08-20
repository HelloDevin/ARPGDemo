using UnityEngine;

namespace ZZZ
{
    [CreateAssetMenu(fileName = "CharacterHealthData", menuName = "Create/Asset/CharacterHealthData")]
    public class CharacterHealthData : ScriptableObject
    {
        //field必须在 { get;  set; }访问器的作用下才能对属性序列化
        [field: SerializeField] public float maxHP { get; private set; }

        [field: SerializeField] public float maxDefenseValue { get; private set; }
    }
}