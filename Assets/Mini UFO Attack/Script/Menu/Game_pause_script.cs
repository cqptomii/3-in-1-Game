using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_pause_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE   
    *
    */
    private Canvas current_canvas;
    public AudioClip [] ref_Game_AudioClip; // reference to game pause sound and main game music
    private AudioSource ref_gamePause_Audiosource; // reference to the audiosource gameObject
    private bool Game_state = true;
    private bool Opening = false; // booleen to verify the opening of the menu
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        ref_gamePause_Audiosource = GetComponent<AudioSource>();
        ref_gamePause_Audiosource.volume = 0.25f;

        Start_Game_music();

        current_canvas = GetComponent<Canvas>();
        current_canvas.planeDistance = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///     Start Main game Music
    /// </summary>
    public void Start_Game_music(){
        ref_gamePause_Audiosource.clip = ref_Game_AudioClip[0];
        ref_gamePause_Audiosource.loop = true;
        ref_gamePause_Audiosource.Play();
    }

    /// <summary>
    ///     Method to make au pause in the game
    /// </summary>
    public void Game_Pause(){
        Game_state = false;
        StartCoroutine(WaitTime(0.2f));
        ref_gamePause_Audiosource.clip = ref_Game_AudioClip[1];
        ref_gamePause_Audiosource.loop = false;
        ref_gamePause_Audiosource.Play();
        current_canvas.planeDistance = 1;
    }

    /// <summary>
    ///     Method to unpause the game
    /// </summary>
    public void Game_Continue(){
        Time.timeScale = 1f;
        Start_Game_music();
        Game_state = true;
        StartCoroutine(WaitTime(0.5f));
        current_canvas.planeDistance = 100;
    }
    

    /// <summary>
    ///     Method to Quit the game
    /// </summary>
    public void Game_shutDown(){
        Application.Quit();
    }

    /// <summary>
    ///     Method to Load the main Menu of our game
    /// </summary>
    public void Load_Menu(){
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    /// <summary>
    ///     Method to get the state of the UFO game
    /// </summary>
    /// <returns>true: unpause / False : Pause</returns>
    public bool Get_State(){
        return Game_state;
    }

    /// <summary>
    ///     Method to know if we are opening the menu
    /// </summary>
    /// <returns> True : opening / False : not</returns>
    public bool Get_Opening(){
        return Opening;
    }

    /*
    *
    *   COROUTINE
    *
    */


    /// <summary>
    ///     Method which wait an amount of time
    ///     Use when the menu is open to avoid repeated opening
    /// </summary>
    /// <param name="seconds"> time wait in seconds</param>
    /// <returns></returns>
    private IEnumerator WaitTime(float seconds){
        float time = 0;
        Opening = true;
        while(time < seconds){
            yield return new WaitForSeconds(0.1f);
            time += 0.1f;
        }
        Opening = false;
        if(seconds == 0.2f){
            Time.timeScale = 0;
        }
        
    }
}
