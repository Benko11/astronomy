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
            List<MenuEntry> list = new List<MenuEntry>();
            list.Add(new MenuEntry("Relay controls (external application)", RelayControls.Perform));
            list.Add(new MenuEntry("Control servo motors", ServoControl.Perform));
            list.Add(new MenuEntry("Execute XML sequence", ParseXML.Perform));
            list.Add(new MenuEntry("Schedule", Schedule.Perform));
            list.Add(new MenuEntry("Global settings", GlobalSettings.Perform));
            

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
                    Console.WriteLine($"Invalid input option, please type in number between 0-{list.Count - 1}\n");
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