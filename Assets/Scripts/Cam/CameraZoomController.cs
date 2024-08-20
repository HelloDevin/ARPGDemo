using System;
using Cinemachine;
using UnityEngine;

namespace ZZZ
{
    public class CameraZoomController : MonoBehaviour
    {
        private const float TOLERANCE = 0.01f;
        
        // 分别设置默认、最小、最大距离
        [SerializeField] [Range(1, 10f)] private float defaultDistance = 6f;
        [SerializeField] [Range(1, 10f)] private float minimumDistance = 1f;
        [SerializeField] [Range(1, 10f)] private float maximumDistance = 6f;

        // 缩放平滑度和灵敏度设置
        [SerializeField] [Range(0, 10f)] private float smoothing = 4f;
        [SerializeField] [Range(0, 10f)] private float zoomSensitivity = 1f;

        // 当前目标距离变量
        private float currentTargetDistance;

        // CinemachineFramingTransposer组件，用于控制相机的缩放
        private CinemachineFramingTransposer framingTransposer;

        // CinemachineInputProvider组件，用于获取输入值
        private CinemachineInputProvider inputProvider;

        // 在Awake方法中获取组件并初始化当前目标距离
        private void Awake()
        {
            // 获取CinemachineVirtualCamera组件并从中获取CinemachineFramingTransposer组件
            framingTransposer = GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineFramingTransposer>();
            // 获取CinemachineInputProvider组件
            inputProvider = GetComponent<CinemachineInputProvider>();
            // 设置当前目标距离为默认距离
            currentTargetDistance = defaultDistance;
        }

        // 在Update方法中调用Zoom方法进行缩放处理
        private void Update()
        {
            Zoom();
        }

        // Zoom方法用于处理相机的缩放逻辑
        private void Zoom()
        {
            // 获取输入轴的值并乘以缩放灵敏度
            float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;
            // 根据输入值更新当前目标距离，并限制在最小和最大距离之间
            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minimumDistance, maximumDistance);

            // 获取当前相机距离
            float currentDistance = framingTransposer.m_CameraDistance;
            // 如果当前相机距离已经等于目标距离，则不进行任何操作
            if (Math.Abs(currentDistance - currentTargetDistance) < TOLERANCE)
            {
                return;
            }

            // 使用线性插值方法平滑过渡到目标距离
            float lerpZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
            // 设置相机距离为平滑过渡后的值
            framingTransposer.m_CameraDistance = lerpZoomValue;
        }
    }
}