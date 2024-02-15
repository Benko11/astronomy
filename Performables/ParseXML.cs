namespace astronomy.Performables
{
    internal class ParseXML : IPerformable
    {
        public static void Perform()
        {
            var xml = new Xml();
            xml.DoStuff();
        }
    }
}
