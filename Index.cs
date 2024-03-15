using astronomy;
using astronomy.Performables;

namespace Astronomy
{
    public delegate void PerformableAction();

    struct MenuEntry
    {
        public String title;
        public PerformableAction action;

        public MenuEntry(String title, PerformableAction action)
        {
            this.title = title;
            this.action = action;
        }
    }

    internal class Index
    {
        static void Main()
        {
            Env.SetValue("Channel_Count", Utils.GetDeviceCount().ToString());

            List<MenuEntry> list =
            [
                new MenuEntry("Relay controls (external application)", RelayControls.Perform),
                new MenuEntry("Control servo motors", ServoControl.Perform),
                new MenuEntry("Execute XML sequence", ParseXML.Perform),
                new MenuEntry("Schedule", Schedule.Perform),
                new MenuEntry("Global settings", GlobalSettings.Perform),
            ];

            char userOption;


            while (true) {
                Console.WriteLine("CoverControl");
                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine($"{i}) {list[i].title}");
                }
                Console.WriteLine("x) Exit");
                Console.Write("Enter option: ");
                string? raw = Console.ReadLine();

                if (raw == "" || raw == null) continue;
                userOption = raw.ToCharArray()[0];

                Console.WriteLine();

                int index = userOption - '0';
                if (index >= 0 && index < list.Count)
                {
                    list[index].action();
                } else
                {
                    if (char.ToLower(userOption) != 'x') Console.WriteLine($"Invalid input option, please type in number between 0-{list.Count - 1} or x to exit the programme\n");
                }

                if (char.ToLower(userOption) == 'x')
                {
                    Exit.Perform();
                    break;
                }
            }
        }
    }
}