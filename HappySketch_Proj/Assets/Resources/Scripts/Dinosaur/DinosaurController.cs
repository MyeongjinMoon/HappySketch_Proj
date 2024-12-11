using System.Collections;
using System.Collections.Generic;
using Jaehoon;
using UnityEngine;

namespace JongJin
{
    public class DinosaurController : MonoBehaviour
    {
        [SerializeField] private GameSceneController gameSceneController;

        [SerializeField] private float speed = 4.0f;

        private float soundIntervalFootstep = 0.0f;

        public float Speed { get { return speed; } }
        private void Start()
        {
 
        }

        private void Update()
        {
            if (gameSceneController.CurState != EGameState.RUNNING)
                return;
            Move();
        }

        private void Move()
        {
            transform.Translate(transform.forward * speed * Time.deltaTime);

            soundIntervalFootstep += Time.deltaTime;
            if (soundIntervalFootstep >= 1.05f)
            {
                soundIntervalFootstep = 0.0f;
                SoundManager.instance.SFXPlay("Sounds/DinosaurMove");
            }
        }
    }
}