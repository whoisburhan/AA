using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.AA
{
    [CreateAssetMenu(fileName = "Level-1", menuName = "AA Level Data")]
    public class LevelData : ScriptableObject
    {
        public bool[] ActiveCircle = new bool[3];
        public bool[] Circle_0_Active_Enemy = new bool[14];
        public bool[] Circle_1_Active_Enemy = new bool[14];
        public bool[] Circle_2_Active_Enemy = new bool[14];

        public float Circle_0_Speed;
        public float Circle_1_Speed;
        public float Circle_2_Speed;

        public bool Circle_0_Two_Side_Rotation;
        public bool Circle_1_Two_Side_Rotation;
        public bool Circle_2_Two_Side_Rotation;

        public bool Circle_0_IsRotateRight;
        public bool Circle_1_IsRotateRight;
        public bool Circle_2_IsRotateRight;

        public bool Circle_0_IsRunAndStopMode;
        public bool Circle_1_IsRunAndStopMode;
        public bool Circle_2_IsRunAndStopMode;

        public bool Circle_1_AND_Circle_2_SameSideRotation;

        public int NoOfBallSpawnInSpawner0;
        public int NoOfBallSpawnInSpawner1;
    }

    
}