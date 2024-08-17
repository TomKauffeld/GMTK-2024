using Assets.Scripts.Machines;
using System.Linq;
using TMPro;

namespace Assets.Scripts.Ui
{
    public class InventoryItem : BaseMonoBehaviour
    {
        public MachineEnum MachineType;

        public TextMeshProUGUI TitleUi;
        public TextMeshProUGUI CountUi;

        public int Count = 0;

        private void Start()
        {
            Register($"inventory:update:{MachineType}:*", OnInventoryUpdate);
            TitleUi.text = $"{MachineType}";
            UpdateCount();
        }

        private void OnInventoryUpdate(string eventName)
        {
            string data = eventName.Split(':').Last();

            if (int.TryParse(data, out int count))
            {
                Count = count;
                UpdateCount();
            }
        }


        private void UpdateCount()
        {
            CountUi.text = Count >= 0 ? Count.ToString() : "";
        }

        public void OnClick()
        {
            LaunchNotification($"switch:machine:{MachineType}");
        }
    }
}
