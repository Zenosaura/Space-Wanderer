using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{   
    int progress;
    public Slider progressSlider;
    public int level = 1;
    public GameObject player;
    public GameObject spawn2;
    public GameObject spawn3;
    public GameObject spawn4;
    

    private bool levelCompleted = false;

    public void CompleteLevel()
    {
        Debug.Log("Complete");
        completeLevelUI.SetActive(true);
        if (level == 2)
        {
            completeLevelUI.SetActive(false);
            //teleport to level 2
            player.transform.position = spawn2.transform.position;
            completeLevelUI.SetActive(true);
        }
        if (level == 3)
        {
            completeLevelUI.SetActive(false);
            //teleport to level 3
            player.transform.position = spawn3.transform.position;
            completeLevelUI.SetActive(true);
        }
        if (level == 4)
        {
            completeLevelUI.SetActive(false);
            //teleport to level 4
            player.transform.position = spawn4.transform.position;
            completeLevelUI.SetActive(true);
        }

    }

    public GameObject completeLevelUI;

    // Start is called before the first frame update
    void Start()
    {
       
        progress = 0;
        progressSlider.value = 0;
        Coin.OnCoinCollect += IncreaseProgress;

  
    }

    void IncreaseProgress(int amount){
        progress += amount;
        progressSlider.value = progress;
        if (progress >= 150 && !levelCompleted){
            // Level complete
            level++;
            progress = 0;
            progressSlider.value = 0;
            CompleteLevel(); 
            

        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    
}
