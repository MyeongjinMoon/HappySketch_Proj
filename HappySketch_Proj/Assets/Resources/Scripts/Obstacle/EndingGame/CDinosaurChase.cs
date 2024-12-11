using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyeongJin
{
	public class CDinosaurChase : MonoBehaviour
	{
		[SerializeField] private GameObject dinosaur;               // 카메라가 따라다닐 타겟
		[SerializeField] private GameObject[] endingPlayer;               // 카메라가 따라다닐 타겟

		[SerializeField] private float offsetX;            // 카메라의 x좌표
		[SerializeField] private float offsetY;           // 카메라의 y좌표
		[SerializeField] private float offsetZ;          // 카메라의 z좌표
        public float playeroffsetX;            // 카메라의 x좌표
        public float playeroffsetY;           // 카메라의 y좌표
        public float playeroffsetZ;          // 카메라의 z좌표
        public float resultPlayeroffsetX;            // 카메라의 x좌표
        public float resultPlayeroffsetY;           // 카메라의 y좌표
        public float resultPlayeroffsetZ;          // 카메라의 z좌표
        [SerializeField] private float CameraSpeed;       // 카메라의 속도

		[SerializeField] private Vector3 shakeOffset;

		private GameObject Target;               // 카메라가 따라다닐 타겟
		private Quaternion originRotation;
		private Vector3 TargetPos;

		private CEndingSceneController cEndingSceneController;
		private bool isGameSuccess;
		private bool isJumpState = false;
		private bool isJumpStateChanged = false;
		private bool isAdjustPosition = false;

		private Animator targetAnimator;
		private Animator[] PlayerAnimator;

        private void Start()
		{
			cEndingSceneController = GameObject.Find("EndingSceneController").GetComponent<CEndingSceneController>();
			isGameSuccess = cEndingSceneController.isGameSuccess;
			originRotation = transform.rotation;

			if (isGameSuccess)
			{
				PlayerAnimator = new Animator[endingPlayer.Length];
                Target = endingPlayer[0];
				for (int i = 0; i < endingPlayer.Length; i++)
					PlayerAnimator[i] = endingPlayer[i].GetComponent<Animator>();
            }
			else
			{
				Target = dinosaur;
                targetAnimator = Target.GetComponent<Animator>();
            }
		}
		void FixedUpdate()
		{
			if (!isGameSuccess)
				TargetPos = new Vector3(
					Target.transform.position.x + offsetX,
					Target.transform.position.y + offsetY,
					Target.transform.position.z + offsetZ
					);
			else
			{
				if (!isAdjustPosition)
				{
					TargetPos = new Vector3(
					Target.transform.position.x + playeroffsetX,
					Target.transform.position.y + playeroffsetY,
					Target.transform.position.z + playeroffsetZ
					);
				}
				else
				{
                    TargetPos = new Vector3(
                    Target.transform.position.x + resultPlayeroffsetX,
                    Target.transform.position.y + resultPlayeroffsetY,
                    Target.transform.position.z + resultPlayeroffsetZ
                    );
                }
			}

            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
		}
		private void Update()
		{
			if (isGameSuccess)
			{
                if (!endingPlayer[0].activeSelf && !isAdjustPosition)
                {
					this.transform.rotation = Quaternion.Euler(22, 0, 0);
					isAdjustPosition = true;
                    playeroffsetY += 1;
                }
            }
			else
			{
				if (targetAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack jump Jaw"))
					isJumpState = true;
				else
				{
					if (isJumpState)
						isJumpStateChanged = true;
				}
            }
			if (isJumpStateChanged)
			{
				isJumpState = false;
				isJumpStateChanged = false;
				StartCoroutine(ShakeCoroutine(200));
            }
		}
		private IEnumerator ShakeCoroutine(float shakeForce)
        {
            float setTime = 1.0f;
            Vector3 originEuler = transform.eulerAngles;

			while (setTime > 0)
            {
                setTime -= Time.deltaTime;

                float rotationX = Random.Range(-shakeOffset.x, shakeOffset.x + 1);
				float rotationY = Random.Range(-shakeOffset.y, shakeOffset.y + 1);
				float rotationZ = Random.Range(-shakeOffset.z, shakeOffset.z + 1);

				Vector3 randomRotation = originEuler + new Vector3(rotationX, rotationY, rotationZ);
				Quaternion rotation = Quaternion.Euler(randomRotation);

				while (Quaternion.Angle(transform.rotation, rotation) > 0.1f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, shakeForce * Time.deltaTime);
					yield return null;
				}
				yield return null;
            }
            StartCoroutine(ResetCoroutine(500));
        }
		private IEnumerator ResetCoroutine(float shakeForce)
		{
			while (Quaternion.Angle(transform.rotation, originRotation) > 0)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, originRotation, shakeForce * Time.deltaTime);
				yield return null;
			}
			yield return null;
            StartCoroutine(ShakeCoroutine(200));
        }
	}
}