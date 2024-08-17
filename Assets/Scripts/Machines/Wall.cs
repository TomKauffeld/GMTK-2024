using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Wall : BaseMachine
    {
        public TileBase[] Tiles;

        public override MachineEnum MachineType => MachineEnum.Wall;


        public override bool Placeable => false;

        public override TileBase[] Tile => Tiles;

        public override void Next()
        {
        }
    }
}
