using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBullet : MonoBehaviour
{
    private static PoolBullet instance;
    public static PoolBullet Instance { get { return instance; } }

    public GameObject bulletPrefab;

    public List<GameObject> bullets = new List<GameObject>();

    public int bulletAmount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnBullet();
    }

    public void SpawnBullet()
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bulletSpawn = Instantiate(bulletPrefab);

            bulletSpawn.SetActive(false);

            bulletSpawn.transform.SetParent(transform);

            bullets.Add(bulletSpawn);
        }
    }

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
        }

        return null;
    }

    
}
