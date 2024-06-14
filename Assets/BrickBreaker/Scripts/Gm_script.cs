using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gm_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */

    // Variable that store differents wall positions
    public GameObject Left_border;
    public GameObject right_border;
    public GameObject top_border;

    public GameObject brick_prefab; // reference to a brick prefab
    public ball_script ball; // reference to our ball script
    public GameObject[] Spritelife; // references to health sprites
    public GameOVER ref_gameover; // reference to the gameover Screen

    protected int brick_amount = 0; // Amount of brick in the screen
    protected Renderer prefabRenderer; // prefab renderer
    protected int life_left = 3; // amount of life

    //Variable that store extremum value of playable area
    protected float x_max;
    protected float x_min;
    protected float y_max;
    protected int time_from_start = 0;
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        Renderer Topborder_Renderer = top_border.GetComponent<Renderer>();
        Renderer Edgeborder_Renderer = Left_border.GetComponent<Renderer>();
        y_max =top_border.transform.position.y - Topborder_Renderer.bounds.size.y;
        x_min = Left_border.transform.position.x + Edgeborder_Renderer.bounds.size.x/2;
        x_max = right_border.transform.position.x - Edgeborder_Renderer.bounds.size.x/2;

        prefabRenderer = brick_prefab.GetComponent<Renderer>();
        LoadGameBoard(prefabRenderer,x_max,x_min,y_max);
        StartCoroutine(StartTimer());
    }

    // Update is called once per frame
    void Update()
    {
        // Show game Over screen
        if(life_left == 0){
            ref_gameover.ShowOverScreen();
        }
        // Load a new Level
        if(brick_amount <= 0){
            LoadGameBoard(prefabRenderer,x_max,x_min,y_max);
        }
    }

    // Method that fill the screen with 3 line of bricks
    void FullGameBoard(Vector3 prefab_size,float x_max, float x_min, float y_max){
        Vector3 spawnPoint = new Vector3(x_min+ prefab_size.x/2,y_max - prefab_size.y/2,-0.01f);
        for(int i = 0;i < 3;i++){
            while(spawnPoint.x < x_max){
                Instantiate(brick_prefab,spawnPoint,Quaternion.identity);
                brick_amount++;
                spawnPoint.x += prefab_size.x;
            }
            spawnPoint.y -= prefab_size.y;
            spawnPoint.x = x_min + prefab_size.x/2;
        }
    }

    //Method that fill randomly the screen with bricks
    void RandomGameBoard(Vector3 prefab_size,float x_max, float x_min, float y_max){
        int columnAmount =  UnityEngine.Random.Range(4,6);
        Vector3 spawnPoint = new Vector3(x_min+ prefab_size.x/2,y_max - prefab_size.y/2,-0.01f);
        int lineAmount = 0;
        int instant_box = 0;
        for(int i = 0; i <= columnAmount ; i++){
            lineAmount =  UnityEngine.Random.Range(5,10);
            while(lineAmount > 0 && spawnPoint.x < x_max){
                instant_box = UnityEngine.Random.Range(0,3);
                if(instant_box==0){
                    spawnPoint.x += prefab_size.x;
                }else{
                    if(spawnPoint.x < x_max ){
                        Instantiate(brick_prefab,spawnPoint,Quaternion.identity);
                        brick_amount++;
                        spawnPoint.x += prefab_size.x;
                    }
                }
            }
            spawnPoint.y -= prefab_size.y;
            spawnPoint.x = x_min + prefab_size.x/2;
        }
    }
    public void Report_brickDeath(){
        brick_amount --;
    }

    //Method that reload a Level of bricks
    void LoadGameBoard(Renderer prefabRenderer,float x_max, float x_min, float y_max){
        int prefab_choice = UnityEngine.Random.Range(0,1);
    
        if(prefab_choice == 0){
            RandomGameBoard(prefabRenderer.bounds.size,x_max,x_min,y_max);
        }else{
            FullGameBoard(prefabRenderer.bounds.size,x_max,x_min,y_max);
        }
    }

    //Method which decrease player health and hide associated sprite
    public void decrease_heath(){
        life_left--;
        Renderer ob_renderer = Spritelife[life_left].GetComponent<Renderer>();
        ob_renderer.enabled = false;
    }
    
    // Method which return time attribute
    public int getTime(){
        return time_from_start;
    }
    //Method that look if we have life left
    public bool IsDead(){
        if(life_left >0){
            return false;
        }else{
            return true;
        }
    }
    // Coroutine that init a Timer which will set the speed of the ball
    private IEnumerator StartTimer()
    {

        while(time_from_start < 120){
            yield return new WaitForSeconds(1f);
            if(time_from_start %10 == 0){
                ball.UpVelocity();
            }
            time_from_start++;
        }
    }
}
