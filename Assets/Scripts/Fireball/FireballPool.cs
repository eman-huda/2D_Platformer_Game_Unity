using UnityEngine;
using System.Collections.Generic;

public class FireballPool : MonoBehaviour
{
    public static FireballPool Instance;

    public GameObject fireballPrefab;
    public int poolSize = 10;

    private List<GameObject> fireballPool = new List<GameObject>();

    void Awake()
    {
        Instance = this; // Make it globally accessible
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(fireballPrefab);
            obj.SetActive(false);
            obj.GetComponent<Projectile>().movementSpeed = 10;
            fireballPool.Add(obj);
        }
    }

    public GameObject GetFireball()
    {
        foreach (GameObject fireball in fireballPool)
        {
            if (!fireball.activeInHierarchy)
            {
                return fireball;
            }
        }

        GameObject newFireball = Instantiate(fireballPrefab);
        newFireball.SetActive(false);
        fireballPool.Add(newFireball);
        return newFireball;
    }
}
