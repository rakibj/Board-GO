using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Board
{
    public class Node : MonoBehaviour
    {
        private Vector2 m_coordinate;
        public Vector2 Coordinate => Utility.Vector2Round(m_coordinate);

        private List<Node> m_neighborNodes;
        public List<Node> NeighborNodes => m_neighborNodes;

        private Board m_board;
        
        public GameObject geometry;
        public float scaleTime = 0.3f;
        public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

        public bool autoRun = false;
        public float delay = 1f;
        private bool m_isInitialized = false;

        private void Awake()
        {
            m_board = FindObjectOfType<Board>();
            m_coordinate = new Vector2(transform.position.x, transform.position.z);
        }
        private void Start()
        {
            if (geometry != null)
            {
                geometry.transform.localScale = Vector3.zero;
                if (autoRun)
                    InitNode();

                if (m_board != null)
                    m_neighborNodes = FindNeighbors(m_board.AllNodes);
            }
        }
        public void ShowGeometry()
        {
            if (geometry != null)
            {
                iTween.ScaleTo(geometry, iTween.Hash(
                    "time", scaleTime,
                    "scale", Vector3.one,
                    "easetype", easeType,
                    "delay", delay
                    ));
            }
        }
        public List<Node> FindNeighbors(List<Node> nodes)
        {
            List<Node> nNodes = new List<Node>();
            foreach (var dir in Board.directions)
            {
                var foundNeighbor = nodes.Find(n => n.Coordinate == Coordinate + dir);
                if (foundNeighbor != null && !nNodes.Contains(foundNeighbor))
                    nNodes.Add(foundNeighbor);
            }
            return nNodes;
        }

        public void InitNode()
        {
            if (!m_isInitialized)
            {
                ShowGeometry();
                InitNeighbors();
                m_isInitialized = true; 
            }
        }

        private void InitNeighbors()
        {
            StartCoroutine(InitNeighborsRoutine());
        }

        private IEnumerator InitNeighborsRoutine()
        {
            yield return new WaitForSeconds(delay);
            foreach (var neighborNode in m_neighborNodes)
            {
                neighborNode.InitNode();
            }
        }
    }
}
