using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, iItems
{
    public static event Action<int> OnCoinCollect;
    public int worth = 5;
 

    public void Collect()
    {
        
        OnCoinCollect?.Invoke(worth);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Coin OnTriggerEnter: " + gameObject.name + ", Frame: " + Time.frameCount);
            Collect();
        }
    }
}