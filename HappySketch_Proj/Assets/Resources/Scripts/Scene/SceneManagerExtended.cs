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
        /// Instance를 받아와 쓰므로 어디선가에서 호출해주어야한다.
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
        ///  씬별 이름을 변환하는 코드 수정필요
        ///  Scene들의 이름을 미리 가져오는 방법을 모르겠어서 일단 이 코드를 통해 type을 넣으면 LoadScene에 이용할 수 있도록 해놓았음
        ///  Scene을 미리 만들어 놓고 껏다 키는 방식인건가??
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
            // TODO<이종진> - Scene 전환시 클리어에서 오류 발생, 의논 필요 - 20241118
            // CurrentScene.Clear();
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
            // TODO<이종진> - 매직 상수 수정 필요 / 임시 2명의 플레이어라고 가정하고 코드 작성 - 20241118
            for (int playerNum = 0; playerNum < 2; playerNum++)
                if (!isPlayersReady[playerNum]) 
                    return false;

            return true;
        }
        public IEnumerator GoToGameScene()
        {
            InputManager.Instance.KeyAction = null;
            yield return new WaitForSeconds(3.0f);
            LoadScene(ESceneType.GAME);
        }
    }
}
