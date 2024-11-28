using JongJin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HakSeung
{
    public enum ESceneType
    {
        START,
        GAME,
        ENDING,

        END
    }

    public class SceneManagerExtended: MonoBehaviour
    {
        public BaseScene CurrentScene { get; }

        private static SceneManagerExtended s_Instance;

        private const string sceneManagerObjectName = "@SceneManager";


        /// <summary>
        /// Instance�� �޾ƿ� ���Ƿ� ��𼱰����� ȣ�����־���Ѵ�.
        /// </summary>
        public static SceneManagerExtended Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    GameObject newSceneManagerObject = new GameObject(sceneManagerObjectName);
                    s_Instance = newSceneManagerObject.AddComponent<SceneManagerExtended>();
                }

                return s_Instance;
            }
        }

        private bool[] isPlayersReady = new bool[4];

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            s_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        /// <summary>
        ///  ���� �̸��� ��ȯ�ϴ� �ڵ� �����ʿ�
        ///  Scene���� �̸��� �̸� �������� ����� �𸣰ھ �ϴ� �� �ڵ带 ���� type�� ������ LoadScene�� �̿��� �� �ֵ��� �س�����
        ///  Scene�� �̸� ����� ���� ���� Ű�� ����ΰǰ�??
        /// </summary>
        public string SceneTypeToString(ESceneType type)
        {
            string sceneName = string.Empty;
            switch(type)
            {
                case ESceneType.START:
                    sceneName = "StartScene";
                    break;
                case ESceneType.GAME:
                    sceneName = "GameScene";
                    break;
                case ESceneType.ENDING:
                    sceneName = "EndingScene";
                    break;
                default:
                    sceneName = "NOTING";
                    Debug.Log($"this scene does not exist");
                    break;
            }

            return sceneName;
        }

        public void LoadScene(ESceneType type)
        {
            // TODO<������> - Scene ��ȯ�� Ŭ����� ���� �߻�, �ǳ� �ʿ� - 20241118

            SceneManager.LoadScene(SceneTypeToString(type));
        }

        public bool GetReady(int playerNum)
        {
            return isPlayersReady[playerNum];
        }
        public void SetReady(int playerNum, bool isReady)
        {
            isPlayersReady[playerNum] = isReady;
        }
        public bool CheckReady()
        {
            // TODO<������> - ���� ��� ���� �ʿ� / �ӽ� 2���� �÷��̾��� �����ϰ� �ڵ� �ۼ� - 20241118
            for (int playerNum = 0; playerNum < 2; playerNum++)
                if (!isPlayersReady[playerNum]) 
                    return false;

            return true;
        }
        public IEnumerator GoToGameScene()
        {
            InputManager.Instance.KeyAction = null;
            UIManager.Instance.ClearAllUI();
            yield return new WaitForSeconds(3.0f);
            LoadScene(ESceneType.GAME);
        }
        public IEnumerator GoToEndingScene()
        {
            InputManager.Instance.KeyAction = null;
            yield return new WaitForSeconds(3.0f);
            LoadScene(ESceneType.ENDING);
        }
    }
}
