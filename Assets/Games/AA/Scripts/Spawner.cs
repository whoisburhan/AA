using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.AA
{
    public class Spawner : MonoBehaviour
    {
        [HideInInspector]
        public int SpawnedBallCounter = 0;

        public void SpawnBall()
        {
            if(SpawnedBallCounter > 0)
            {
                Destroy(Instantiate(GameManager.Instance.rippleEffect, this.transform.position, this.transform.rotation), 2f);
                GameObject go = Instantiate(GameManager.Instance.BallPrefab, this.transform.position, this.transform.rotation);

                Ball ball = go.GetComponent<Ball>();
                if(ball != null)
                {
                    ball.BallNo.text = SpawnedBallCounter.ToString();
                    ball.spawnFrom = this;
                    ball.BallNo.transform.rotation = Quaternion.identity;
                }
                SpawnedBallCounter--;
            }
        }
    }
}
