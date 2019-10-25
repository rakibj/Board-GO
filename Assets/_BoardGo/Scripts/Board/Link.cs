using System;
using UnityEngine;

namespace _BoardGo.Scripts.Board
{
    public class Link : MonoBehaviour
    {
        public float scaleTime = .25f;
        public float delay = 0.1f;
        public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

        public void DrawLink(Vector3 startPos, Vector3 endPos)
        {
            transform.localScale = new Vector3(1,1,0);
            Vector3 dirVector = endPos - startPos;
            float zScale = dirVector.magnitude / Board.spacing;
            var newScale = new Vector3(1,1,zScale);
            transform.rotation = Quaternion.LookRotation(dirVector);
            transform.position = startPos + dirVector.normalized * .1f;
            iTween.ScaleTo(gameObject, iTween.Hash(
                "time", scaleTime,
                "scale", newScale,
                "easetype", easeType,
                "delay", delay
                ));
            iTween.MoveTo(gameObject, iTween.Hash(
                "time", scaleTime,
                "position", startPos,
                "easetype", easeType,
                "delay", delay
                ));
        }
    }
}
