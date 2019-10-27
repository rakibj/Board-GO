using System;
using System.Collections;
using System.Collections.Generic;
using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Enemy
{
    public enum MovementType
    {
        Sationary,
        Patrol,
        Spinner
    }

    public class EnemyMover : Mover
    {
        public float standTime = 1f;
        private Vector3 directionToMove = new Vector3(0f, 0f, Board.Board.spacing);
        public MovementType movementType;
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void MoveOneTurn()
        {
            switch (movementType)
            {
                case MovementType.Sationary:
                    Stand();
                    break;
                case MovementType.Patrol:
                    Patrol();
                    break;
                case MovementType.Spinner:
                    Spin();
                    break;
            }
        }

        private void Stand()
        {
            StartCoroutine(StandRoutine());
        }

        private IEnumerator StandRoutine()
        {
            yield return new WaitForSeconds(standTime);
            base.finishMovementEvent.Invoke();
        }

        private void Patrol()
        {
            StartCoroutine(PatrolRoutine());
        }

        private IEnumerator PatrolRoutine()
        {
            Vector3 startPos = new Vector3(m_currentNode.Coordinate.x, 0, m_currentNode.Coordinate.y);
            var newDest = startPos + transform.TransformVector(directionToMove);
            var nextDest = startPos + transform.TransformVector(directionToMove * 2);
            Move(newDest, 0);
            while (isMoving)
            {
                yield return null;
            }

            if (m_board != null)
            {
                var newDestNode = m_board.FindNodeAt(newDest);
                var nextDestNode = m_board.FindNodeAt(nextDest);
                if (nextDestNode == null || !newDestNode.LinkedNodes.Contains(nextDestNode))
                {
                    destination = startPos;
                    FaceDestination();
                    yield return new WaitForSeconds(rotateTime);
                }
            }

            base.finishMovementEvent.Invoke();
        }

        private void Spin()
        {
            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            var localForward = new Vector3(0,0,Board.Board.spacing);
            destination = transform.TransformVector(localForward * -1) + transform.position;
            FaceDestination();
            yield return new WaitForSeconds(rotateTime);
            base.finishMovementEvent.Invoke();
        }
    }
}
