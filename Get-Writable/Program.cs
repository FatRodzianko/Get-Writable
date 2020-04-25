using System;
using System.Management;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Linq;
using System.Text;
using System.Security.Principal;


namespace Get_Writable
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a starting directory to search from. Exiting...");
                System.Environment.Exit(1);
            }

            string startDir = args[0];

            if (!Directory.Exists(startDir))
            {
                Console.Write("Cannot find the " + startDir + " directory. Exiting...");
                System.Environment.Exit(1);
            }

            List<string> subDirs = new List<string>();
            List<string> subDirs2 = new List<string>();
            List<string> directories = new List<string>();

            ShowAllFoldersUnder(startDir);

            foreach (string sub in directories)
            {
                DirectoryInfo subInfo = new DirectoryInfo(sub);
                DirectorySecurity subSecurity = subInfo.GetAccessControl();
                foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    if ((rule.FileSystemRights == FileSystemRights.FullControl)|| (rule.FileSystemRights.HasFlag(FileSystemRights.Write)) || (rule.FileSystemRights.HasFlag(FileSystemRights.Modify)))
                    {
                        if ((rule.IdentityReference.Value == @"BUILTIN\Users") || (rule.IdentityReference.Value == "Everyone") || (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                        {
                            Console.WriteLine(sub);
                        }
                    }
                }
                
            }


        }

        private static void ShowAllFoldersUnder(string path)
        {
                     
            try
            {
                foreach (string folder in Directory.GetDirectories(path))
                {

                    //directories.Add(Path.GetDirectoryName(folder));
                    //Console.WriteLine("{0}", Path.GetDirectoryName(folder));
                    try
                    {
                        try
                        {
                            string[] exes = Directory.GetFiles(folder, "*.EXE");
                            string[] dlls = Directory.GetFiles(folder, "*.DLL");
                            string[] ps1s = Directory.GetFiles(folder, "*.ps1");
                            string[] bats = Directory.GetFiles(folder, "*.bat");
                            string[] scts = Directory.GetFiles(folder, "*.sct");
                            string[] cmds = Directory.GetFiles(folder, "*.cmd");
                            string[] syss = Directory.GetFiles(folder, "*.sys");

                            if (dlls.Length != 0 && exes.Length != 0)
                            {

                                string[] exesANDdlls = exes.Concat(dlls).ToArray();

                                bool checkFiles = CheckExeAndDlls(exesANDdlls, folder, "(exe+dll)");
                                if (checkFiles == false)
                                {
                                    DirectoryInfo subInfo = new DirectoryInfo(folder);
                                    DirectorySecurity subSecurity = subInfo.GetAccessControl();
                                    foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                                    {
                                        if ((rule.FileSystemRights == FileSystemRights.FullControl) || (rule.FileSystemRights.HasFlag(FileSystemRights.Write)) || (rule.FileSystemRights.HasFlag(FileSystemRights.Modify)))
                                        {
                                            if ((rule.IdentityReference.Value == @"BUILTIN\Users") | (rule.IdentityReference.Value == "Everyone") | (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                                            {
                                                Console.WriteLine("(directory+exe)," + folder);
                                            }
                                        }

                                    }
                                }
                            }
                            else if (exes.Length != 0 && dlls.Length == 0)
                            {
                                bool checkFiles = CheckExeAndDlls(exes, folder, "(exe)");
                                if (checkFiles == false)
                                {
                                    DirectoryInfo subInfo = new DirectoryInfo(folder);
                                    DirectorySecurity subSecurity = subInfo.GetAccessControl();
                                    foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                                    {
                                        if ((rule.FileSystemRights == FileSystemRights.FullControl) || (rule.FileSystemRights.HasFlag(FileSystemRights.Write)) || (rule.FileSystemRights.HasFlag(FileSystemRights.Modify)))
                                        {
                                            if ((rule.IdentityReference.Value == @"BUILTIN\Users") | (rule.IdentityReference.Value == "Everyone") | (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                                            {
                                                Console.WriteLine("(directory+exe)," + folder);
                                            }
                                        }

                                    }
                                }
                            }
                            else if (dlls.Length != 0 && exes.Length == 0)
                            {
                                CheckExeAndDlls(dlls, folder, "(dll)");
                            }
                            if (ps1s.Length != 0 || bats.Length != 0 || scts.Length !=0 || cmds.Length !=0)
                            {
                                string[] ps1sANDbats = ps1s.Concat(bats).ToArray();
                                string[] addSCTs = ps1sANDbats.Concat(scts).ToArray();
                                string[] scripts = addSCTs.Concat(cmds).ToArray();
                                CheckExeAndDlls(scripts, folder, "(scripts)");
                            }
                            if (syss.Length != 0)
                            {
                                CheckExeAndDlls(syss, folder, "(drivers)");
                            }
                        }
                        catch (Exception e) { }
                    }
                    catch (UnauthorizedAccessException) { }
                    ShowAllFoldersUnder(folder);
                }
                
            }
            catch (UnauthorizedAccessException) {}
        }
        private static bool CheckExeAndDlls(string[] files, string folder, string fileType)
        {
            foreach (string file in files)
            {
                FileSecurity fileSecurity = File.GetAccessControl(file);
                foreach (FileSystemAccessRule rule in fileSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    if ((rule.FileSystemRights == FileSystemRights.FullControl) || (rule.FileSystemRights.HasFlag(FileSystemRights.Write)) || (rule.FileSystemRights.HasFlag(FileSystemRights.Modify)))
                    {
                        if ((rule.IdentityReference.Value == @"BUILTIN\Users") | (rule.IdentityReference.Value == "Everyone") | (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                        {
                            Console.WriteLine(fileType +"," + folder);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
