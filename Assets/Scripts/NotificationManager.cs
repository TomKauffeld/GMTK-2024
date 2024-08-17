using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class NotificationManager : MonoBehaviour
    {
        private readonly Dictionary<string, List<Action<string>>> _handlers = new();


        public void Register(string pattern, Action<string> handler)
        {
            if (!_handlers.ContainsKey(pattern))
                _handlers.Add(pattern, new List<Action<string>>());
            _handlers[pattern].Add(handler);
        }

        public void Unregister(string pattern, Action<string> handler)
        {
            if (_handlers.TryGetValue(pattern, out List<Action<string>> handlers))
                handlers.Remove(handler);
        }

        private void InvokeHandlers(string pattern, string notification)
        {
            if (_handlers.TryGetValue(pattern, out List<Action<string>> handlers))
            {
                foreach (Action<string> handler in handlers)
                {
                    handler.Invoke(notification);
                }
            }
        }


        public void LaunchNotification(string notification)
        {
#if UNITY_EDITOR
            Debug.Log($"Notification launched: {notification}");
#endif
            InvokeHandlers("*", notification);

            string[] parts = notification.Split(':');
            string pattern = "";
            for (int i = 0; i < parts.Length - 1; ++i)
            {
                if (i == 0)
                    pattern = parts[i];
                else
                    pattern += ":" + parts[i];

                InvokeHandlers(pattern + ":*", notification);
            }

            InvokeHandlers(notification, notification);
        }
    }
}