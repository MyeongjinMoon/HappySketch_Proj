using HakSeung;
using MyeongJin;
using Jaehoon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace JongJin
{
    public class PlayerController : MonoBehaviour
    {
        private readonly string[] playerTag = { "Player1", "Player2", "Player3", "Player4" };
        private readonly string groundTag = "Ground";
        private readonly string paramMission = "isMission";
        private readonly string paramSpeed = "speed";
        private readonly string paramJump = "isJump";
        private readonly string jumpAniName = "Jump";
        private readonly string paramCrouch = "isCrouch";
        private readonly string crouchAniName = "Crouch";
        private readonly string paramHeart = "isHeart";
        private readonly string heartAniName = "Heart";
        private readonly string paramLeftTouch = "isLeftTouch";
        private readonly string leftTouchAniName = "Left Touch";
        private readonly string paramRightTouch = "isRightTouch";
        private readonly string rightTouchAniName = "Right Touch";
        private readonly string idleAniName = "Idle";


        enum EPlayer { PLAYER1, PLAYER2, PLAYER3, PLAYER4 }
        enum EPlayerState { CUTSCENE, RUNNING, MISSION }

        // TODO<������> - �׽�Ʈ�� �ۼ� �����ʿ� - 20241110
        [SerializeField] private CSpawnController cSpawnController;
        [SerializeField] private GameSceneController gameSceneController;

        [SerializeField] private BoxCollider upCollider;
        [SerializeField] private BoxCollider downCollider;

        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float increaseSuccessSpeed = 1.0f;
        [SerializeField] private float decreaseFailSpeed = 0.5f;
        [SerializeField] private float jumpForce = 5.0f;

        [SerializeField] private float increaseSpeed = 0.1f;
        [SerializeField] private float decreaseSpeed = 0.5f;

        [SerializeField] private float minSpeed = 0.5f;
        [SerializeField] private float maxSpeed = 10.0f;

        private EPlayer playerId;
        private RunningState runningController;
        private EPlayerState curState;

        private int isGrounded = 0;
        private bool isActivated = false;

        private Rigidbody rigid;
        private Animator animator;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            InputManager.Instance.KeyAction -= OnKeyBoard;
            InputManager.Instance.KeyAction += OnKeyBoard;

            for (int playerNum = 0; playerNum < playerTag.Length; playerNum++)
            {
                if (this.tag != playerTag[playerNum]) continue;
                playerId = (EPlayer)playerNum;
                break;
            }

            if (gameSceneController != null)
            {
                runningController = gameSceneController.GetComponent<RunningState>();
                animator.SetFloat(paramSpeed, speed);
            }
            curState = EPlayerState.CUTSCENE;
        }

        private void Update()
        {
            UpdateState();
            if (curState == EPlayerState.RUNNING)
                Move();
        }

        private void OnKeyBoard()
        {
            if (gameSceneController == null &&
                ((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.LeftShift))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.RightShift))))
            {
                Heart();
            }

            if (isGrounded <= 0 || isActivated)
                return;

            if (curState == EPlayerState.RUNNING
                && (playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.S))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.DownArrow)))
            {
                IncreaseSpeed();
            }
            if ((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.W))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                Jump();
            }

            if (curState == EPlayerState.RUNNING && gameSceneController != null)
                return;

            if ((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.A))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                LeftTouch();
            }
            if ((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.D))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                RightTouch();
            }
            if ((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.LeftControl))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.RightControl)))
            {
                Crouch();
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if(collision == null) return;

            switch (collision.gameObject.tag)
            {
                case "Ground":
                    animator.SetBool(paramJump, false);
                    isGrounded++;
                    if (downCollider != null) downCollider.enabled = true;
                    break;
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision == null) return;

            switch (collision.gameObject.tag)
            {
                case "Ground":
                    isGrounded--;
                    break;
            }
        }
        private void UpdateState()
        {
            if (gameSceneController == null)
                return;

            if (curState != EPlayerState.RUNNING
                && gameSceneController.CurState == EGameState.RUNNING)
                SetRunningState();
            else if (curState != EPlayerState.MISSION
                  && gameSceneController.CurState != EGameState.RUNNING
                  && gameSceneController.CurState != EGameState.CUTSCENE)
                SetMissionState();
        }
        private void SetRunningState()
        {
            isGrounded = 0;

            curState = EPlayerState.RUNNING;
            animator.SetBool(paramMission, false);
            transform.position = runningController.GetPlayerPrevPosition((int)playerId);

            if (runningController.isMissionSuccess) speed += increaseSuccessSpeed;
            else speed -= decreaseFailSpeed;
        }
        private void SetMissionState()
        {
            curState = EPlayerState.MISSION;
            animator.SetBool(paramMission, true);

            transform.position = new Vector3(148f + (int)playerId * 4f, 2.0f, 0.0f);
        }

        private void Move()
        {
            if (gameSceneController == null)
                return;

            DecreaseSpeed();

            if (!runningController.IsBeyondMaxDistance(this.transform.position))
                return;
            if (runningController.IsUnderMinDistance(this.transform.position, out Vector3 curPos)
                && speed < runningController.DinosaurSpeed)
            {
                this.transform.position = curPos;
                return;
            }

            transform.Translate(transform.forward * Time.deltaTime * speed);
        }
        private void Jump()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(jumpAniName))
                animator.Play(jumpAniName, -1, 0f);
            animator.SetBool(paramJump, true);
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (downCollider != null) downCollider.enabled = false;
        }
        private void Crouch()
        {
            StartCoroutine(CrouchActive());
        }
        private void Heart()
        {
            if (SceneManagerExtended.Instance.GetReady((int)playerId))
                HeartDeActive();
            else
                HeartActive();
        }
        private void LeftTouch()
        {
            StartCoroutine(LeftTouchActive());

            if (cSpawnController == null) return;

            if (gameSceneController.CurState == EGameState.SECONDMISSION)
                cSpawnController.GenerateSwatter((int)playerId, 0);
            else if (gameSceneController.CurState == EGameState.THIRDMISSION)
                cSpawnController.GenerateRay();
        }
        private void RightTouch()
        {
            StartCoroutine(RightTouchActive());

            if (cSpawnController == null) return;

            if (gameSceneController.CurState == EGameState.SECONDMISSION)
                cSpawnController.GenerateSwatter((int)playerId, 1);
            else if (gameSceneController.CurState == EGameState.THIRDMISSION)
                cSpawnController.GenerateRay();
        }
        private void IncreaseSpeed()
        {
            if (gameSceneController == null)
                return;

            if (speed > maxSpeed)
                return;

            speed += increaseSpeed;
            animator.SetFloat(paramSpeed, speed);
        }
        private void DecreaseSpeed()
        {
            if (speed < minSpeed)
                return;
            speed -= Time.deltaTime * decreaseSpeed;
            animator.SetFloat(paramSpeed, speed);
        }
        private void HeartActive()
        {
            animator.SetBool(paramHeart, true);
            isActivated = true;

            SceneManagerExtended.Instance.SetReady((int)playerId, true);
            if (SceneManagerExtended.Instance.CheckReady())
                StartCoroutine(SceneManagerExtended.Instance.GoToGameScene());
        }
        private void HeartDeActive()
        {
            SceneManagerExtended.Instance.SetReady((int)playerId, false);
            isActivated = false;
            animator.SetBool(paramHeart, false);
        }

        IEnumerator CrouchActive()
        {
            animator.SetBool(paramCrouch, true);
            isActivated = true;
            if (upCollider != null) upCollider.enabled = false;

            yield return new WaitForSeconds(0.3f);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float curAnimationTime = stateInfo.length;
            yield return new WaitForSeconds(curAnimationTime);

            if (upCollider != null) upCollider.enabled = true;
            isActivated = false;
            animator.SetBool(paramCrouch, false);
        }

        IEnumerator LeftTouchActive()
        {
            animator.SetBool(paramLeftTouch, true);
            isActivated = true;

            yield return new WaitForSeconds(0.3f);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float curAnimationTime = stateInfo.length;
            yield return new WaitForSeconds(curAnimationTime);

            isActivated = false;
            animator.SetBool(paramLeftTouch, false);
        }
        IEnumerator RightTouchActive()
        {
            animator.SetBool(paramRightTouch, true);
            isActivated = true;

            yield return new WaitForSeconds(0.3f);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float curAnimationTime = stateInfo.length;
            yield return new WaitForSeconds(curAnimationTime);

            isActivated = false;
            animator.SetBool(paramRightTouch, false);
        }
        public void OnBuff()
        {

        }
        public void OnDeBuff()
        {

        }
    }
}