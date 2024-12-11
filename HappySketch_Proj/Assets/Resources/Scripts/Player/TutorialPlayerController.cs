using HakSeung;
using Jaehoon;
using JongJin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HakSeung
{
    public class TutorialPlayerController : MonoBehaviour
    {
        private readonly string[] playerTag = { "Player1", "Player2", "Player3", "Player4" };
        private readonly string groundTag = "Ground";
        private readonly string paramHeart = "isHeart";
        private readonly string paramSpeed = "speed";
        private readonly string paramJump = "isJump";
        private readonly string jumpAniName = "Jump";

        enum EPlayer { PLAYER1, PLAYER2, PLAYER3, PLAYER4 }
        [SerializeField] private StartSceneController startSceneController;
        [SerializeField] private float jumpForce = 5.0f;

        [SerializeField] private float speed = 0.0f;

        [SerializeField] private float maxSpeed = 10.0f;
        [SerializeField] private float minSpeed = 0.0f;

        private float increaseSpeed = 0.5f;
        private float decreaseSpeed = 0.5f;

        private Animator animator;
        private Rigidbody rigid;

        private EPlayer playerId;

        private int isGrounded = 0;
        private bool isActivated = false;
        
        public float Speed { get { return speed; } }
        public bool ActionTrigger { get; private set; } = false;
        private CUITutorialPopup.TutorialState curTutorialState;

        private void Awake()
        {

            rigid = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            PlayerReset(CUITutorialPopup.TutorialState.STORY);
        }

        void Start()
        {
            InputManager.Instance.KeyAction -= OnKeyBoard;
            InputManager.Instance.KeyAction += OnKeyBoard;

            for (int playerNum = 0; playerNum < playerTag.Length; playerNum++)
            {
                if (this.tag != playerTag[playerNum]) continue;
                playerId = (EPlayer)playerNum;
                break;
            }
        }

        void Update()
        {
            if (speed > 0.0f)
                DecreaseSpeed();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag(groundTag))
                return;

            animator.SetBool(paramJump, false);
            isGrounded++;

            if (curTutorialState == CUITutorialPopup.TutorialState.JUMP)
                ActionTrigger = false;
        }
        private void OnCollisionExit(Collision collision)
        {
            if (!collision.gameObject.CompareTag(groundTag))
                return;
            
            isGrounded--;

            if (curTutorialState == CUITutorialPopup.TutorialState.JUMP)
                ActionTrigger = true;
        }

        private void OnKeyBoard()
        {
            if (startSceneController.CurState != EStartGameState.TUTORIALACTION)
                return;

            if (isGrounded <= 0)
                return;

            if (((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.LeftShift))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.RightShift))))
                Heart();

            if ( isActivated)
                return;
            if ((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.S))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.DownArrow)))
                IncreaseSpeed();

            if ((playerId == EPlayer.PLAYER1 && Input.GetKeyDown(KeyCode.W))
                || (playerId == EPlayer.PLAYER2 && Input.GetKeyDown(KeyCode.UpArrow)))
                Jump();

        }

        private void Jump()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(jumpAniName))
                animator.Play(jumpAniName, -1, 0f);
            animator.SetBool(paramJump, true);
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            SoundManager.instance.SFXPlay("Sounds/PlayerJump");
        }

        private void IncreaseSpeed()
        {
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
                return;
            }

            speed += increaseSpeed;
            animator.SetFloat(paramSpeed, speed);
        }

        private void DecreaseSpeed()
        {
            if (speed <= minSpeed)
            {
                speed = 0;
                return;
            }
            speed -= Time.deltaTime * decreaseSpeed;
            animator.SetFloat(paramSpeed, speed);
        }

        private void Heart()
        {
            if (isActivated)
                HeartDeActive();
            else
                HeartActive();
        }

        private void HeartActive()
        {
            speed = minSpeed;
            animator.SetFloat(paramSpeed, speed);

            animator.SetBool(paramHeart, true);
            isActivated = true;

            if (curTutorialState == CUITutorialPopup.TutorialState.HEART)
            {
                SceneManagerExtended.Instance.SetReady((int)playerId, true);
            }
        }
            private void HeartDeActive()
        {
            isActivated = false;
            animator.SetBool(paramHeart, false);

            if (curTutorialState == CUITutorialPopup.TutorialState.HEART)
                 SceneManagerExtended.Instance.SetReady((int)playerId, false);
                 
        }

        public void PlayerReset(CUITutorialPopup.TutorialState tutorialState)
        {
            curTutorialState = tutorialState;
            increaseSpeed = 0.5f;
            decreaseSpeed = 0.5f;
            speed = minSpeed;
            animator.SetFloat(paramSpeed, minSpeed);
            ActionTrigger = false;
        }


    }
}