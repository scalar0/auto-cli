using System.Diagnostics;

namespace autocli.Functionnals
{
    internal static class Utils
    {
        /// <summary>
        /// Method to box the title between 2 "___...___" lines.
        /// </summary>
        /// <param name="title">Title of the application to be boxed and displayed in the cli.</param>
        /// <example></example>
        internal static string Boxed(string title)
        {
            string box = "";
            int c = 0;
            do
            {
                box += "_";
                c++;
            }
            while (c < title.Length);
            box += "\n\n";
            return box + title + "\n\n" + box +
                $"Author : scalar-tns.\nHost name : {Environment.MachineName}\n" +
                $"OS : {Environment.OSVersion}\n" +
                $"Host version : .NET {Environment.Version}\n\n";
        }

        internal static void ExecuteCommandSync(string command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as
                // the parameters. Incidentally, /c tells cmd that we want it to execute the command
                // that follows, and then exit.
                ProcessStartInfo procStartInfo = new("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output. This means
                // that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                Process proc = new();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                Console.WriteLine(result);
            }
            catch (Exception ex) { Log.Error(ex, ex.Message, ex.ToString); }
        }
    }
}