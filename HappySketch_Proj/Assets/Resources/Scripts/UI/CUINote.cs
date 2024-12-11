using Cinemachine;
using HakSeung;
using JongJin;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HakSeung
{
	public class CUINote : MonoBehaviour
	{
		private enum ENoteImageObject
		{
			HITCHECKRING,
			SUCCESS,
			FAIL,

			END
		}

		[SerializeField] private bool isHit;
		[SerializeField] private float curTime;
		[SerializeField] private const float noteHitCheckTime = 3f;
		[SerializeField] private const float noteHitResultTime = 1f;
		[SerializeField] private const float hitCheckRingScale = 3f;
		[SerializeField] private const float distanceToPlayerPostion = 1f;
		[SerializeField] private Transform playerTransform;

		private const float noteFailTime = 0f;

		private GameObject obstacle;
		private CinemachineVirtualCamera mainCamera;
		private GameObject[] player;

		private int myPlayerNum;

		public GameObject[] noteObjects = new GameObject[(int)ENoteImageObject.END];

		Coroutine coCheckNoteHit;

		private void Init()
		{
			curTime = noteFailTime;
			mainCamera = GameObject.Find("RunningVirtualCamera").GetComponent<CinemachineVirtualCamera>();
            player = new GameObject[2];
            player[0] = GameObject.FindWithTag("Player1");
            player[1] = GameObject.FindWithTag("Player2");
		}
		public void Show(GameObject newObstacle, int playerNum)
		{
            gameObject.GetComponent<Image>().color = Color.white;
            myPlayerNum = playerNum;
            obstacle = newObstacle;
			this.gameObject.SetActive(true);
		}
		private void Awake()
		{
			Init();
		}
		private void OnEnable()
		{
			isHit = false;
			curTime = noteHitCheckTime;

			noteObjects[(int)ENoteImageObject.HITCHECKRING].SetActive(true);
			noteObjects[(int)ENoteImageObject.SUCCESS].SetActive(false);
			noteObjects[(int)ENoteImageObject.FAIL].SetActive(false);

			noteObjects[(int)ENoteImageObject.HITCHECKRING].transform.localScale *= hitCheckRingScale;

			coCheckNoteHit = StartCoroutine(IECheckNoteHitInSuccessTime());
		}

		private void OnDisable()
		{
			if (coCheckNoteHit != null)
				StopCoroutine(coCheckNoteHit);

			curTime = noteFailTime;
			noteObjects[(int)ENoteImageObject.HITCHECKRING].transform.localScale = Vector3.one;
		}

		private IEnumerator IECheckNoteHitInSuccessTime()
		{
			float hitNoteScale = transform.localScale.x;
			Vector3 initGap = GetObstaclePosition(obstacle) - GetPlayerPosition(player[myPlayerNum]);

			while (GetPlayerPosition(player[myPlayerNum]).z < GetObstaclePosition(obstacle).z)
			{
                float progress = (GetObstaclePosition(obstacle).z - GetPlayerPosition(player[myPlayerNum]).z) / initGap.z;
                noteObjects[(int)ENoteImageObject.HITCHECKRING].transform.localScale =
                    Vector3.Lerp(Vector3.one * hitCheckRingScale, Vector3.one, 1 - Mathf.Clamp01(progress));

				SyncUIWithPlayerPosition(GetPlayerPosition(player[myPlayerNum]));

				yield return null;
            }

			curTime = 0;

			if (obstacle.GetComponent<BoxCollider>().enabled)
				isHit = true;

			if (isHit)
			{
				gameObject.GetComponent<Image>().color = Color.green;
				player[myPlayerNum].GetComponent<PlayerController>().OnBuff();
			}
			else
			{
				gameObject.GetComponent<Image>().color = Color.red;
                player[myPlayerNum].GetComponent<PlayerController>().OnDeBuff();
            }

			noteObjects[(int)ENoteImageObject.HITCHECKRING].SetActive(false);

			while (curTime <= noteHitResultTime)
			{
				curTime += Time.deltaTime;

				SyncUIWithPlayerPosition(GetPlayerPosition(player[myPlayerNum]));

				yield return null;
			}

			gameObject.GetComponent<Image>().color = Color.white;
			this.gameObject.SetActive(false);
		}
		private Vector3 GetPlayerPosition(GameObject player)
		{
			return player.transform.position;
        }
        private Vector3 GetObstaclePosition(GameObject obstacle)
        {
            return obstacle.transform.position;
        }
        private void SyncUIWithPlayerPosition(Vector3 playerPosition)
		{
			gameObject.GetComponent<RectTransform>().anchoredPosition = 
				Camera.main.WorldToScreenPoint(playerPosition + Vector3.up * distanceToPlayerPostion);

			Vector2 temp = new Vector2(1920 / 2, 1080 / 2);

			gameObject.GetComponent<RectTransform>().anchoredPosition -= temp;
        }
	}
}