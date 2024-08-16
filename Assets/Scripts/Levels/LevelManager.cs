using System;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class LevelManager : MonoBehaviour
    {
        public BaseLevel[] Levels = Array.Empty<BaseLevel>();

        public Action<BaseLevel> OnLevelLoad { get; set; }

        private BaseLevel _lastLoadedLevel { get; set; }

        public BaseLevel LoadLevel(BaseLevel prototype)
        {

            BaseLevel level = prototype != null ? Instantiate(prototype) : null;
            _lastLoadedLevel = prototype;
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
            return LoadLevel(_lastLoadedLevel);
        }
    }
}
