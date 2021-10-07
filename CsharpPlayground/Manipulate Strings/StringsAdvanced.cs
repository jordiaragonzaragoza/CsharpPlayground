namespace StringsAdvanced
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    public static class StringsAdvanced
    {
        public static void Start()
        {
            StringBuilder();
            StringBuilderReader();
            MiscStringMethods();
        }

        public static void StringBuilder()
        {
            //Used when you are working with strings in a tight loop
            var sb = new StringBuilder(string.Empty);
            for (var i = 0; i < 10000; i++)
            {
                sb.Append("x");
            }
        }

        public static void StringBuilderReader()
        {
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                writer.WriteStartElement("book");
                writer.WriteElementString("price", "19.95");
                writer.WriteEndElement();
                writer.Flush();
            }
            var xml = stringWriter.ToString();


            var stringReader = new StringReader(xml);
            using (var reader = XmlReader.Create(stringReader))
            {
                reader.ReadToFollowing("price");
                var price = decimal.Parse(reader.ReadInnerXml(), new CultureInfo("en-US")); // Make sure that you read the decimal part correctly
                
            }
        }

        public static void MiscStringMethods()
        {
            var value = "My Sample Value";
            var indexOfp = value.IndexOf('p'); // returns 6
            var lastIndexOfm = value.LastIndexOf('m'); // returns 5

            value = "<mycustominput/>";
            if (value.StartsWith("<")) { }
            if (value.EndsWith(">")) { }

            value = "My Sample Value";
            var subString = value.Substring(3, 6); // Returns ‘Sample’

            var pattern = "(Mr\\.? | Mrs\\.? | Miss | Ms\\.? )";
            string[] names = { "Mr. Henry Hunt", "Ms. Sara Samuels", "Abraham Adams", "Ms. Nicole Norris" };
            foreach (var name in names)
            {
                Console.WriteLine(Regex.Replace(name, pattern, string.Empty));
            }

            double cost = 1234.56;
            Console.WriteLine(cost.ToString("C", new System.Globalization.CultureInfo("en - US")));
            // Displays $1,234.56
        }
    }
}
