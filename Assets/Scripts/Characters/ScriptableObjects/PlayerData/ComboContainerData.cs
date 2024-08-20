using System.Collections.Generic;
using UnityEngine;

namespace ZZZ
{
    [CreateAssetMenu(fileName = "ComboContainerData", menuName = "Create/Asset/CoomboContainerData")]
    public class ComboContainerData : ScriptableObject
    {
        [SerializeField] public List<ComboData> comboDatas = new List<ComboData>();
        [SerializeField, Header("闪A")] public ComboData dodgeATKData;

        private ComboData firstComboData;

        public void Init()
        {
            if (comboDatas.Count == 0)
            {
                return;
            }

            //缓存第一个连招
            firstComboData = comboDatas[0];
            Debug.Log("初始化");
        }

        public ComboData GetComboData(int index)
        {
            if (comboDatas.Count <= index) return null;

            return comboDatas[index];
        }

        public string GetComboName(int index)
        {
            if (comboDatas.Count == 0) return null;
            if (comboDatas[index].comboName == null)
            {
                Debug.LogWarning($"{index}的索引值下没有连招名");
                return null;
            }

            return comboDatas[index].comboName;
        }
        
        public float GetComboDamage(int index)
        { 
            if(comboDatas.Count == 0) { return 0f; }
            if (comboDatas[index].comboDamage == 0) { Debug.LogWarning(index + "的索引值下没有伤害"); }
            return comboDatas[index].comboDamage;
        }
        
        public string GetComboHitName(int index)
        {
            if (comboDatas.Count == 0) { return null; }
            if (comboDatas[index].hitName == null) { Debug.LogWarning(index + "的索引值下没有受伤名"); }
            return comboDatas[index].hitName;
        }
        
        public int GetComboATKCount(int index)
        {
            return comboDatas[index].ATKCount;
        }

        public void ResetComboDatas()
        {
            if (comboDatas == null)
            {
                Debug.Log(comboDatas + "是空的");
                return;
            }

            if (comboDatas[0] != firstComboData)
            {
                comboDatas[0] = firstComboData;
                Debug.Log("切换的结果为" + comboDatas[0].name);
            }
        }

        public int GetComboMaxCount()
        {
            return comboDatas.Count;
        }

        public void SwitchDodgeATK()
        {
            if (dodgeATKData == null)
            {
                return;
            }

            comboDatas[0] = dodgeATKData;
        }

        public float GetComboShakeForce(int index, int ATKIndex)
        {
            Debug.Log("ATKIndex为" + ATKIndex);
            Debug.Log("comboDatas[index].shakeForce.Length为" + (comboDatas[index].shakeForce.Length));

            if (comboDatas[index].shakeForce == null || ATKIndex > comboDatas[index].shakeForce.Length)
            {
                //说明我不设置Force或者没有设置全Force，代表每该ATK都没有震屏
                return 0;
            }

            return comboDatas[index].shakeForce[ATKIndex - 1];
        }

        public float GetComboDistance(int index)
        {
            if (comboDatas.Count == 0)
            {
                return 0;
            }

            if (comboDatas[index].attackDistance == 0)
            {
                Debug.LogWarning(index + "的索引值下没有设置连招的攻击距离");
            }

            return comboDatas[index].attackDistance;
        }
    }
}