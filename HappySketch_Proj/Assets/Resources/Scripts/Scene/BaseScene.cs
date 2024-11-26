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
        /// Scene�� �ʱ�ȭ �ؾߵǴ� �������� ��� �Լ�
        /// </summary>
        protected virtual void InitScene()
        {
            //TODO<�н�> ��ӽ��Ѽ� UIManager�� ���� ���� Scene�� Show()������ �ҷ��� ���� 24/11/13 
        }

        public abstract void Clear();
    }
}
