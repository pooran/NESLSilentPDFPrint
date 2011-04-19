using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.InteropServices.Automation;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace NESLSilentPDFPrint
{
    public partial class MainPage : UserControl
    {

        string TempFolderPathString = "C:\\Temp\\";
        string ScriptFileNameString = "GetPrintersListToTextFile.vbs";
        string DownloadPDFScriptFileNameString = "DownloadPDF.vbs";
        string OutputFileNameString = "Printers.txt";
        string TestPDFString = "TestPDF.pdf";
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        { 
            DownloadPDFScript();
            CheckUpdateAvailable(); 
            CheckOOBStatus();
            CheckNESLStatus();
        }

        
        private void InstallNESLButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Silverlight.Windows.Installer.InstallNESL(new Uri("NESLSetup.msi", UriKind.RelativeOrAbsolute), true, true);
            NESLInstallStateTextBlock.Text = "NESL v2 is installed.";
            InstallNESLButton.IsEnabled = false;
            PrintPDFButton.IsEnabled = true;       
        }

     
        private void GetPrintersButton_Click(object sender, RoutedEventArgs e)
        {
            
            WebClient a = new WebClient();
            a.OpenReadAsync(new Uri(ScriptFileNameString, UriKind.Relative));
            a.OpenReadCompleted += (object sender1, OpenReadCompletedEventArgs e1) =>
                {
                    StreamReader reader = new StreamReader(e1.Result);
                    string VBSContents = reader.ReadToEnd();
                    reader.Close();

                    if (AutomationFactory.IsAvailable)
                    {
                        // Interesting, I can simply try to dispose a dynamic object without checking whether it has implemented IDisposible:)
                        using (dynamic fso = AutomationFactory.CreateObject("Scripting.FileSystemObject"))
                        {
                            if (!fso.FolderExists(TempFolderPathString)) fso.CreateFolder(TempFolderPathString);
                            dynamic txtFile = fso.CreateTextFile(TempFolderPathString + ScriptFileNameString);
                            txtFile.WriteLine(VBSContents);
                            txtFile.close();
                        }
                    }
                    dynamic shell = AutomationFactory.CreateObject("Shell.Application");
                    shell.ShellExecute(TempFolderPathString + ScriptFileNameString, "", "", "open", 1);

                    if (AutomationFactory.IsAvailable)
                    {
                        var fileContent = String.Empty;

                        using (dynamic fso = AutomationFactory.CreateObject("Scripting.FileSystemObject"))
                        {
                            dynamic file = fso.OpenTextFile(TempFolderPathString + OutputFileNameString, 1, -1);
                            while (!file.AtEndOfStream)
                                fileContent = fileContent + file.ReadLine() + ",";
                            file.Close();
                        }
                        if (fileContent != "") fileContent = fileContent.Substring(0, fileContent.Length - 1);
                        using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite())
                        {
                            if (isf.FileExists(OutputFileNameString))
                            {
                                isf.DeleteFile(OutputFileNameString);
                            }
                            using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(OutputFileNameString, FileMode.OpenOrCreate, isf))
                            {
                                using (StreamWriter sw = new StreamWriter(isfs))
                                {
                                    sw.Write(fileContent);
                                    sw.Close();
                                }
                            }
                            string[] str = fileContent.Split(',');
                            ListOfPrintersComboBox.IsEnabled = true;
                            foreach (var item in str)
                            {
                                ListOfPrintersComboBox.Items.Add(item);
                            }
                            if (ListOfPrintersComboBox.Items.Count > 0) ListOfPrintersComboBox.SelectedIndex = 0;
                        }
                    }
                };
            PrintPDFButton.IsEnabled = true;
        }
        private void PrintPDFButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic shell = AutomationFactory.CreateObject("Shell.Application");
            shell.ShellExecute(TempFolderPathString + DownloadPDFScriptFileNameString, "\"http://localhost:50163/ClientBin/TestPDF.pdf\", " + "\"C:\\temp\\TestPDF.pdf\", \"" + ListOfPrintersComboBox.SelectedItem.ToString()+"\"", "", "open", 1);
       }

        private void WriteStream(Stream PDFStream, IsolatedStorageFileStream fileStream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = PDFStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }

       
        private void OOBInstallButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Install();
        }

        private void DownloadPDFScript()
        {
            WebClient a = new WebClient();
            a.OpenReadAsync(new Uri(DownloadPDFScriptFileNameString, UriKind.Relative));
            a.OpenReadCompleted += (object sender1, OpenReadCompletedEventArgs e1) =>
            {
                StreamReader reader = new StreamReader(e1.Result);
                string VBSContents = reader.ReadToEnd();
                reader.Close();

                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic fso = AutomationFactory.CreateObject("Scripting.FileSystemObject"))
                    {
                        if (!fso.FolderExists(TempFolderPathString)) fso.CreateFolder(TempFolderPathString);
                        dynamic txtFile = fso.CreateTextFile(TempFolderPathString + DownloadPDFScriptFileNameString);
                        txtFile.WriteLine(VBSContents);
                        txtFile.close();
                    }
                }
            };
        }

        private void CheckUpdateAvailable()
        {
            Application.Current.CheckAndDownloadUpdateAsync();
            Application.Current.CheckAndDownloadUpdateCompleted += (object sender, CheckAndDownloadUpdateCompletedEventArgs e) =>
            {
                if (e.UpdateAvailable)
                    MessageBox.Show("There has been an update to this application. Please restart the application to enjoy the new updates", "Updates!", MessageBoxButton.OK);
                else if (e.Error != null)
                {
                    MessageBox.Show(
                        "Something wrong with the latest update. Please contact administartor for further details");
                }
            };
        }

        private void CheckOOBStatus()
        {
            if (!Application.Current.IsRunningOutOfBrowser)
            {
                if (Application.Current.InstallState != InstallState.Installed)
                {
                    OOBInstallStatusTextBlock.Text = "You need to install the application to proceed further. Click on 'Install App' button";
                    OOBInstallButton.IsEnabled = true;
                }
                else
                {
                    OOBInstallStatusTextBlock.Text = "Application is installed. Please double click on installed app Desktop Icon";
                    OOBInstallButton.IsEnabled = false;
                }
            }
            else
            {
                OOBInstallStatusTextBlock.Text = "Application is installed.";
                OOBInstallButton.IsEnabled = false;
                InstallNESLButton.IsEnabled = true;
                ListOfPrintersComboBox.IsEnabled = false;
            }
        }

        private void CheckNESLStatus()
        {
            if (Microsoft.Silverlight.Windows.Installer.CheckNESLInstalled(2, 0))
            {
                if(Application.Current.IsRunningOutOfBrowser) InstallNESLButton.IsEnabled = false;
                NESLInstallStateTextBlock.Text = "NESL v2 is already installed";
                if (Application.Current.IsRunningOutOfBrowser) GetPrintersButton.IsEnabled = true;
            }
            else
            {
                if (Application.Current.IsRunningOutOfBrowser) InstallNESLButton.IsEnabled = true;
                NESLInstallStateTextBlock.Text = "Please install NESL v2 by clicking on 'Install NESL' button";
            }
        }
    }
}
