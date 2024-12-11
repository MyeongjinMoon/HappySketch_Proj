using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HakSeung
{
    public abstract class BaseScene : MonoBehaviour
    {
        public ESceneType SceneType { get; protected set; } = ESceneType.END;

        private void Awake()
        {
            InitScene();
        }

        /// <summary>
        /// Scene의 초기화 해야되는 정보들을 담는 함수
        /// </summary>
        protected virtual void InitScene()
        {
            
        }

        public abstract void Clear();
    }
}
