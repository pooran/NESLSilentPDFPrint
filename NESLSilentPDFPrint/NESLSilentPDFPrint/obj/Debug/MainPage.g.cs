﻿#pragma checksum "e:\NESLSilentPDFPrint\NESLSilentPDFPrint\NESLSilentPDFPrint\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A36558A6D9C5ECD3C40D34323B809ACF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace NESLSilentPDFPrint {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button InstallNESLButton;
        
        internal System.Windows.Controls.Button GetPrintersButton;
        
        internal System.Windows.Controls.Button PrintPDFButton;
        
        internal System.Windows.Controls.TextBlock NESLInstallStateTextBlock;
        
        internal System.Windows.Controls.ComboBox ListOfPrintersComboBox;
        
        internal System.Windows.Controls.TextBlock PDFPrintingStatusTextBlock;
        
        internal System.Windows.Controls.Button OOBInstallButton;
        
        internal System.Windows.Controls.TextBlock OOBInstallStatusTextBlock;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/NESLSilentPDFPrint;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.InstallNESLButton = ((System.Windows.Controls.Button)(this.FindName("InstallNESLButton")));
            this.GetPrintersButton = ((System.Windows.Controls.Button)(this.FindName("GetPrintersButton")));
            this.PrintPDFButton = ((System.Windows.Controls.Button)(this.FindName("PrintPDFButton")));
            this.NESLInstallStateTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NESLInstallStateTextBlock")));
            this.ListOfPrintersComboBox = ((System.Windows.Controls.ComboBox)(this.FindName("ListOfPrintersComboBox")));
            this.PDFPrintingStatusTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PDFPrintingStatusTextBlock")));
            this.OOBInstallButton = ((System.Windows.Controls.Button)(this.FindName("OOBInstallButton")));
            this.OOBInstallStatusTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("OOBInstallStatusTextBlock")));
        }
    }
}

