using HakSeung;
using Jaehoon;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

namespace MyeongJin
{
	public class CFly : MonoBehaviour
	{
		public IObjectPool<CFly> Pool { get; set; }

		private Vector3 startPosition;

		private ParticleSystem flyParticleSystem;
		private ParticleSystem lightBugParticleSystem;
        private GameObject blood;

        private GameObject gameSceneController;
        private GameSceneController gamecSceneController;
        private EGameState curState = EGameState.SECONDMISSION;

        private bool isDead = false;

        private void Awake()
		{
            flyParticleSystem = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            lightBugParticleSystem = this.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
			blood = this.transform.GetChild(2).gameObject;

            gameSceneController = GameObject.Find("GameSceneController");
            gamecSceneController = gameSceneController.GetComponent<GameSceneController>();
        }
        private void Update()
        {
            if (IsStateChanged())
                ReturnToPool();
            if (!isDead)
            {
                isDead = true;
                SoundManager.instance.SFXPlay("Sounds/InsectFly", 1.5f);
            }
        }
        private void OnDisable()
		{
			ResetObstacle();
		}
        public void ReturnToPool(int fillValue)
        {
            if (((CUIEventPanel)UIManager.Instance.CurSceneUI) != null)
                ((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.FillProgressBar(fillValue);
            Pool.Release(this);
        }
        public void ReturnToPool()
        {
            Pool.Release(this);
        }
        public void ResetObstacle()
		{
			this.GetComponent<BoxCollider>().enabled = true;
            blood.SetActive(false);

            var mainModule = flyParticleSystem.main;
            mainModule.gravityModifier = 0f;

            var mainModule2 = lightBugParticleSystem.main;
            mainModule2.gravityModifier = 0f;
        }
        protected bool IsStateChanged()
        {
            return curState != gamecSceneController.CurState;
        }
        private void OnTriggerEnter(Collider other)
		{
			this.GetComponent<BoxCollider>().enabled = false;

			blood.SetActive(true);

			var mainModule = flyParticleSystem.main;
            mainModule.gravityModifier = 0.6f;

            var mainModule2 = lightBugParticleSystem.main;
            mainModule2.gravityModifier = 0.6f;

            SoundManager.instance.SFXPlay("Sounds/InsectKill");

            StartCoroutine("Catch");
		}
		private IEnumerator Catch()
		{
			yield return new WaitForSeconds(1.0f);

            ReturnToPool(20);
		}
	}
}