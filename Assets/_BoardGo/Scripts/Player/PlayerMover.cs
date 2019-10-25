using System;
using System.Collections;
using UnityEngine;

namespace _BoardGo.Scripts.Player
{
    public class PlayerMover : MonoBehaviour
    {
        public Vector3 destination;
        public bool isMoving = false;
        public iTween.EaseType easeType;
        public float moveSpeed = 1.5f;
        public float iTweenDelay = 0f;
        private Board.Board m_board;

        private void Awake()
        {
            m_board = FindObjectOfType<Board.Board>();
        }

        private void Start()
        {
            UpdateBoard();
            if (m_board != null && m_board.PlayerNode != null)
            {
                m_board.PlayerNode.InitNode();
            }
            
        }

        public void Move(Vector3 destinationPos, float delayTime = .25f)
        {
            if (m_board != null)
            {
                var targetNode = m_board.FindNodeAt(destinationPos);
                if (targetNode == null)
                    return;
                if (!m_board.PlayerNode.LinkedNodes.Contains(targetNode))
                    return;
            }
            StartCoroutine(MoveRoutine(destinationPos, delayTime));
        }
        private IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
        {
            isMoving = true;
            destination = destinationPos;
            yield return new WaitForSeconds(delayTime);
            iTween.MoveTo(gameObject, iTween.Hash(
                "x", destinationPos.x,
                "y", destinationPos.y,
                "z", destinationPos.z,
                "delay", iTweenDelay,
                "easetype", easeType,
                "speed", moveSpeed
                ));

            while (Vector3.Distance(transform.position, destinationPos) > .01f)
            {
                yield return null;
            }
            
            iTween.Stop(gameObject);
            transform.position = destinationPos;
            isMoving = false;
            UpdateBoard();
        }
        public void MoveLeft()
        {
            var newPosition = transform.position + new Vector3(-Board.Board.spacing, 0, 0);
            Move(newPosition, 0);
        }
        public void MoveRight()
        {
            var newPosition = transform.position + new Vector3(Board.Board.spacing, 0, 0);
            Move(newPosition, 0);
        }
        public void MoveForward()
        {
            var newPosition = transform.position + new Vector3(0, 0, Board.Board.spacing);
            Move(newPosition, 0);
        }
        public void MoveBackward()
        {
            var newPosition = transform.position + new Vector3(0, 0, -Board.Board.spacing);
            Move(newPosition, 0);
        }

        private void UpdateBoard()
        {
            if (m_board != null)
                m_board.UpdatePlayerNode();
        }
    }
}
