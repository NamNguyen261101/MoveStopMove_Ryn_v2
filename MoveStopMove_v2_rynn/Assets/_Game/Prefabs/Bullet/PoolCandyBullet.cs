using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCandyBullet : MonoBehaviour
{
    private static PoolCandyBullet instance;
    public static PoolCandyBullet Instance { get { return instance; } }

    public GameObject bulletPrefab;

    public List<GameObject> bullets = new List<GameObject>();

    public int bulletAmount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       
            for (int i = 0; i < bulletAmount; i++)
            {
                GameObject bulletSpawn = Instantiate(bulletPrefab);

                bulletSpawn.transform.SetParent(transform);

                bulletSpawn.SetActive(false);

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
