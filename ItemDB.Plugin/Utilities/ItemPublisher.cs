// ItemDB.Plugin
// ItemPublisher.cs
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
using System.IO;
using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Utilities;
using NLog;
using ItemDB.Plugin.Properties;
using ItemDB.Plugin.Views;
using ItemDB.Plugin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using FFXIVAPP.Common.Core.Memory.Enums;
namespace ItemDB.Plugin.Utilities
{
    public static class ItemPublisher
    {
        public static List<ItemFullInfo> HireCatalog = new List<ItemFullInfo>();
        //TODO remove this when character name bug is fixed
        public static bool logout = false;
        private static void WriteCatalog(List<ItemFullInfo> list, int hire = 0)
        {
            if (Constants.CharacterName == "")
            {
                return;
            }
            var userPath = Path.Combine(NameDatabase.filepath, "Users");
            DirectoryInfo di = new DirectoryInfo(userPath);

            if (!di.Exists)
            {
                di.Create();
            }
            String suffix;
            if (hire == 0)
            {
                suffix= Constants.CharacterName + ".json";
            }
            else {
                suffix= Constants.CharacterName + "-雇员"+hire.ToString()+".json";
            }
            var fileName = Path.Combine(userPath, suffix);

            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            string xx = JsonConvert.SerializeObject(list);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xx);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }
        public static void ProcessHire(int hire)
        {
            if (hire == 0)
            {
                return;
            }
            else
            {
                if (HireCatalog.Count > 0)
                {
                    WriteCatalog(HireCatalog, hire);
                }
            }
        }
        private static uint GetAllItemAmount(List<InventoryEntity> inventoryEntry)
        {
            uint count = 0;
            foreach(var container in inventoryEntry)
            {
                count += container.Amount;
            }
            return count;
        }
        public static void Process(List<InventoryEntity> inventoryEntry)
        {
            try
            {
                if (GetAllItemAmount(inventoryEntry) == 0)
                {
                    logout = true;
                    return;
                }
                if (logout == true)
                {
                    //skip the first saving after logout
                    logout = false;
                    return;
                }
                List<ItemFullInfo> allItems = new List<ItemFullInfo>();
                HireCatalog.Clear();
                foreach (var container in inventoryEntry)
                {
                    if (container.Amount > 0)
                    {
                        foreach (var item in container.Items)
                        {
                            var lItem = new ItemFullInfo();
                            lItem.Info = item;
                            lItem.Desc = NameDatabase.GetDescFromInfo(item);
                            lItem.Location = new Location();
                            lItem.Location.Type = container.Type;
                            lItem.Location.Character = Constants.CharacterName;
                            if (container.Type < Inventory.Container.HIRE_1 || container.Type > Inventory.Container.HIRE_7)
                            { 
                                allItems.Add(lItem); 
                            }
                            else
                            {
                                HireCatalog.Add(lItem);
                            }
                            
                        }
                        
                    }

                }
                 
                WriteCatalog(allItems); 
                
                
               
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }
    }
}