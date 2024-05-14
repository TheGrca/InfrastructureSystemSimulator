﻿#pragma checksum "..\..\..\Views\NetworkEntitiesView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6516188E76F27AE1884D8720B2C004EAFEB94EC84EECDCBCD961E808B967631E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NetworkService.Model;
using NetworkService.ViewModel;
using NetworkService.Views;
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


namespace NetworkService.Views {
    
    
    /// <summary>
    /// NetworkEntitiesView
    /// </summary>
    public partial class NetworkEntitiesView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 60 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView EntityListView;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IdTextBox;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label IDErrorLabel;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NameTextBox;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label NameErrorLabel;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image PictureDisplay;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelectImageButton;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ImageErrorLabel;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\Views\NetworkEntitiesView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid KeyboardGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/NetworkService;component/views/networkentitiesview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\NetworkEntitiesView.xaml"
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
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 60 "..\..\..\Views\NetworkEntitiesView.xaml"
            this.SearchTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.TextBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\Views\NetworkEntitiesView.xaml"
            this.SearchTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.TextBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.EntityListView = ((System.Windows.Controls.ListView)(target));
            return;
            case 3:
            this.IdTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 100 "..\..\..\Views\NetworkEntitiesView.xaml"
            this.IdTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.TextBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 100 "..\..\..\Views\NetworkEntitiesView.xaml"
            this.IdTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.TextBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.IDErrorLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.NameTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 105 "..\..\..\Views\NetworkEntitiesView.xaml"
            this.NameTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.TextBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 105 "..\..\..\Views\NetworkEntitiesView.xaml"
            this.NameTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.TextBox_LostFocus);
            
            #line default
            #line hidden
            return;
            case 6:
            this.NameErrorLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.PictureDisplay = ((System.Windows.Controls.Image)(target));
            return;
            case 8:
            this.SelectImageButton = ((System.Windows.Controls.Button)(target));
            
            #line 114 "..\..\..\Views\NetworkEntitiesView.xaml"
            this.SelectImageButton.Click += new System.Windows.RoutedEventHandler(this.SelectImageButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ImageErrorLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.KeyboardGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 11:
            
            #line 143 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 144 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 145 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 146 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 147 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 148 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 149 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 150 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 151 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 152 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 155 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 156 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 23:
            
            #line 157 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 24:
            
            #line 158 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 25:
            
            #line 159 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 26:
            
            #line 160 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 27:
            
            #line 161 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 28:
            
            #line 162 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 29:
            
            #line 163 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 30:
            
            #line 164 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 31:
            
            #line 167 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 32:
            
            #line 168 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 33:
            
            #line 169 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 34:
            
            #line 170 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 35:
            
            #line 171 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 36:
            
            #line 172 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 37:
            
            #line 173 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 38:
            
            #line 174 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 39:
            
            #line 175 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 40:
            
            #line 179 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 41:
            
            #line 180 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 42:
            
            #line 181 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 43:
            
            #line 182 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 44:
            
            #line 183 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 45:
            
            #line 184 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 46:
            
            #line 185 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 47:
            
            #line 186 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 48:
            
            #line 188 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 49:
            
            #line 189 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 50:
            
            #line 190 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            case 51:
            
            #line 191 "..\..\..\Views\NetworkEntitiesView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.KeyboardButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

