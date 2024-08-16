using Assets.Scripts.Machines;
using TMPro;

namespace Assets.Scripts
{
    public class InventoryItem : BaseMonoBehaviour
    {
        public MachineEnum MachineType;

        public TextMeshProUGUI CountUi;

        private int _count = 0;
        public int Count = 0;

        private void Start()
        {
            Register($"inventory:update:{MachineType}", OnInventoryUpdate);
            UpdateCount();
        }

        private void Update()
        {
            if (_count != Count)
                UpdateCount();
        }

        private void OnInventoryUpdate(string eventName)
        {
            UpdateCount();
        }


        private void UpdateCount()
        {
            _count = Count;
            CountUi.text = _count >= 0 ? _count.ToString() : "";
        }
    }
}
