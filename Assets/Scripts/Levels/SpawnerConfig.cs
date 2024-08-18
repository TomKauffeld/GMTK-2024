using Assets.Scripts.Figures;
using System;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    [Serializable]
    public struct SpawnerConfig
    {
        public Vector2Int Position;
        public FigureType FigureType;
    }
}
