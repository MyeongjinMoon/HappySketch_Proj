using System.Collections;
using System.Collections.Generic;
using JongJin;
using UnityEngine;

namespace JaeHoon
{
    public class MissionRoomController : MonoBehaviour
    {
        [SerializeField] private RunningState runningState;
        private Transform missionGround_1;

        void Start()
        {
            runningState = GetComponent<RunningState>();
            missionGround_1 = GameObject.Find("MissionStages").transform.Find("MissionGround_1");
        }

        void Update()
        {
            if (!runningState.IsFirstMissionTriggered())
            {
                missionGround_1.gameObject.SetActive(false);
            }
            if (runningState.IsFirstMissionTriggered())
            {
                GameObject.Find("MissionStages").transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
