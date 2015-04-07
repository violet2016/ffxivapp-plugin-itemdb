// ItemDB.Plugin
// MainViewModel.cs
// 
// Copyright @2015 VioletCheng - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using ItemDB.Plugin.Models;
using ItemDB.Plugin.Views;
using ItemDB.Plugin.Utilities;
namespace ItemDB.Plugin.ViewModels
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
        }

        #endregion

        #region Declarations
        public ICommand SearchItemCommand { get; private set; }
        public ICommand UpdateHire1Command { get; private set; }
        public ICommand UpdateHire2Command { get; private set; }
       
        #endregion
        public MainViewModel()
        {
            SearchItemCommand = new DelegateCommand(SearchItem);
            UpdateHire1Command = new DelegateCommand(UpdateHire1);
            UpdateHire2Command = new DelegateCommand(UpdateHire2);
           
        }
        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings
        public static void SearchItem()
        {
            var searchItem = new ItemFilter
            {
                Key = MainView.View.TKey.Text
            };
            if (SharedRegEx.IsValidRegex(searchItem.Key))
            {
                searchItem.RegEx = new Regex(searchItem.Key, SharedRegEx.DefaultOptions);
            }
            if (!String.IsNullOrWhiteSpace(searchItem.Key))
            {
                PluginViewModel.Instance.Filter = searchItem;
            }
            MainView.View.TKey.Text = "";
        }
        private static void UpdateHire1()
        {
            
            ItemPublisher.ProcessHire(1);
        }
        private static void UpdateHire2()
        {
            
            ItemPublisher.ProcessHire(2);
        }
        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
