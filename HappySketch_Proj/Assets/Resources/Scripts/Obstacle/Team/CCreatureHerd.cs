using HakSeung;
using JongJin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Rendering.InspectorCurveEditor;

namespace MyeongJin
{
	public class CCreatureHerd : MonoBehaviour
	{
		public IObjectPool<CCreatureHerd> Pool { get; set; }

		private Animator animator;

		private GameObject gameSceneController;
		private GameSceneController gamecSceneController;
		private EGameState curState;
		private EGameState oldState;

		private Vector3 startPosition;
		// <<

		private void Start()
		{
			gameSceneController = GameObject.Find("GameSceneController");
			gamecSceneController = gameSceneController.GetComponent<GameSceneController>();
        }
		private void StateCheck()
		{
			curState = gamecSceneController.CurState;
		}
        private bool IsStateChanged()
		{
			bool isChanged = false;

			if (oldState != curState)
				isChanged = true;

			oldState = curState;

			return isChanged;
		}
		private void OnDisable()
		{
			ResetObstacle();
		}
		public void ReturnToPool()
		{
			Pool.Release(this);
		}
        public void ReturnToPool(int fillValue)
        {
            CUIEventPanel eventPanel = UIManager.Instance.CurSceneUI as CUIEventPanel;

            Pool.Release(this);
            if (eventPanel != null)
				((CUIEventPanel)UIManager.Instance.CurSceneUI).progressBar.FillProgressBar(fillValue);
        }
        public void ResetObstacle()
		{

		}
	}
}