namespace autocli.Functionnals
{
    public static class Utils
    {
        // Method to Box the title of the application
        public static string Boxed(string title)
        {
            int l = title.Length;
            string box = "";
            for (int c = 1; c < l; c++)
            {
                box += "_";
            }
            box += "\n\n";
            return box + title + "\n\n" + box +
                $"Author : scalar-tns.\nHost name : {Environment.MachineName}\n" +
                $"OS : {Environment.OSVersion}\n" +
                $"Host version : .NET {Environment.Version}\n\n";
        }

        // Method to automatically locate app.json input file
        public static string Locate(string input, string keyword)
        {
            string crit = string.IsNullOrEmpty(input) ? keyword : input;
            string[] data_file = Directory.GetFiles(Directory.GetCurrentDirectory(), crit);
            return data_file[^1];
        }
    }
}