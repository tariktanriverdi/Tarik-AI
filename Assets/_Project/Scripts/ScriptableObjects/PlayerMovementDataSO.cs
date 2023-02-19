using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerMovement", menuName = "Datas/PlayerMovement", order = 2)]
    public class PlayerMovementDataSO : ScriptableObject

    {
        public float ForwarSpeed = 1;
        public float RotateSpeed = 1;
        public float Acceleration = 5f;
    }
}