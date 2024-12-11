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
        /// Scene�� �ʱ�ȭ �ؾߵǴ� �������� ��� �Լ�
        /// </summary>
        protected virtual void InitScene()
        {
            
        }

        public abstract void Clear();
    }
}
