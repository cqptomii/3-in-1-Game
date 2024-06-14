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
    public GameObject Apple_prefab; // reference to apple prefab
    public AudioClip back_sound;   // reference to background audioClip
    public SpriteRenderer fader_renderer; // reference to the fade sprite
    public TextMeshPro timer; // reference to timer TextBox
    public GameOVER ref_gameOver; // reference to GameOver script

    protected float time = 90f; // Timer
    protected float currentTime; // Variable to manage current time left
    protected float Spawn_interval = 3f; // interval between two apple spawn
    protected float spawnPos = 0.0f; // initial spawn position
    protected AudioSource background_music;
    protected float current_alpha = 1f;
    /*
    *
    * 
    *
    */


    // Start is called before the first frame update
    void Start()
    {
        GameObject newApple = Instantiate(Apple_prefab);
        spawnPos = (float)Random.Range(-8.5f,8.5f);
        newApple.transform.position = new Vector3 ( spawnPos,6.0f, 0);
        
        //Background music settings
        background_music = gameObject.AddComponent<AudioSource>();
        background_music.loop = true;
        background_music.volume = 0.05f;
        background_music.clip = back_sound;

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
        // if timer reach 0 then we instantiate another projectile on the screen
        if(Spawn_interval <= 0 && currentTime >0){
            GameObject newApple = Instantiate(Apple_prefab);
            spawnPos = (float)Random.Range(-8.5f,8.5f);
            newApple.transform.position = new Vector3 ( spawnPos,6.0f, 0);


            Spawn_interval = 1f - (time-currentTime)/200;
        }
    }

    public float GetCurrentTime(){
        return currentTime;
    }
    //Coroutine to fade out from white/launch music with a delay
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
    // timer Coroutine
    private IEnumerator StartCountDown(){
        // decrease timer
        while(currentTime > 0){
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
