using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Spawner_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */

    //prefab references
    public GameObject Apple_prefab; // reference to apple prefab
    public GameObject Bomb_prefab; // reference to bomb prefab
    public GameObject Gold_prefab; // reference to golden apple prefab
    public GameObject Rotten_prefab; // reference to rotten apple prefab

    // objects references
    public AudioClip back_sound;   // reference to background audioClip
    public SpriteRenderer fader_renderer; // reference to the fade sprite
    public TextMeshPro timer; // reference to timer TextBox
    public GameOVER ref_gameOver; // reference to GameOver script

    // Spawner Attributes
    protected float time = 90f; // Timer
    public float currentTime; // Variable to manage current time left
    protected float Spawn_interval = 1f; // interval between two apple spawn
    protected float spawnPos = 0.0f; // initial spawn position
    protected AudioSource background_music; // reference to the Spawner AudioSource
    protected float current_alpha = 1f; // current transparency of the Fader
    private int appleCount = 0; //counter to know how many apples have fallen
    private int applesUntilOther; //counter to know how mant apples have fallen must stille fall before another projectile
    /*
    *
    * 
    *
    */


    // Start is called before the first frame update
    void Start()
    {
        //Background music settings
        background_music = gameObject.AddComponent<AudioSource>();
        background_music.loop = true;
        background_music.volume = 0.05f;
        background_music.clip = back_sound;

        //Set initial apple count until special projectile
        applesUntilOther = Random.Range(3, 7);

        currentTime = time;
        //Start timer coroutine
        StartCoroutine(StartCountDown());

        // Call Sprite Fade Coroutine
        StartCoroutine( FadeOutFromWhite() );
        
    }

    // Update is called once per frame
    void Update()
    {
        Spawn_interval -= Time.deltaTime;
        GameObject objectToSPawn = Apple_prefab;
        // if timer reach 0 then we instantiate another projectile on the screen
        if(Spawn_interval <= 0 && currentTime >0){
            // Spawn a special apple 
            if (appleCount >= applesUntilOther)
            {
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    objectToSPawn = Rotten_prefab;
                }
                else if (rand == 1)
                {
                    objectToSPawn = Gold_prefab;
                }
                else if (rand == 2)
                {
                    objectToSPawn = Bomb_prefab;
                }
                appleCount = 0;
                //Set initial apple count until the next projectile
                applesUntilOther = Random.Range(3, 7);
            }
            else
            {
                objectToSPawn = Apple_prefab;
            }

            spawnPos = (float)Random.Range(-8.5f, 8.5f);
            objectToSPawn.transform.position = new Vector3(spawnPos, 6.0f, -1f);
            Instantiate(objectToSPawn);
            appleCount++;
            Spawn_interval = 1f - (time-currentTime)/200;
        }
    }

    /// <summary>
    ///     Method to get the current time left in the timer
    /// </summary>
    /// <returns></returns>
    public float GetCurrentTime(){
        return currentTime;
    }

    /*
    *
    *   Coroutine
    *
    */

    /// <summary>
    ///     Coroutine to fade out from white/launch music with a delay
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutFromWhite()
    {
        yield return new WaitForSeconds(0.5f);

        background_music.Play();

        while (current_alpha > 0)
        {
            current_alpha -= Time.deltaTime / 2;
            fader_renderer.color = new Color(1, 1, 1, current_alpha);
            yield return null;
        }

        Destroy(fader_renderer.gameObject);

    }

    /// <summary>
    ///     timer Coroutine which modify the color of the timer when it last 10 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCountDown(){
        // decrease timer
        while(currentTime > 0){
            
            // set the color in red if  there is 10 seconds left
            if(currentTime <= 10){
                timer.color = Color.red;
            }
            if(currentTime%60 <10){
                timer.SetText(Mathf.FloorToInt(currentTime / 60) + ":0" + Mathf.FloorToInt(currentTime%60) + " Left");
            }else{
                timer.SetText(Mathf.FloorToInt(currentTime / 60) + ":" + Mathf.FloorToInt(currentTime%60) + " Left");
            }
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
        
        //When timer reach 0 show GameOver Screen
        ref_gameOver.ShowOverScreen();
    }
}
