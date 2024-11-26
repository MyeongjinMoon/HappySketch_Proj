using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailContoller : MonoBehaviour
{
    public int CollisionCount { get; private set; }

    private void OnEnable()
    {
        CollisionCount = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider")
            CollisionCount++;
    }
}
