using UnityEngine;

namespace ScriptableObjects
{
     [CreateAssetMenu(fileName = "InputData", menuName = "Datas/InputData", order = 1)]
     public class InputDataSO : ScriptableObject
     {
         public bool onDown = false;
         public float Horizontal = 0;
         public float Vertical = 0;
         public UpTouchDelegate UpTouch;

         public delegate void UpTouchDelegate();
         
     }
}