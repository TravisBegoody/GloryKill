using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    /// <summary>
    /// When the GameObject should be destroyed in seconds
    /// </summary>
    [SerializeField]
    public float DestroyInTime;

    void Start()
    {
        Destroy(this.gameObject, DestroyInTime);
    }
}
