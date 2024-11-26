using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager sInstance;
        public static InputManager Instance {
            get {
                if (sInstance == null) {
                    GameObject newManagersObject = new GameObject("@InputManager");
                    sInstance = newManagersObject.AddComponent<InputManager>();
                }
                return sInstance;
            }
        }
        private void Awake() {
            if (sInstance != null && sInstance != this) {
                Destroy(this.gameObject);
                return;
            }
            sInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update() 
        {
            OnUpdate();
        }

        public Action KeyAction = null;
        public void OnUpdate()
        {
            if (!Input.anyKey)
                return;

            if (KeyAction != null)
                KeyAction.Invoke();
        }
    }
}