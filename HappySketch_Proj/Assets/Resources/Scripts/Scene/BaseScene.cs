using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HakSeung
{
    public abstract class BaseScene : MonoBehaviour
    {
        public ESceneType SceneType { get; protected set; } = ESceneType.END; //DefaultScene Value

        private void Awake()
        {
            InitScene();
        }

        /// <summary>
        /// Scene의 초기화 해야되는 정보들을 담는 함수
        /// </summary>
        protected virtual void InitScene()
        {
            //TODO<학승> 상속시켜서 UIManager를 통해 현재 Scene의 Show()같은거 불러올 예정 24/11/13 
        }

        public abstract void Clear();
    }
}
