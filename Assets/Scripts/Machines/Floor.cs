using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Floor : BaseMachine
    {

        public TileBase FloorTile;
        public override MachineType MachineType => MachineType.None;

        public override TileBase Tile => FloorTile;

        public override void Next()
        {
        }
    }
}
