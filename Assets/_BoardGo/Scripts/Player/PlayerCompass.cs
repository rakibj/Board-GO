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

        private void ResetArrows()
        {
            for (int i = 0; i < Board.Board.directions.Length; i++)
            {
                iTween.Stop(m_arrows[i]);
                var dirVector = new Vector3(Board.Board.directions[i].normalized.x, 0f,
                    Board.Board.directions[i].normalized.y);
                m_arrows[i].transform.position = transform.position + dirVector * startDistance;
            }
        }

        private void MoveArrows()
        {
            foreach (var arrow in m_arrows)
            {
                MoveArrow(arrow);
            }
        }

        public void ShowArrows(bool state)
        {
            if (m_board == null)
            {
                Debug.LogWarning("PlayerCompass.cs: No Board Found");
                return;
            }
            if (m_arrows == null || m_arrows.Count != Board.Board.directions.Length)
            {
                Debug.LogWarning("PlayerCompass.cs: No Arrows Found");
                return;
            }

            if (m_board.PlayerNode != null)
            {
                for (int i = 0; i < Board.Board.directions.Length; i++)
                {
                    var neighbor = m_board.PlayerNode.FindNeighborAt(Board.Board.directions[i]);
                    if (neighbor == null || !state)
                    {
                        m_arrows[i].SetActive(false);
                    }
                    else
                    {
                        var activeState = m_board.PlayerNode.LinkedNodes.Contains(neighbor);
                        m_arrows[i].SetActive(activeState);
                    }
                }
            }
            ResetArrows();
            MoveArrows();
        }
    }
}
