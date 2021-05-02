using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustSpawner : MonoBehaviour
{
    public GameObject dust;

    public void SpawnDust()
    {
        Instantiate(dust, transform.position, Quaternion.identity);
    }
}
