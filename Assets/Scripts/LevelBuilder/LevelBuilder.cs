using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform enemySpawn;
    [SerializeField] private Transform playerSpawn;

    public static LevelBuilder Instance;

    private IDamage damagedObject;
    private void Awake()
    {
        Instance = this;
        Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        Instantiate(npcPrefab, enemySpawn.position, Quaternion.identity);
    }

    public void RespawnObject(GameObject spawned)
    {
        damagedObject = spawned.GetComponent<IDamage>();
        damagedObject.Die();
        if (!spawned.CompareTag("Player"))
        {
            spawned.SetActive(false);
        }
        
        StartCoroutine(WaitRespawn(spawned));
    }

    private IEnumerator WaitRespawn(GameObject spawned)
    {
        yield return new WaitForSeconds(5);
        damagedObject.Respawned();
        if (spawned.CompareTag("Player"))
        {
            spawned.transform.position = playerSpawn.position;
        }
        else
        {
            spawned.transform.position = enemySpawn.position;
        }
        
    }
}
