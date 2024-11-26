using HakSeung;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

		//TODO<�н�> const �빮�� ��ȯ�ؾߵ� 11/13
		[SerializeField] private bool isHit;
		[SerializeField] private float curTime;
		[SerializeField] private const float noteHitCheckTime = 3f;
		[SerializeField] private const float noteHitResultTime = 1f;
		[SerializeField] private const float hitCheckRingScale = 3f;
		[SerializeField] private const float distanceToPlayerPostion = 1f;
		[SerializeField] private Transform playerTransform;

		private const float noteFailTime = 0f;

		// >>: ���� �߰� �ڵ�
		private GameObject obstacle;
		private GameObject[] player;

		private int myPlayerNum;
		// <<

		public GameObject[] noteObjects = new GameObject[(int)ENoteImageObject.END];

		Coroutine coCheckNoteHit; // �ڷ�ƾ ���� �𸣰ھ �ӽ�

		private void Init()
		{
			curTime = noteFailTime;
            player = new GameObject[2];
            player[0] = GameObject.FindWithTag("Player1");
            player[1] = GameObject.FindWithTag("Player2");
		}
		public void Show(GameObject newObstacle, int playerNum)
		{
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
				gameObject.GetComponent<Image>().color = Color.green; //���߿� �̹����� �޾ƿ��°� ���� �ʿ�
			else
				gameObject.GetComponent<Image>().color = Color.red;

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
			this.transform.position = Camera.main.WorldToScreenPoint(playerPosition + Vector3.up * distanceToPlayerPostion);
		}
	}
}