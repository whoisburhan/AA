using UnityEngine;
using System.Collections.Generic;
using TMPro;
namespace GS.AA
{
    public class Circle : MonoBehaviour
    {
        private SpriteRenderer sr;
        public bool TwoSideRotation;
        public TextMeshPro levelText;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            GameManager.OnLevelCompleted += AutoDeactivate;
            GameManager.OnGameOver += AutoDeactivate;
        }

        private void OnDisable()
        {
            GameManager.OnLevelCompleted -= AutoDeactivate;
            GameManager.OnGameOver -= AutoDeactivate;
        }

        // Active - Deactiving Circles deadly enemy child
        public void SetEnemyActivityStatus(bool[] _activityStatus)
        {
            List<GameObject> enemyList = new List<GameObject>();
            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).CompareTag(TagsAndPlayerprefs.BALL_TAG))
                {
                    enemyList.Add(transform.GetChild(i).gameObject);
                }
            }

            if(_activityStatus.Length != enemyList.Count)
            {
                Debug.LogError("Enemy Numer mismatched!!! _activityStatus.Length:" + _activityStatus.Length + "  | enemyList.Count:" + enemyList.Count);
                return;
            }

            for(int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].SetActive(_activityStatus[i]);
            }
        }

        public void AutoDeactivate()
        {
            this.gameObject.SetActive(false);
        }

    }
}