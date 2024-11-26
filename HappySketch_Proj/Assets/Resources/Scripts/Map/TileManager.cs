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
        private float preventSinkhole = 135.0f;                 // Ÿ���� �Ű����� �� player�� �������� ���� ����

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
                || secondPlayerTransform.position.z - preventSinkhole > tileRespawnPoint - (numberOfTiles * tileLength))                    // �÷��̾� ��ġ�� �������� Ÿ���� �߰� �����ϰ� ������ Ÿ���� ���ġ
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
                RepositionTile();                                   // Ÿ�� ���ġ
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
            GameObject oldestTile = activeTiles[0];         // ���� ������ Ÿ���� �����Ͽ� ������ �̵�
            activeTiles.RemoveAt(0);

            oldestTile.transform.position = transform.forward * tileRespawnPoint;               // Ÿ���� �� ��ġ�� ���ġ
            tileRespawnPoint += tileLength;

            activeTiles.Add(oldestTile);                 // ���ġ�� Ÿ���� ����Ʈ�� ���� �߰�
        }
    }
}
