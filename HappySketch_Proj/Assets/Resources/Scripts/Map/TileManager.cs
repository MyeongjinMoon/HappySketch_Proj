using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace JaeHoon
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] tilePrefabs;
        [SerializeField] private float tileRespawnPoint = 0.0f;
        [SerializeField] private float tileLength = 16.0f;
        [SerializeField] private int numberOfTiles = 3;
        [SerializeField] private Transform firstPlayerTransform;
        [SerializeField] private Transform secondPlayerTransform;
        private List<GameObject> activeTiles = new List<GameObject>();
        private float preventSinkhole = 135.0f;                 // 타일이 옮겨지기 전 player가 떨어짐을 막기 위함

        private void Start()
        {
            for (int index = 0; index < numberOfTiles; index++)
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
            }
        }

        private void Update()
        {
            if (firstPlayerTransform.position.z - preventSinkhole > tileRespawnPoint - (numberOfTiles * tileLength)
                || secondPlayerTransform.position.z - preventSinkhole > tileRespawnPoint - (numberOfTiles * tileLength))                    // 플레이어 위치를 기준으로 타일을 추가 생성하고 오래된 타일을 재배치
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
                RepositionTile();                                   // 타일 재배치
            }
        }

        private void SpawnTile(int tileIndex)
        {
            GameObject playerRun = Instantiate(tilePrefabs[tileIndex], transform.forward * tileRespawnPoint, transform.rotation);
            activeTiles.Add(playerRun);
            tileRespawnPoint += tileLength;
        }

        private void RepositionTile()
        {
            GameObject oldestTile = activeTiles[0];         // 가장 오래된 타일을 재사용하여 앞으로 이동
            activeTiles.RemoveAt(0);

            oldestTile.transform.position = transform.forward * tileRespawnPoint;               // 타일을 새 위치로 재배치
            tileRespawnPoint += tileLength;

            activeTiles.Add(oldestTile);                 // 재배치된 타일을 리스트의 끝에 추가
        }
    }
}
