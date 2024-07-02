using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    public GameObject [] health_display; // reference to health display
    public TextMeshPro ref_ScoreBox;
    public GameObject ref_double_pointText;
    public GameObject ref_player_shield; // reference to the player shield sprite
    public GameObject ref_invincible_Text; // reference to the Text Box to show player invincibility
    public Ennemy_spawner_script ref_ennemy_spawner;
    public GameObject ref_Wiki; // reference to the wiki


    // Main ATTRIBUTES
    private bool game_running;
    private float player_health = 100f;
    private int current_score = 0;
    private int currentInt_player_health; // player health converted in INT
    private int last_health = 10; // Variable to store the last health of the player
    private int health_difference; // Variable to store health difference between last and current health
    private int health_index = 9; // Store Health display index start

    //Var Player active bonus
    private bool double_score_point = false;
    private bool haveShield = false;
    private bool is_Invincible = false;



    private static GameManager_script _instance;
    /*
    *
    *
    *
    */
    public static GameManager_script Instance
    {
        get{ 
            return _instance;
        }
    }
    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        game_running = true;
    }

    // Update is called once per frame
    void Update()
    {
        Update_health();    
    }

    /// <summary>
    ///     Method which Update differentes Health UI with the current player health
    /// </summary>
    void Update_health(){
        // Get the health modulo 10;
        currentInt_player_health = GetInt_Health() / 10;
        health_difference = last_health - currentInt_player_health;
        // Player lose health
        if(health_difference > 0 ){
            //Change local Scale of health display
            for(int i = 0;i < health_difference; i ++){
                health_display[health_index].transform.localScale = new Vector3(0.3f,0.3f,1f);
                health_index--;
            }
            last_health = currentInt_player_health;
        } // Player gain health
        else if(health_difference < 0){
            //Change local Scale of health display
            for(int i = 0;i > health_difference; i --){
                health_display[health_index].transform.localScale = new Vector3(1f,1f,1f);
                health_index++;
            }
            last_health = currentInt_player_health;
        }
        if(player_health <= 0 ){
            game_running = false;
            ref_Wiki.SetActive(false);
        }
    }

    /// <summary>
    ///     This method provide current player rounded health 
    /// </summary>
    public int GetInt_Health(){
        return Mathf.RoundToInt(player_health);
    }

    /// <summary>
    ///     This method decrease an amount of Player health 
    /// </summary>
    /// <param name="amount"> amount of health decreased</param>
    public void Decrease_health(float amount){
        player_health -= amount;
        player_health = Mathf.Clamp(player_health,0,100);
    }

    /// <summary>
    ///     This method restore an amount of Player health 
    /// </summary>
    /// <param name="amount"> amount of health gained</param>
    public void Gain_health(float amount){
        player_health += amount;
        player_health = Mathf.Clamp(player_health,0,100);
    }
    
    /// <summary>
    ///     Method that look if the player is dead or not
    /// </summary>
    /// <returns> True : Dead / False otherwise</returns>
    public bool Is_Alive(){
        if(player_health <= 0 ){
            return false;
        }else{
            return true;
        }
    }

    /// <summary>
    ///     Update Score Point UI with a given amount
    /// </summary>
    /// <param name="amount"> amount of point to add to the score</param>
    public void Update_ScorePoint(int amount){
        if(double_score_point){
            StartCoroutine(Score_Increase(current_score+2*amount));
        }else{
            StartCoroutine(Score_Increase(current_score+amount));
        }
    }

    /// <summary>
    /// Methode to get the current score of the player
    /// </summary>
    /// <returns>current_score</returns>
    public int GetScore(){
        return current_score;
    }

    /// <summary>
    ///     Method that start the double point Coroutine
    /// </summary>
    public void Double_pointStart(){
        ref_double_pointText.SetActive(true);
        double_score_point = true;
        StartCoroutine(Start_timer(30,1));
    }

    /// <summary>
    ///     Stop the Double point
    /// </summary>
    void Double_pointStop(){
        ref_double_pointText.SetActive(false);
        double_score_point = false;
    }

    /// <summary>
    ///     Start the invincibilty of the player
    /// </summary>    
    public void Invicibility_Start(){
        ref_invincible_Text.SetActive(true);
        is_Invincible = true;
        StartCoroutine(Start_timer(10,3));
    }

    /// <summary>
    ///     Stop the invincibility of the player
    /// </summary>
    void Invicibility_Stop(){
        ref_invincible_Text.SetActive(false);
        is_Invincible = false;
    }

    /// <summary>
    ///     Start the shiel on the player
    /// </summary>
    public void ShieldStart(){
        ref_player_shield.SetActive(true);
        haveShield = true;
        StartCoroutine(Start_timer(10,2));
    }

    /// <summary>
    ///     Stop the shield 
    /// </summary>
    public void ShieldStop(){
        ref_player_shield.SetActive(false);
        haveShield = false;
    }

    /// <summary>
    ///     Get the state of the shield Active/Disabled
    /// </summary>
    /// <returns>True / False</returns>
    public bool GetShieldState(){
        return haveShield;
    }

    /// <summary>
    ///     Get the state of the invincibility bonus : Active/Disabled
    /// </summary>
    /// <returns>True / False</returns>
    public bool GetInvicibilityState(){
        return is_Invincible;
    }

    /// <summary>
    ///     Get the state of the game : Active/Disabled
    /// </summary>
    /// <returns>True / False</returns>
    public bool Game_IsRunning(){
        return game_running;
    }

    /// <summary>
    ///     Decrease the amount of ennemy in the spawner
    /// </summary>
    public void Decrease_Ennemy(){
        ref_ennemy_spawner.ennemy_Death();
    }
    /*
    *
    * Coroutine
    *
    */

    /// <summary>
    ///     Increase the score of the player with a small pause
    /// </summary>
    /// <param name="target"> score that we target</param>
    /// <returns></returns>
    private IEnumerator Score_Increase(int target){
        while (current_score < target){
            current_score += 10;
            ref_ScoreBox.SetText(current_score.ToString());
            yield return new WaitForSeconds(0.005f);
        }
    }

    /// <summary>
    ///     Method that provide a timer for active bonus like shield or invincibility
    /// </summary>
    /// <param name="seconds"> amount of time</param>
    /// <param name="bonus_index">bonus type</param>
    /// <returns></returns>
    private IEnumerator Start_timer(int seconds,int bonus_index)
    {
        while(seconds > 0){
            yield return new WaitForSeconds(1f);
            seconds--;
        }
        if(bonus_index == 1){
            Double_pointStop();
        }else if(bonus_index == 2){
            ShieldStop();
        }else if(bonus_index == 3){
            Invicibility_Stop();
        }

    }
}
