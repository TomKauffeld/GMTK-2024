using Assets.Scripts.Machines;

namespace Assets.Scripts.Ui
{
    public class InventoryManager : BaseMonoBehaviour
    {


        private void Start()
        {
            LaunchNotification($"switch:machine:{MachineType.None}");
        }
    }
}
