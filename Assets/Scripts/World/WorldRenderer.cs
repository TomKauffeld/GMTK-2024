using Assets.Scripts.Machines;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(Tilemap))]
    public class WorldRenderer : MonoBehaviour
    {
        public Camera RenderingCamera;
        private Vector2Int _renderSize = Vector2Int.zero;

        public bool IsReady { get; private set; } = false;

        public Vector2Int RenderSize
        {
            get => _renderSize;
            set
            {
                _renderSize = value;
                UpdateRenderSize();
            }
        }

        private void UpdateRenderSize()
        {
            if (_renderSize.x <= 0 || _renderSize.y <= 0)
                return;

            RenderingCamera ??= Camera.main;

            RenderingCamera.transform.position = new Vector3(
                _renderSize.x / 2f,
                (_renderSize.y + 4) / 2f - 4,
                RenderingCamera.transform.position.z
            );

            PixelPerfectCamera camera = RenderingCamera.GetComponent<PixelPerfectCamera>();


            int height = camera.refResolutionY / (_renderSize.y + 4);
            int width = camera.refResolutionX / (_renderSize.x);
            int size = Math.Min(height, width) + 2;
            camera.assetsPPU = size;
        }

        public event Action OnReady;


        private Tilemap _tilemap;

        void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            IsReady = true;
            OnReady?.Invoke();
        }


        internal void SetMachine([NotNull] BaseMachine machine)
        {
            SetTile(machine.Position, machine.Tile);
        }

        internal void SetEmpty(Vector2Int position)
        {
            SetTile(position, null);
        }


        private void SetTile(Vector2Int position, [CanBeNull] TileBase tile)
        {
            _tilemap.SetTile(new Vector3Int(position.x, position.y), tile);
        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            Vector3Int tmp = _tilemap.WorldToCell(position);
            return new Vector2Int(tmp.x, tmp.y);
        }
    }
}
