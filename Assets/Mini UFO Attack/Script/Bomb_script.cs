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
    public GameObject [] bomb_display;
    private int current_bomb = 0;
    private Renderer Bomb_renderer;
    private Material Bomb_material;
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
    public bool CanUse(){
        return (current_bomb >0);
    }
    public void UseBomb(){
        if(current_bomb > 0){
            current_bomb--;
            getBombCarac();
            Bomb_color.a = 1f;
            Bomb_material.color = Bomb_color;
        }
    }
    public void gainBomb(){
        if(current_bomb < 5){
            getBombCarac();
            Bomb_color.a = 2;
            Bomb_material.color = Bomb_color;
            current_bomb++;
        }
    }
    // Method which obtain bomb sprite caracs
    void getBombCarac(){
        Bomb_renderer = bomb_display[current_bomb].GetComponent<Renderer>();
        Bomb_material = Bomb_renderer.material;
        Bomb_color = Bomb_material.color;
    }
}
