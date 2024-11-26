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

        // Start is called before the first frame update
        void Start()
        {
            runningState = GetComponent<RunningState>();
            missionGround_1 = GameObject.Find("MissionStages").transform.Find("MissionGround_1");
        }

        // Update is called once per frame
        void Update()
        { 
            //if (Input.GetMouseButtonDown(0))     // ���콺 ���� Ŭ��
            if (!runningState.IsFirstMissionTriggered())
            {
                missionGround_1.gameObject.SetActive(false);
            }
            //if (Input.GetMouseButtonDown(1))     // ���콺 ������ Ŭ��
            if (runningState.IsFirstMissionTriggered())
            {
                GameObject.Find("MissionStages").transform.GetChild(0).gameObject.SetActive(true);         // MissionStages�� ù ��° �ڽ� 
            }
        }
    }
}
