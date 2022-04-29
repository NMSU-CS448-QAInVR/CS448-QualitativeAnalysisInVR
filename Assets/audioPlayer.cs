using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayer : MonoBehaviour
{

    public AudioSource audioSource;
    

    bool isPlaying = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

       public void playMusic(){
        if(isPlaying){
            audioSource.Pause();
            isPlaying = false;
        }
        else{
        audioSource.Play();
        isPlaying = true;
        }
    }


}


