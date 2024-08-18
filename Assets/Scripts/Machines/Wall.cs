using Assets.Scripts.Figures;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Machines
{
    public class Wall : BaseMachine
    {
        public TileBase WallTile;

        public override MachineType MachineType => MachineType.Wall;


        public override bool Placeable => false;
        public override bool CanHoldFigure => false;

        public override TileBase Tile => WallTile;

        public override void Next()
        {
        }

        public override void UpdateFigure(BaseFigure figure)
        {
        }
    }
}
