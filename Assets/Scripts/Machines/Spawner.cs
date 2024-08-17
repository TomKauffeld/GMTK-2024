using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Spawner : BaseMachine
    {
        public TileBase[] Tiles;

        public override MachineEnum MachineType => MachineEnum.Spawner;

        public override bool Placeable => false;

        public override TileBase[] Tile => Tiles;

        public override void Next()
        {
        }
    }
}
