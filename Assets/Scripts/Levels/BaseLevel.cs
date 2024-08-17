using UnityEngine;

namespace Assets.Scripts.Levels
{
    public abstract class BaseLevel : MonoBehaviour
    {
        public abstract Vector2Int Size { get; }
        public int RemainingBelts;
        public int RemainingRotations;
        public int RemainingSize;
        public int RemainingFlips;
    }
}
