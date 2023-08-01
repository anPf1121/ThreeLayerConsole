using MySqlConnector;
using System.Diagnostics;

namespace DAL
{
    public class DbConfig
    {
        private static MySqlConnection connection = new MySqlConnection();
        private DbConfig() { }
        public static MySqlConnection GetDefaultConnection()
        {
            return GetConnection("server=localhost;user id=root;password=123456;port=3306;database=phonestore;IgnoreCommandTransaction=true;");
        }
        public static bool CreateDefaultDb()
        {
            string sqlFilePath = "phonestore.sql";
            string mysqlPath = @"C:\DatabaseMySQL\MySQL\MySQL Server 8.0\bin\mysql.exe"; // Path to mysql.exe
            string username = "root";
            string hostname = "localhost";
            string password = "123456";

            string arguments = $"-u {username} -h {hostname} -p {password} < \"{sqlFilePath}\"";

            try
            {
                Console.WriteLine("Arguments: " + arguments);
                Process process = new Process();
                process.StartInfo.FileName = mysqlPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                // Set up event handlers for asynchronous reading
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Wait for the process to complete, with a timeout to prevent potential deadlocks
                bool exited = process.WaitForExit(5000); // Adjust the timeout as needed

                if (exited)
                {
                    Console.WriteLine("Execution completed successfully.");
                }
                else
                {
                    Console.WriteLine("Execution timed out or encountered an error.");
                    // Optionally, you can kill the process if it doesn't exit within the timeout
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
            }
            return true;
        }
        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                // Console.WriteLine("Output: " + e.Data);
            }
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                // Console.WriteLine("Error: " + e.Data);
            }
        }
        public static MySqlConnection GetConnection()
        {
            try
            {
                string conString;
                using (System.IO.FileStream fileStream = System.IO.File.OpenRead("DbConfig.txt"))
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    conString = reader.ReadLine() ?? "server=localhost;user id=root;password=123456;port=3306;database=phonestore;IgnoreCommandTransaction=true;";
                }

                if (!conString.Contains("IgnoreCommandTransaction=true"))
                {
                    conString += "IgnoreCommandTransaction=true;";
                }
                Console.ReadLine();
                return GetConnection(conString);
            }
            catch
            {
                return GetDefaultConnection();
            }
        }
        public static MySqlConnection GetConnection(string connectionString)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.ConnectionString = connectionString;

            }
            return connection;
        }
    }
}