using System;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class LevelManager : MonoBehaviour
    {
        public BaseLevel[] Levels = Array.Empty<BaseLevel>();

        public Action<BaseLevel> OnLevelLoad { get; set; }

        private BaseLevel LastLoadedLevel { get; set; }
        private BaseLevel LoadedLevel { get; set; }

        public BaseLevel LoadLevel(BaseLevel prototype)
        {
            if (LoadedLevel != null)
                Destroy(LoadedLevel.gameObject);


            BaseLevel level = prototype != null ? Instantiate(prototype) : null;
            LastLoadedLevel = prototype;
            LoadedLevel = level;
            OnLevelLoad?.Invoke(level);
            return level;
        }


        public BaseLevel LoadLevel(int level)
        {
            --level;
            if (level >= 0 && level < Levels.Length)
            {
                BaseLevel prototype = Levels[level];

                return LoadLevel(prototype);
            }

            return null;
        }


        public BaseLevel ReloadLevel()
        {
            return LoadLevel(LastLoadedLevel);
        }
    }
}
