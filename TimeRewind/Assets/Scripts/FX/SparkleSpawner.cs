using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleSpawner : MonoBehaviour
{
    public GameObject sparkleFX;
    public Transform staffTip;

    public void SpawnSparkle()
    {
        Instantiate(sparkleFX, staffTip.position, Quaternion.identity);
    }
}
