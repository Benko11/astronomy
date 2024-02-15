namespace astronomy
{
    internal class FrameConfiguration
    {
        public string Name { get; set; }
        public uint Duration { get; set; }
        public ushort[] Positions { get; set; }

        public FrameConfiguration(string name, uint duration, ushort[] positions)
        {
            Name = name;
            Duration = duration;
            Positions = positions;
        }

        public override string ToString()
        {
            return $"{Name}: ({string.Join(", ", Positions)}) ({Duration}ms)";
        }

        public void Deconstruct(out string name, out uint duration, out ushort[] positions)
        {
            name = Name;
            duration = Duration;
            positions = Positions;
        }
    }
}
