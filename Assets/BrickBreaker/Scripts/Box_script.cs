using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.AnimatedValues;

public class Box_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */
    public GameObject coin_prefab; // reference to coin prefab
    public GameObject speedBonus_prefab;
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
        
    }

    // Method to manage collisions between the ball and our boxes
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "ball"){
            Vector3 box_pos = gameObject.transform.position;
            Destroy(gameObject);
            // Instantiate one coins when ball destroyed a box (with 10% chance)
            if(UnityEngine.Random.Range(1,10) == 1){
                Instantiate(coin_prefab,box_pos,Quaternion.identity);
            }
            // Instantiate one Speed Bonus when ball destroyed a box (with 5% chance)
            if(UnityEngine.Random.Range(1,10) == 1){
                Instantiate(speedBonus_prefab,box_pos,Quaternion.identity);
            }
        }
    }
}
