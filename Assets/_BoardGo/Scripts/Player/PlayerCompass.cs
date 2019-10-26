using System;
using System.Collections.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Player
{
    public class PlayerCompass : MonoBehaviour
    {
        private Board.Board m_board;
        public GameObject arrowPrefab;
        List<GameObject> m_arrows = new List<GameObject>();
        public float scale = 1;
        public float startDistance = 0.25f;
        public float endDistance = 0.5f;

        public float moveTime = 1f;
        public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
        public float delay = 0;
        private void Awake()
        {
            m_board = FindObjectOfType<Board.Board>();
            SetupArrows();
            MoveArrows();
        }

        private void SetupArrows()
        {
            if (arrowPrefab == null)
            {
                Debug.LogWarning("PlayerCompass.cs: Arrow Prefab not assigned");
                return;
            }

            foreach (var dir in Board.Board.directions)
            {
                Vector3 dirVector = new Vector3(dir.normalized.x, 0, dir.normalized.y);
                Quaternion rotation = Quaternion.LookRotation(dirVector);
                GameObject arrowInstance =
                    Instantiate(arrowPrefab, transform.position + dirVector * startDistance, rotation);
                arrowInstance.transform.localScale = Vector3.one * scale;
                arrowInstance.transform.parent = transform;
                m_arrows.Add(arrowInstance);
            }
        }
        private void MoveArrow(GameObject arrowInstance)
        {
            iTween.MoveBy(arrowInstance, iTween.Hash(
                "z", endDistance - startDistance,
                "looptype", iTween.LoopType.loop,
                "time", moveTime,
                "easetype", easeType
                ));
        }

        private void MoveArrows()
        {
            foreach (var arrow in m_arrows)
            {
                MoveArrow(arrow);
            }
        }
    }
}
