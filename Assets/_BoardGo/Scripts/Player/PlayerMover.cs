﻿using System.Collections;
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

        private void Start()
        {
        }

        IEnumerator Test()
        {
            yield return new WaitForSeconds(1);
            MoveRight();
            yield return new WaitForSeconds(2);
            MoveRight();
            yield return new WaitForSeconds(2);
            MoveForward();
            yield return new WaitForSeconds(2);
            MoveForward();
        }
        public void Move(Vector3 destinationPos, float delayTime = .25f)
        {
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
        }
        public void MoveLeft()
        {
            var newPosition = transform.position + new Vector3(-2, 0, 0);
            Move(newPosition, 0);
        }
        public void MoveRight()
        {
            var newPosition = transform.position + new Vector3(2, 0, 0);
            Move(newPosition, 0);
        }
        public void MoveForward()
        {
            var newPosition = transform.position + new Vector3(0, 0, 2);
            Move(newPosition, 0);
        }
        public void MoveBackward()
        {
            var newPosition = transform.position + new Vector3(0, 0, -2);
            Move(newPosition, 0);
        }
    }
}