using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */
    public GameObject [] bomb_display; // reference to Gameobject that display player bomb amount
    private int current_bomb = 0; // Amout of bomb that the player have
    private Renderer Bomb_renderer; //renderer of the Bomb gameObject
    private Material Bomb_material; // material of the Bomb gameObject
    private Color Bomb_color;
    /*
    *
    * ATTRIBUTE
    *
    */
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    ///     Method that verify if the player can use Bomb
    /// </summary>
    /// <returns>True : he can use / False otherwise</returns>
    public bool CanUse(){
        return (current_bomb > 0);
    }

    /// <summary>
    ///     Bomb use
    /// </summary>
    public void UseBomb(){
        if(current_bomb > 0){
            current_bomb--;
            getBombCarac();
            Bomb_color.a = 1f;
            Bomb_material.color = Bomb_color;
        }
    }

    /// <summary>
    ///     Increase Bomb amo
    /// </summary>
    public void gainBomb(){
        if(current_bomb < 5){
            getBombCarac();
            Bomb_color.a = 2;
            Bomb_material.color = Bomb_color;
            current_bomb++;
        }
    }
    
    /// <summary>
    ///     Method which obtain bomb sprite caracs
    /// </summary>
    void getBombCarac(){
        Bomb_renderer = bomb_display[current_bomb].GetComponent<Renderer>();
        Bomb_material = Bomb_renderer.material;
        Bomb_color = Bomb_material.color;
    }
}
