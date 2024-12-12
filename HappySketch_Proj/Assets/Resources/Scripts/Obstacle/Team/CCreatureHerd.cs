using HakSeung;
using JongJin;
using UnityEngine;
using UnityEngine.Pool;

namespace MyeongJin
{
	public abstract class CCreatureHerd : MonoBehaviour
	{
		public IObjectPool<CCreatureHerd> Pool { get; set; }

		private GameObject gameSceneController;
		private GameSceneController gamecSceneController;
		private readonly EGameState curState = EGameState.FIRSTMISSION;

		private void Start()
		{
			gameSceneController = GameObject.Find("GameSceneController");
			gamecSceneController = gameSceneController.GetComponent<GameSceneController>();
		}
		protected bool IsStateChanged()
		{
			if(gamecSceneController == null && gameSceneController == null)
			{
                gameSceneController = GameObject.Find("GameSceneController");
                gamecSceneController = gameSceneController.GetComponent<GameSceneController>();
            }
			return curState != gamecSceneController.CurState;
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
	}
}