using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    public GameObject [] health_display; // reference to health display
    public Player_script ref_player_script; // reference to the player script
    private int current_Player_health; // Variable to store the current palyer health
    private int last_health = 10; // Variable to store the last health of the player
    private int health_difference; // Variable to store health difference between last and current health
    private int health_index = 9; // Store Health display index start
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Get the health modulo 10;
        current_Player_health = ref_player_script.GetInt_Health() / 10;
        health_difference = last_health - current_Player_health;
        // Player lose health
        if(health_difference > 0 ){
            //Change local Scale of health display
            for(int i = 0;i < health_difference; i ++){
                health_display[health_index].transform.localScale = new Vector3(0.3f,0.3f,1f);
                health_index--;
            }
            last_health = current_Player_health;
        } // Player gain health
        else if(health_difference < 0){
            //Change local Scale of health display
            for(int i = 0;i > health_difference; i --){
                health_display[health_index].transform.localScale = new Vector3(1f,1f,1f);
                health_index++;
            }
            last_health = current_Player_health;
        }
        }
}
