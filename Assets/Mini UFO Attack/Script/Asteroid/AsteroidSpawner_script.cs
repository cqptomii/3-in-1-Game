using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    public GameObject [] Asteroid_prefab; // references to differents asteroid prefab
    protected Camera main_camera;
    protected Vector2 screen_bounds;
    protected float Spawn_interval = 0.8f;
    protected float Spawn_pos;
    protected float last_Spawn_pos;
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
        screen_bounds = main_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, main_camera.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        Spawn_interval -= Time.deltaTime;

        // if timer reach 0 then we instantiate another projectile on the screen
        if(Spawn_interval <= 0){
            GameObject newAsteroid;
            newAsteroid = Instantiate(Asteroid_prefab[Random.Range(0,Asteroid_prefab.Length-1)]);
            
            Spawn_pos = (float)Random.Range(-screen_bounds.y,screen_bounds.y);
            if(Spawn_pos == last_Spawn_pos){
                 Spawn_pos = (float)Random.Range(-screen_bounds.y,screen_bounds.y);
            }
            newAsteroid.transform.position = new Vector3 ( screen_bounds.x + 1,Spawn_pos, 10);

            Spawn_interval = 0.8f;
            last_Spawn_pos = Spawn_pos;
        }
    }
    
}
