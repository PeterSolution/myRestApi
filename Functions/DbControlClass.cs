using Microsoft.Win32;
using System.Diagnostics;

namespace ServerApi.Functions
{
    public class DbControlClass
    {
        public static void CreateSqlServerInstance(string instanceName)
        {
            // Sprawdź, czy SQL Server jest zainstalowany
            if (!IsSqlServerInstalled())
            {
                throw new Exception("SQL Server nie jest zainstalowany na tym komputerze.");
            }

            // Ścieżka do pliku wykonywalnego SQL Server Setup
            string setupPath = GetSqlServerSetupPath();

            // Przygotuj argumenty dla instalatora
            string arguments = $"/Q /IACCEPTSQLSERVERLICENSETERMS /ACTION=InstallFailoverCluster " +
                               $"/INSTANCENAME={instanceName} /SQLSVCACCOUNT=\"NT AUTHORITY\\SYSTEM\" " +
                               $"/SQLSYSADMINACCOUNTS=\"BUILTIN\\Administrators\" " +
                               $"/AGTSVCACCOUNT=\"NT AUTHORITY\\SYSTEM\" /TCPENABLED=1";

            // Uruchom instalator SQL Server
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = setupPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Nie udało się utworzyć instancji SQL Server.  {process.ExitCode}");
            }
            Console.WriteLine($"Instancja SQL Server '{instanceName}' została pomyślnie utworzona.");
        }

        private static bool IsSqlServerInstalled()
        {
            string registryPath = @"SOFTWARE\Microsoft\Microsoft SQL Server";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath))
            {
                return key != null;
            }
        }

        private static string GetSqlServerSetupPath()
        {
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string setupPath = Path.Combine(programFiles, "Microsoft SQL Server", "150", "Setup Bootstrap", "SQLServer2019", "setup.exe");

            if (!File.Exists(setupPath))
            {
                throw new FileNotFoundException("Nie można znaleźć pliku instalacyjnego SQL Server.");
            }

            return setupPath;
        }

    }
}
