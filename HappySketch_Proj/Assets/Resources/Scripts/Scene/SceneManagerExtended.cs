using JongJin;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        private Image fadeImage;

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

            fadeImage = GameObject.Find("PanelFade").GetComponent<Image>();
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

            float time = 0.0f;

            Color alpha = fadeImage.color;
            while (time < 1.0f)
            {
                time += Time.deltaTime;
                alpha.a = Mathf.Lerp(0.0f, 1.0f, time);
                fadeImage.color = alpha;
                yield return null;
            }

            LoadScene(ESceneType.GAME);
        }
        public IEnumerator GoToEndingScene()
        {
            UIManager.Instance.ClearAllUI();
            InputManager.Instance.KeyAction = null;
            yield return new WaitForSeconds(1.0f);
            LoadScene(ESceneType.ENDING);
        }
        public void GetFadeImageInStart()
        {
            fadeImage = GameObject.Find("PanelFade").GetComponent<Image>();
        }
    }
}
