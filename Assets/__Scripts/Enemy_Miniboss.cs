using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Miniboss : Enemy
{
    public GameObject hero;
    private float birthTime;
    // Start is called before the first frame update
    public float Cooldown = 8f;
    public float CoolAdd = 8f;
    public float age;
    public enum State { Tracking, Charinging, Returning};
    private State state;

    private Vector3 enemyY;
    private Vector3 originY;
    private Vector3 heroY;
    private Vector3 endY;


    void Start()
    {
        birthTime = Time.timeSinceLevelLoad;
        hero = GameObject.FindGameObjectWithTag("Player");
        age = 0;
        state = State.Tracking;
        Cooldown = 8f;
        CoolAdd = 8f;
        
        enemyY = this.gameObject.transform.position;
        originY = enemyY;
        heroY = hero.transform.position;
        endY = heroY;

        health = 30;

    }

    public override void Move()
    {
        if (hero != null)
        {
            //this is to try and track the player's movement,
            //and move the enemy to match the player's X coordinate
            age += Time.deltaTime;
            switch (state){
                case State.Tracking:
                    Vector3 playerPos = hero.transform.position;
                    Vector3 enemyPos = this.gameObject.transform.position;
                    enemyPos.x = playerPos.x;
                    this.transform.position = enemyPos;
                    
                    if(age > Cooldown)
                    {
                        originY.y = enemyPos.y;
                        endY = hero.transform.position;
                        state = State.Charinging;
                        Debug.Log("SMASH!");
                    }
                    break;
                //My idea is that the enemy targets the hero's X position.
                //Once a timer has passed, the enemy slams towards the hero's saved position.
                case State.Charinging:
                    speed = 50;
                    Vector3 tempPos = pos;
                    tempPos.y -= speed * Time.deltaTime;
                    this.transform.position = tempPos;
                    if(this.gameObject.transform.position.y < endY.y)
                    {
                        state = State.Returning;
                    }
                    break;
                //After slamming, it returns to the near top of the screen, before starting the timer again.
                case State.Returning:
                    speed = 50;
                    tempPos = pos;
                    tempPos.y += speed * Time.deltaTime;
                    this.transform.position = tempPos;
                    if (this.gameObject.transform.position.y >= originY.y)
                    {
                        Cooldown += CoolAdd;
                        state = State.Tracking;
                    }
                    break;
                }
            }
        }
    }

