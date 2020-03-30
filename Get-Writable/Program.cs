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

            //var uniqDirs = new HashSet<string>(directories);
           

            /*foreach (string dir in directories)
            {
                Console.WriteLine(dir);
            }*/
            /*try
             {
                 foreach (string folder in Directory.GetDirectories(startDir))
                 {
                     subDirs.Add(folder);
                     Console.WriteLine(folder);
                 }
             }
             catch (UnauthorizedAccessException) { }




             foreach (string sub in subDirs)
             {                
                try
                {
                     string[] dirs = Directory.GetDirectories(sub, "*", SearchOption.AllDirectories);
                     subDirs2.Add(sub);
                     foreach (string dir in dirs)
                     {
                         subDirs2.Add(dir);
                     }
                }
                catch (UnauthorizedAccessException) { Console.WriteLine("Cannot read: " + sub); }
             }



             foreach (string sub in subDirs2)
             {
                 Console.WriteLine(sub);
             }


             */
            foreach (string sub in directories)
            {
                DirectoryInfo subInfo = new DirectoryInfo(sub);
                DirectorySecurity subSecurity = subInfo.GetAccessControl();
                foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    if (rule.FileSystemRights == FileSystemRights.FullControl)
                    {
                        if ((rule.IdentityReference.Value == @"BUILTIN\Users") || (rule.IdentityReference.Value == "Everyone"))
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

                            if (dlls.Length != 0 && exes.Length != 0)
                            {
                                DirectoryInfo subInfo = new DirectoryInfo(folder);
                                DirectorySecurity subSecurity = subInfo.GetAccessControl();
                                foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                                {
                                    if (rule.FileSystemRights == FileSystemRights.FullControl)
                                    {
                                        if ((rule.IdentityReference.Value == @"BUILTIN\Users") || (rule.IdentityReference.Value == "Everyone") || (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                                        {
                                            Console.WriteLine("(dll+exe)," + folder);
                                        }
                                    }

                                }

                            }
                            else if (exes.Length != 0 && dlls.Length == 0)
                            {
                                DirectoryInfo subInfo = new DirectoryInfo(folder);
                                DirectorySecurity subSecurity = subInfo.GetAccessControl();
                                foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                                {
                                    if (rule.FileSystemRights == FileSystemRights.FullControl)
                                    {
                                        if ((rule.IdentityReference.Value == @"BUILTIN\Users") || (rule.IdentityReference.Value == "Everyone") || (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                                        {
                                            Console.WriteLine("(exe)," + folder);
                                        }
                                    }

                                }
                            }
                            else if (dlls.Length != 0 && exes.Length == 0)
                            {
                                DirectoryInfo subInfo = new DirectoryInfo(folder);
                                DirectorySecurity subSecurity = subInfo.GetAccessControl();
                                foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                                {
                                    if (rule.FileSystemRights == FileSystemRights.FullControl)
                                    {
                                        if ((rule.IdentityReference.Value == @"BUILTIN\Users") || (rule.IdentityReference.Value == "Everyone") || (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                                        {
                                            Console.WriteLine("(dll)," + folder);
                                        }
                                    }

                                }
                            }
                        }
                        catch (Exception e) { }

                        /*
                        DirectoryInfo subInfo = new DirectoryInfo(folder);
                        DirectorySecurity subSecurity = subInfo.GetAccessControl();
                        foreach (FileSystemAccessRule rule in subSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                        {
                            if (rule.FileSystemRights == FileSystemRights.FullControl)
                            {
                                if ((rule.IdentityReference.Value == @"BUILTIN\Users") || (rule.IdentityReference.Value == "Everyone") || (rule.IdentityReference.Value == @"NT AUTHORITY\Authenticated Users"))
                                {
                                    //Console.WriteLine(folder);
                                }
                            }
                        }*/
                    }
                    catch (UnauthorizedAccessException) { }
                    ShowAllFoldersUnder(folder);
                }
                
            }
            catch (UnauthorizedAccessException) {}
        }
    }
}
