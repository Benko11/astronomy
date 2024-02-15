using Pololu.UsbWrapper;
using Pololu.Usc;

namespace astronomy
{
    internal class Servo
    {
        private List<DeviceListItem> _devices;
        public List<DeviceListItem> Devices { get { return _devices; } }

        public DeviceListItem GetActiveDevice()
        {
            return _devices.First();
        }

        public Servo() {
            _devices = Usc.getConnectedDevices();
        }

        public delegate void ExecuteCallback(Usc device);

        public Usc Connect()
        {
            if (_devices.Count < 2)
            {
                return new Usc(_devices.First());
            }

            Console.Write("Select device: ");
            Console.ReadLine();
            return new Usc(_devices.First());
        }

        public void Execute(ExecuteCallback callback)
        {
            try
            {
                Usc device = Connect();
                callback(device);
                Disconnect(device);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"It appears you have not plugged in the equipment ({ex.Message})\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"It appears that the equipment has been unplugged, check if the cables are firmly plugged in ({ex.Message})\n");
            }
        }

        public void Disconnect(Usc device)
        {
            device.Dispose();
        }
    }
}
