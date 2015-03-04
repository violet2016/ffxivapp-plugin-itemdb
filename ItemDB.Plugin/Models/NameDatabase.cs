// ItemDB.Plugin
// NameDatabase.cs
// 
// Copyright @2015 Violet Cheng - All Rights Reserved
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
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FFXIVAPP.Common.Core.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
namespace ItemDB.Plugin.Models
{
    public static class NameDatabase
    {
        private static Dictionary<uint, ItemDesc> _itemDictionary = new Dictionary<uint,ItemDesc>();
        public static String filepath = AppDomain.CurrentDomain.BaseDirectory + @"Plugins\FFXIVAPP.Plugin.ItemDB\DescFiles\";
        public static void Initialize()
        {
            
            DirectoryInfo di = new DirectoryInfo(filepath);
            
            if (di.Exists)
            {
                foreach (var fi in di.GetFiles("*.json", SearchOption.TopDirectoryOnly))
                {
                    string jsonContent = "";
                    using (StreamReader sr = new StreamReader(fi.FullName))
                    {
                        jsonContent = sr.ReadToEnd();
                    }
                    List<ItemDesc> tlist = JsonConvert.DeserializeObject<List<ItemDesc>>(jsonContent);
                    foreach(var item in tlist)
                    {
                        _itemDictionary.Add(item.ID, item);
                    }
                    
                }
            }
            
        }
        public static uint GetIDFromName(String iName)
        {
            return 0;
        }

        public static ItemDesc GetDescFromID(uint id)
        {
            if (_itemDictionary.ContainsKey(id))
            {
                return _itemDictionary[id];
            }
            return null;
        }

        public static ItemDesc GetDescFromInfo(ItemInfo info)
        {
            return GetDescFromID(info.ID);

        }

    }
}