﻿#pragma checksum "..\..\TogVisWPF.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B756CE288F9AE29FAE0564E694C0742B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using RevitAddinKeyNoteSystem;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace RevitAddinKeyNoteSystem {
    
    
    /// <summary>
    /// FormTogVisWPF
    /// </summary>
    public partial class FormTogVisWPF : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\TogVisWPF.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label MsgLabelTop;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\TogVisWPF.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label MsgLabelBot;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\TogVisWPF.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Button11;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\TogVisWPF.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Button12;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\TogVisWPF.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Button13;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/RevitAddinKeyNoteSystem;component/togviswpf.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TogVisWPF.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\TogVisWPF.xaml"
            ((RevitAddinKeyNoteSystem.FormTogVisWPF)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 12 "..\..\TogVisWPF.xaml"
            ((RevitAddinKeyNoteSystem.FormTogVisWPF)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 17 "..\..\TogVisWPF.xaml"
            ((RevitAddinKeyNoteSystem.FormTogVisWPF)(target)).LocationChanged += new System.EventHandler(this.Window_LocationChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 29 "..\..\TogVisWPF.xaml"
            ((System.Windows.Controls.DockPanel)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.DockPanel_MouseEnter);
            
            #line default
            #line hidden
            
            #line 30 "..\..\TogVisWPF.xaml"
            ((System.Windows.Controls.DockPanel)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.DockPanel_MouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.MsgLabelTop = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.MsgLabelBot = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.Button11 = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.Button12 = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.Button13 = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

