using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.AA
{
    public class Rotator : MonoBehaviour
    {
        [HideInInspector] public bool IsRotateRight;
        [HideInInspector] public bool IsRunAndStopMode;
        [HideInInspector] public float RotateSpeed;

        private bool isRunning = true;
        private float timer = .75f;

        private void Update()
        {
            if (!IsRunAndStopMode)
            {
                if (IsRotateRight)
                    transform.Rotate(0f, 0f, -RotateSpeed * Time.deltaTime);
                else
                    transform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
            }

            else
            {
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    isRunning = !isRunning;
                    timer = .75f;
                }

                if (isRunning)
                {
                    if (IsRotateRight)
                        transform.Rotate(0f, 0f, -RotateSpeed * Time.deltaTime);
                    else
                        transform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
                }
            }
        }
    } 
}
