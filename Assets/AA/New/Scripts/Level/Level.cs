using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum Direction 
{
    LeftToRight, RightToLeft
}

public enum Shapes 
{
    Circle, Shape2, Shape3, Shape4, Shape5, Shape6
}

[CreateAssetMenu(fileName = "Level - 1", menuName = "Level")]
public class Level : ScriptableObject
{
    public Shapes ShapeType;
    public float RotateSpeed;
    public Direction RotateDirection;
    public bool StopAndGo;
    public List<bool> Pins;
    public int Target;

}
