using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public abstract class BaseMachine : MonoBehaviour
    {
        public event Action<BaseMachine> TileChanged;

        public abstract MachineEnum MachineType { get; }
        public abstract TileBase Tile { get; }

        public Vector2Int Position => new((int)transform.localPosition.x, (int)transform.localPosition.y);


        protected void InvokeTileChanged()
        {
            TileChanged?.Invoke(this);
        }

        public abstract void Next();
    }
}
