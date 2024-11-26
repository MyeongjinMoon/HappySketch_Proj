using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager sInstance;
        public static GameManager Instance 
        {
            get 
            {
                if (sInstance == null) 
                {
                    GameObject newManagersObject = new GameObject("@GameManager");
                    sInstance = newManagersObject.AddComponent<GameManager>();
                }
                return sInstance;
            }
        }

        private void Awake() 
        {
            if (sInstance != null && sInstance != this) 
            {
                Destroy(this.gameObject);
                return;
            }
            sInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update() 
        {

        }
    }
}
