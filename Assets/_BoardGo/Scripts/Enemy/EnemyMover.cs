using System.Collections;
using _BoardGo.Scripts.Generic;
using UnityEngine;

namespace _BoardGo.Scripts.Enemy
{
    public class EnemyMover : Mover
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        IEnumerator TestMovement()
        {
            yield return new WaitForSeconds(2f);
            MoveForward();
            yield return new WaitForSeconds(2f);
            MoveRight();
            yield return new WaitForSeconds(2f);
            MoveForward();
            yield return new WaitForSeconds(2f);
            MoveLeft();
            yield return new WaitForSeconds(2f);
            MoveForward();
        }
    }
}
