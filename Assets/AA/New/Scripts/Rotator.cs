using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public bool stopAndGO;
    public float speed = 100f;
	public Direction rotatorDirection;

    private bool isRunning = true;
    private float timer = .75f;



    public void UpdateRotator(float speed = 100f, bool stopAndGo = false, Direction direction = Direction.LeftToRight) 
    {
        this.speed = speed;
        this.stopAndGO = stopAndGo;
        rotatorDirection = direction;
    }


    void Update ()
	{
		
        if (!stopAndGO)
        {
            if (rotatorDirection == Direction.LeftToRight)
            {
                transform.Rotate(0f, 0f, speed * Time.deltaTime);
            }
            else if (rotatorDirection == Direction.RightToLeft)
            {
                transform.Rotate(0f, 0f, -speed * Time.deltaTime);
            }
        }

        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isRunning = !isRunning;
                timer = .75f;
            }

            if (isRunning)
            {
                if (rotatorDirection == Direction.LeftToRight)
                {
                    transform.Rotate(0f, 0f, speed * Time.deltaTime);
                }
                else if (rotatorDirection == Direction.RightToLeft)
                {
                    transform.Rotate(0f, 0f, -speed * Time.deltaTime);
                }
            }
        }
    }

}
