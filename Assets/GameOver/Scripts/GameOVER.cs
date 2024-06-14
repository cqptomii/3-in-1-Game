using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOVER : MonoBehaviour
{
    /**
    *
    *ATTRIBUTE
    *
    */
    public TextMeshProUGUI scoreBox; // Score Text Box
    public Bin_script refApplePlayer_script; // reference to Player script in Apple Catcher Game
    public ball_script refBrickBreaker_script; // reference to ball script in BrickBreaker
    public GameOVER ref_GameOver; // reference to himself


    protected Canvas current_canvas; // reference to our canvas
    protected Scene current_scene; // var which contain the current scene
    private AudioSource ref_audiosource; // reference to gameOver audiosource

    /**
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        ref_audiosource = GetComponent<AudioSource>();
        current_canvas = GetComponent<Canvas>();
        current_canvas.planeDistance = 100;

        ref_audiosource.loop = false;
        ref_audiosource.volume = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOverScreen(){

        current_canvas.planeDistance = 1;
        current_scene = SceneManager.GetActiveScene();
        
        if(current_scene.name == "BrickGame"){
            scoreBox.SetText("Score:" + refBrickBreaker_script.getScore());
            refBrickBreaker_script.resetVelocity();
        }else if(current_scene.name == "AppleGame"){
            scoreBox.SetText("Score :" + refApplePlayer_script.GetScore());
        }else{

        }
    }
    public void ReloadScene(){
        SceneManager.LoadScene(current_scene.name);
    }
    public void QuitGame(){
        SceneManager.LoadScene("Menu");
    }
}
