using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GeneralEventManager : MonoBehaviour
    {
        public string LastEvent = string.Empty;


        private readonly Dictionary<string, List<Action<string>>> _handlers = new();


        public void Register(string pattern, Action<string> handler)
        {
            if (!_handlers.ContainsKey(pattern))
                _handlers.Add(pattern, new List<Action<string>>());
            _handlers[pattern].Add(handler);
        }

        public void Unregister(string pattern, Action<string> handler)
        {
            if (_handlers.ContainsKey(pattern))
                _handlers[pattern].Remove(handler);
        }

        private void InvokeHandlers(string pattern, string eventName)
        {
            if (_handlers.TryGetValue(pattern, out List<Action<string>> handlers))
            {
                foreach (Action<string> handler in handlers)
                {
                    handler.Invoke(eventName);
                }
            }
        }


        public void LaunchEvent(string eventName)
        {
            LastEvent = eventName;
            InvokeHandlers("*", eventName);

            string[] parts = eventName.Split(':');
            string pattern = "";
            for (int i = 0; i < parts.Length - 1; ++i)
            {
                if (i == 0)
                    pattern = parts[i];
                else
                    pattern += ":" + parts[i];

                InvokeHandlers(pattern + ":*", eventName);
            }

            InvokeHandlers(eventName, eventName);
        }
    }
}