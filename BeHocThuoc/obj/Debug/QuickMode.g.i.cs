﻿

#pragma checksum "C:\Users\Luc\documents\visual studio 2012\Projects\BeHocThuoc\BeHocThuoc\QuickMode.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DE55CBE990181D4045E6D4A85D7A19DA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeHocThuoc
{
    partial class QuickMode : global::BeHocThuoc.Common.LayoutAwarePage
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Data.CollectionViewSource groupedItemsViewSource; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.GridView PlayGround; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button backButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock TimeField; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///QuickMode.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            groupedItemsViewSource = (global::Windows.UI.Xaml.Data.CollectionViewSource)this.FindName("groupedItemsViewSource");
            PlayGround = (global::Windows.UI.Xaml.Controls.GridView)this.FindName("PlayGround");
            backButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("backButton");
            TimeField = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("TimeField");
        }
    }
}



