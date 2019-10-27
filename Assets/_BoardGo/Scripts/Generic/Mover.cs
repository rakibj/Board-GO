using System.Collections;
using _BoardGo.Scripts.Board;
using _BoardGo.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace _BoardGo.Scripts.Generic
{
    
    public class Mover : MonoBehaviour
    {
        public bool faceDestination = false;
        public Vector3 destination;
        public bool isMoving = false;
        public iTween.EaseType easeType;
        public float moveSpeed = 1.5f;
        public float rotateTime = 0.5f;
        public float iTweenDelay = 0f;
        protected Board.Board m_board;
        protected Node m_currentNode;
        public Node CurrentNode => m_currentNode;
        public UnityEvent finishMovementEvent; 
        protected virtual void Awake()
        {
            m_board = FindObjectOfType<Board.Board>();
        }

        protected virtual void Start()
        {
            UpdateCurrentNode();
        }

        public void Move(Vector3 destinationPos, float delayTime = .25f)
        {
            if (isMoving)
                return;
            
            if (m_board != null)
            {
                var targetNode = m_board.FindNodeAt(destinationPos);
                if (targetNode == null && m_currentNode == null)
                    return;
                if (!m_currentNode.LinkedNodes.Contains(targetNode))
                    return;
            }
            StartCoroutine(MoveRoutine(destinationPos, delayTime));
        }
        protected virtual IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
        {
            
            isMoving = true;
            destination = destinationPos;
            if (faceDestination)
            {
                FaceDestination();
                yield return new WaitForSeconds(.25f);
            }
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
            UpdateCurrentNode();
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

        protected void UpdateCurrentNode()
        {
            if (m_board != null)
            {
                m_currentNode = m_board.FindNodeAt(transform.position);
            }
        }

        protected void FaceDestination()
        {
            var relativePosition = destination - transform.position;
            var newRotation = Quaternion.LookRotation(relativePosition);
            var newY = newRotation.eulerAngles.y;
            iTween.RotateTo(gameObject, iTween.Hash(
                "y", newY,
                "delay", 0f,
                "easetype", easeType,
                "time", rotateTime
                ));
        }
    }
}
