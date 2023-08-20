using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//[ExecuteInEditMode]
[Serializable]
public struct PinData 
{
    public string Name;
    public Transform Shape;
    public List<Transform> Pins;
}
namespace FantasyRealm.AA {
    public class LevelGenerator : MonoBehaviour
    {

        public static LevelGenerator Instacne;

        [Range(0, 100f)]
        public float RotateSpeed;
        public bool StopAndGo;
        public Direction Direction;
        public int Counter = 0;
        public string LevelName;
        public int Target;



        //[SerializeField] private List<PinData> pinData;
        [SerializeField] private PinData[] m_Data = new PinData[6];


        [Header("Levels")]
        [SerializeField] private List<Level> levels = new List<Level>();


        private void Awake()
        {
            if (Instacne == null)
            {
                Instacne = this;
                //DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Create();
            }
        }

        private void Create()
        {
            //Level newLevel = new Level();


            //newLevel.Target = Target;

            //var _activeShape = ActiveShape();
            //newLevel.ShapeType = (Shapes)_activeShape;
            //var _pinData = m_Data[_activeShape].Pins;

            //var rotator = m_Data[_activeShape].Shape.GetComponent<Rotator>();
            //newLevel.RotateSpeed = rotator.speed;
            //newLevel.RotateDirection = rotator.rotatorDirection;
            //newLevel.StopAndGo = rotator.stopAndGO;

            //var _tempPins = new List<bool>();

            //for (int i = 0; i < _pinData.Count; i++)
            //{
            //    if (_pinData[i].gameObject.activeSelf) _tempPins.Add(true);
            //    else _tempPins.Add(false);
            //}


            //newLevel.Pins = new List<bool>(_tempPins);
            //AssetDatabase.CreateAsset(newLevel, $"Assets/New/LevelData/{LevelName} - {Counter}.asset");
            //Counter++;
        }


        private int ActiveShape()
        {
            for (int i = 0; i < m_Data.Length; i++)
            {
                if (m_Data[i].Shape.gameObject.activeSelf) return i;
            }

            Debug.LogError("NO SHAPE IS ACTIVE");
            return -1;
        }


        #region Load Level

        public void RequestForLoadLevel(int levelNo)
        {
            int _newLevelNo = levelNo >= levels.Count ? UnityEngine.Random.Range(1, levels.Count) : levelNo;
            LoadLevel(levels[_newLevelNo]);
        }

        private void LoadLevel(Level newLevel)
        {
            // Target
            DeactivateAllShape();
            var _pinData = m_Data[(int)newLevel.ShapeType];

            _pinData.Shape.gameObject.SetActive(true);

            var _rotator = _pinData.Shape.GetComponent<Rotator>();

            _rotator.rotatorDirection = newLevel.RotateDirection;
            _rotator.stopAndGO = newLevel.StopAndGo;
            _rotator.speed = newLevel.RotateSpeed;

            if (_pinData.Pins.Count != newLevel.Pins.Count)
            {
                Debug.LogError("PINDATA and LEVEL PIN NOT EQUAL");
                return;
            }

            for (int i = 0; i < _pinData.Pins.Count; i++)
            {
                _pinData.Pins[i].gameObject.SetActive(newLevel.Pins[i]);
            }

            GameManager.Instance.PinLeftToCompleteLevel = newLevel.Target;
        }

        private void DeactivateAllShape()
        {
            foreach (var shape in m_Data)
            {
                shape.Shape.gameObject.SetActive(false);
            }

        }

        #endregion

    }
}