using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ennemy_spawner_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */
    public GameObject [] ref_ennemy_prefab; // reference to all ennemy prefab

    private Camera current_camera; //  reference to the scene camera
    private Vector3 Screen_bounds; // Viewing frustrum of the camera
    private float spawn_interval = 3f; // time between ennemy spawning
    private Vector3 SpawnPosition; // coordinate of ennemy spawning
    private int time_since_start = 0; // time since gamestart, for set the game difficulty
    private int Difficulty_level = 0; // State of difficulty of the game, which will change in function of the time since start 
    private int ennemy_in_screen = 0; // number of ennemy in the screen
    private int max_ennemy_in_screen = 5; // Max ennemy who can be in the screen at the same time
    private float UI_POS = 3.7f; // y position of the Top UI

    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        current_camera = Camera.main;
        Screen_bounds = current_camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,current_camera.transform.position.z)); 
        
        SpawnPosition.x = Screen_bounds.x + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager_script.Instance.Game_IsRunning()){
            if(ennemy_in_screen < max_ennemy_in_screen){
                spawn_interval -= Time.deltaTime;
                if(spawn_interval <= 0){
                    Add_Ennemy();
                    // Reset spawn_interval
                    if(Difficulty_level == 1){
                        spawn_interval = 2f;
                    }else if(Difficulty_level == 2){
                        spawn_interval = 1f;
                    }else if(Difficulty_level == 3){
                        spawn_interval = 0.5f;
                    }else{
                        spawn_interval = 3f;
                    }
                }
            }
        } 
    }

    /// <summary>
    ///     Decrease ennemy amount in the screen
    /// </summary>
    public void ennemy_Death(){
        ennemy_in_screen--;
    }

    /// <summary>
    ///     Add one random ennemy in the screen
    /// </summary>
    void Add_Ennemy(){
        //Spawn Random ennemy on random y coordinate
        Instantiate(ref_ennemy_prefab[Random.Range(0,ref_ennemy_prefab.Length)],
        new Vector3(Screen_bounds.x +1,Random.Range(-Screen_bounds.y+1,UI_POS-1),0f),
        Quaternion.identity);
        ennemy_in_screen++;
    }

    /*
    *
    * Coroutine
    *
    */

    /// <summary>
    ///     Fix the difficulty of the game with the time last since the game start
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fix_GameDifficulty(){
        while(time_since_start < 200){
            if(time_since_start == 60){
                Difficulty_level = 1;
                max_ennemy_in_screen = 10;
            }else if(time_since_start ==120){
                Difficulty_level = 2;
                max_ennemy_in_screen = 15;
            }
            yield return new WaitForSeconds(1f);
            time_since_start++;
        }
        Difficulty_level = 3;
    }
}
