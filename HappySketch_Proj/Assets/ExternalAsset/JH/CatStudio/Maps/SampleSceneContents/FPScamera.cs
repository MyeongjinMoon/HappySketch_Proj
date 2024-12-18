using UnityEngine;
using System.Collections;


namespace CatStudio.AssetMap
{


	public class FPScamera : MonoBehaviour
	{

		private Camera mCamera;
		private CharacterController mCharacterController;

		[SerializeField]
		private float mCamSpeed = 1.0f;

		[SerializeField]
		private float mWalkSpeed = 3.0f;
		[SerializeField]
		private float mRunSpeed = 6.0f;
		[SerializeField]
		private float mJumpPower = 4.5f;

		[SerializeField]
		private GameObject mTextOrientation;


		private Vector3 mGravity = Vector3.zero;

		private bool mJumpCheck = false;


		private float startTime;


		private Vector3 mCamTra;



		void Awake()
		{
			Application.targetFrameRate = 60;
		}





		// Use this for initialization
		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			mCamera = Camera.main;
			mCharacterController = GetComponent<CharacterController>();
		}

		// Update is called once per frame
		void Update()
		{

			if (GroundedTouchCheck() == true)
			{

				if (mJumpCheck == true)
				{
					//Debug.Log("着地");
					mCamTra = mCamera.transform.localPosition;
					StartCoroutine(LandingChara());
					mJumpCheck = false;
				}

				if (Input.GetButtonDown("Jump"))
				{
					mGravity.y = mJumpPower;
					StopCoroutine(JumpCheckWait());
					StartCoroutine(JumpCheckWait());
				}
			}

		}



		void OnGUI()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}

		}



		void FixedUpdate()
		{
			//Vector3 playerDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
			//Debug.Log("Mouse X : " + Input.GetAxis("Mouse X"));
			//Debug.Log("Mouse Y : " + Input.GetAxis("Mouse Y"));

			LookRot(mCamera.transform);
			CharaMove();

			if (mTextOrientation)
			{
				mTextOrientation.transform.localEulerAngles = new Vector3(0.0f, 0.0f, this.transform.localEulerAngles.y);
			}



			// 落下
			mGravity.y += Physics.gravity.y * Time.fixedDeltaTime;
			mCharacterController.Move(mGravity * Time.fixedDeltaTime);

			// 着地していたら速度を0にする
			if (mCharacterController.isGrounded)
			{
				mGravity.y = 0;
			}


		}



		public void LookRot(Transform camera)
		{
			float yRot;
			float xRot;

			yRot = Input.GetAxis("Mouse X") * mCamSpeed;
			xRot = Input.GetAxis("Mouse Y") * mCamSpeed;

			Quaternion m_CameraTargetRot = camera.localRotation;

			camera.localRotation *= Quaternion.Euler(-xRot, 0.0f, 0.0f);

			this.transform.localRotation *= Quaternion.Euler(0.0f, yRot, 0.0f);

		}


		public void CharaMove()
		{
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			float speed;

			Vector3 desiredMove = this.transform.forward * vertical + this.transform.right * horizontal;

			if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire1"))
			{
				speed = mRunSpeed;
			}
			else
			{
				speed = mWalkSpeed;
			}
			desiredMove.x = desiredMove.x * speed;
			desiredMove.z = desiredMove.z * speed;


			if (mCharacterController)
			{
				mCharacterController.Move(desiredMove * Time.fixedDeltaTime);
			}

		}




		private IEnumerator LandingChara()
		{

			float speed = 40.0f;
			float distance = 0.1f;

			startTime = Time.timeSinceLevelLoad;
			bool loopcheck = true;
			while (loopcheck == true)
			{

				float diff = Time.timeSinceLevelLoad - startTime;
				mCamera.transform.localPosition = new Vector3(0.0f, (Mathf.Cos(diff * speed) * distance + (mCamTra.y - distance)), 0.0f);


				if (diff > (Mathf.PI * 2.0f) / speed)
				{
					loopcheck = false;
				}

				yield return null;
			}

			mCamera.transform.localPosition = mCamTra;

		}



		private IEnumerator JumpCheckWait()
		{

			yield return new WaitForSeconds(0.3f);
			mJumpCheck = true;
		}



		public bool GroundedTouchCheck()
		{
			if (mCharacterController.isGrounded)
			{
				return true;
			}

			float dist = 0.2f;
			Ray ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);

			return Physics.Raycast(ray, dist);
		}



	}

}