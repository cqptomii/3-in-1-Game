using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiki_script : MonoBehaviour
{
    
    /*
    *
    * ATTRIBUTES
    *
    */
        public GameObject ref_fader; // reference to the fader Gameobject in the wiki canvas
        public GameObject ref_Container; // reference to wiki Container
        private bool wiki_state = false;
    /*
    *
    *
    *
    */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///     Method that show the wiki on the screen
    /// </summary>
    public void show_wiki(){
        wiki_state = true;
        ref_fader.SetActive(true);
        ref_Container.SetActive(true);
        Time.timeScale = 0;
    }
    
    /// <summary>
    /// method that hide the wiki on the screen
    /// </summary>
    public void hide_wiki(){
        wiki_state = false;
        ref_fader.SetActive(false);
        ref_Container.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    ///     Method that return the state of the game
    /// </summary>
    /// <returns> True / false</returns>
    public bool Get_wiki_state(){
        return wiki_state;
    }
}
