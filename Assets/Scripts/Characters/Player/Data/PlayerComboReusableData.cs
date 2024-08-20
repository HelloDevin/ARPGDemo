using UnityEngine;

namespace ZZZ
{
    public class PlayerComboReusableData
    {
        public Transform cameraTransform;

        public Vector3 detectionDir;

        public Vector3 detectionOrigin;


        public ComboContainerData currentCombo;

        public ComboData currentSkill;

        /// <summary>
        /// 攻击伤害点索引（）一个攻击动画可能有几个伤害检测事件
        /// </summary>
        public int ATKIndex;

        public int comboIndex;

        /// <summary>
        /// 防止因为index更新导致ATK传递index出现不对应的数值
        /// </summary>
        public BindableProperty<int> currentIndex = new BindableProperty<int>();
        
        /// <summary>
        /// 输入的允许输入时间，相当于是否开启 预输入
        /// </summary>
        public bool canInput;
        
        /// <summary>
        /// 攻击动画最小时间播放的开关，相当于连招 冷却时间
        /// </summary>
        public bool canATK;


        /// <summary>
        /// 在可以攻击的条件下按下攻击键触发 攻击指令
        /// </summary>
        public bool hasATKCommand;

        public bool canLink; //可以衔接连招

        /// <summary>
        /// 可以通过移动打断
        /// </summary>
        public bool canMoveInterrupt;

        public int executeIndex;

        public bool canQTE; //触发切人技能特写的条件
    }
}