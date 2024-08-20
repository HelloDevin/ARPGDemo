using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace ZZZ
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
        }

        void Update()
        {
        }

        public void Test1()
        {
            Debug.Log(transform.forward);
            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;

            Debug.Log(angleCurrent);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Test))]
    public class TestEditor : UnityEditor.Editor
    {
        private Test _testScript;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _testScript = (Test) target;
            if (GUILayout.Button("Test"))
            {
                _testScript.Test1();
            }
        }
    }
#endif
}