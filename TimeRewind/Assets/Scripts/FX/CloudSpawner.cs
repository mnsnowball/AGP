using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public float spawnDelay = 2.5f;
    public float maxHeight = 3f;
    public float minHeight = -3f;
    public float cloudSpeed = 2f;
    public List<GameObject> cloudPrefabs;
    public float scaleRandomness;

    bool canSpawn = true;

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnCloud());
        }
    }

    IEnumerator SpawnCloud(){
        canSpawn = false;
        int index = Random.Range(0, cloudPrefabs.Count);
        float heightModifier = Random.Range(minHeight, maxHeight);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + heightModifier, transform.position.z);
        Cloud spawned = Instantiate(cloudPrefabs[index], spawnPos, Quaternion.identity).GetComponent<Cloud>();
        spawned.transform.eulerAngles = new Vector3(-90f, 0, -90f);
        spawned.moveSpeed = Random.Range(1f, cloudSpeed);

        float scaleMultiplierX = Random.Range(1, scaleRandomness);
        float scaleMultiplierY = Random.Range(1, scaleRandomness);
        float scaleMultiplierZ = Random.Range(1, scaleRandomness);
        transform.localScale = new Vector3(transform.localScale.x * scaleMultiplierX, transform.localScale.y * scaleMultiplierY, transform.localScale.z * scaleMultiplierZ);

        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
