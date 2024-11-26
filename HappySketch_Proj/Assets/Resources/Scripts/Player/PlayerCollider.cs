using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JongJin
{
    public class PlayerCollider : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        void Update()
        {
            Move();
        }

        void Move()
        {
            this.transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }
}

