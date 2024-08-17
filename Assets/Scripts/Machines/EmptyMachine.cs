using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    internal class EmptyMachine : BaseMachine
    {

        public TileBase[] Tiles;
        public override MachineEnum MachineType => MachineEnum.None;

        public override TileBase[] Tile => Tiles;

        public override void Next()
        {
        }
    }
}
