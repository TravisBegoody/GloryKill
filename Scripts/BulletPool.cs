using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public List<GameObject> Pool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        for (int i = 0; i < 250; i++)
        {
            GameObject temp = Instantiate(new GameObject("Bullet"),this.transform);
            temp.SetActive(false);

            Pool.Add(temp);
        }
    }
    //Find Unused bullet and returns it
    public GameObject UnusedBullet()
    {
        GameObject UnusuedBullet = Pool.First(x => x != null);
        return UnusuedBullet;
    }
}
