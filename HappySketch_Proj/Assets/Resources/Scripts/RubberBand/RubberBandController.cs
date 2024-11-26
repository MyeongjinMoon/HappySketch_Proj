using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public class RubberBandController : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] private GameObject[] players;
        [SerializeField] private GameObject dinosaur;

        [Header("Distance")]
        [SerializeField] private float totalRunningDistance = 100.0f;
        [SerializeField] private float minDistance = 5.0f;
        [SerializeField] private float maxDistance = 15.0f;

        [Header("ProgressRate")]
        [SerializeField] private float tailMissionStartRate = 15.0f;
        // TODO<이종진> - 진행률에 따른 미션 이름 수정 필요 - 20241110
        [SerializeField] private float firstMissionRate = 35.0f;
        [SerializeField] private float secondMissionRate = 55.0f;
        [SerializeField] private float thirdMissionRate = 80.0f;


        private bool isPossibleTailMission = false;

        private float[] playerDistance = { 0.0f, 0.0f, 0.0f, 0.0f };
        private float firstRankerDistance = 0.0f;
        private float lastRankerDistance = 0.0f;

        public float ProgressRate { get { return lastRankerDistance / totalRunningDistance * 100.0f; } }

        public float DinosaurSpeed { get { return dinosaurSpeed; } }
        private float dinosaurSpeed = 2.0f;

        private void Start()
        {
            dinosaurSpeed = dinosaur.GetComponent<DinosaurController>().Speed;
        }

        private void Update()
        {
            if (!isPossibleTailMission && ProgressRate > tailMissionStartRate)
                isPossibleTailMission = true;
            Move();
            CalculatePlayerDistance();
            CalculateRank();
        }

        private void Move()
        {
            transform.position = dinosaur.transform.forward * firstRankerDistance;
        }
        private void CalculatePlayerDistance()
        {
            for (int playerNum = 0; playerNum < players.Length; playerNum++)
                playerDistance[playerNum] = players[playerNum].transform.position.z;
        }
        private void CalculateRank()
        {
            firstRankerDistance = playerDistance[0];
            lastRankerDistance = playerDistance[0];

            for (int playerNum = 1; playerNum < players.Length; playerNum++)
            {
                if (playerDistance[playerNum] > firstRankerDistance) firstRankerDistance = playerDistance[playerNum];
                if (playerDistance[playerNum] < lastRankerDistance) lastRankerDistance = playerDistance[playerNum];
            }
        }

        public bool IsBeyondMaxDistance(Vector3 position)
        {
            if (position.z - dinosaur.transform.position.z > maxDistance)
                return false;
            return true;
        }

        public bool IsUnderMinDistance(Vector3 position, out Vector3 curPos)
        {
            curPos = new Vector3(position.x, position.y, dinosaur.transform.position.z + minDistance);
            if (position.z - dinosaur.transform.position.z < minDistance)
                return true;
            return false;
        }
    }
}
