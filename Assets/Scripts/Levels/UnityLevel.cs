using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class UnityLevel : BaseLevel
    {
        public Vector2Int LevelSize;

        public override Vector2Int Size => LevelSize;
    }
}
