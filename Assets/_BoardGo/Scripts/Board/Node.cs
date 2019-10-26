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

        private List<Node> m_linkedNodes = new List<Node>();
        public List<Node> LinkedNodes => m_linkedNodes;
        
        private Board m_board;
        
        public GameObject geometry;
        public GameObject linkPrefab;
        public float scaleTime = 0.3f;
        public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

        public float delay = 1f;
        public LayerMask obstacleLayer;
        public bool isLevelGoal = false;        
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
                var foundNeighbor = FindNeighborAt(nodes, dir);
                if (foundNeighbor != null && !nNodes.Contains(foundNeighbor))
                    nNodes.Add(foundNeighbor);
            }
            return nNodes;
        }

        public Node FindNeighborAt(List<Node> nodes, Vector2 dir)
        {
            return nodes.Find(n => n.Coordinate == Coordinate + dir);
        }
        public Node FindNeighborAt(Vector2 dir)
        {
            return FindNeighborAt(m_neighborNodes, dir);
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
                if (!m_linkedNodes.Contains(neighborNode))
                {
                    Obstacle obstacle = FindObstacle(neighborNode);
                    if (obstacle == null)
                    {
                        LinkNode(neighborNode);
                        neighborNode.InitNode();
                    }
                }
            }
        }

        private void LinkNode(Node targetNode)
        {
            if (linkPrefab != null)
            {
                GameObject linkInstance = Instantiate(linkPrefab, transform.position, Quaternion.identity);
                linkInstance.transform.parent = transform;
                Link link = linkInstance.GetComponent<Link>();
                if (link != null)
                {
                    link.DrawLink(transform.position, targetNode.transform.position);
                }

                if (!m_linkedNodes.Contains(targetNode))
                {
                    m_linkedNodes.Add(targetNode);
                }

                if (!targetNode.LinkedNodes.Contains(this))
                {
                    targetNode.LinkedNodes.Add(this);
                }
            }
        }

        private Obstacle FindObstacle(Node targetNode)
        {
            Vector3 checkDirection = targetNode.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, checkDirection, out hit, Board.spacing + .1f, obstacleLayer))
            {
                return hit.collider.GetComponent<Obstacle>();
            }

            return null;
        }

        
        
    }
}
