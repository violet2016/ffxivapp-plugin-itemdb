// ItemDB.Plugin
// ItemWarehouse.cs
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ItemDB.Plugin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using FFXIVAPP.Common.Core.Memory.Enums;
using ItemDB.Plugin.Models;
using FFXIVAPP.Common.RegularExpressions;
namespace ItemDB.Plugin.Models
{
    public static class ItemWarehouse
    {
        private static Dictionary<Inventory.Container, String> _locationName = new Dictionary<Inventory.Container, String>
        {
            {Inventory.Container.INVENTORY_1, "物品"}, 
            {Inventory.Container.INVENTORY_2, "物品"}, 
            {Inventory.Container.INVENTORY_3, "物品"},
            {Inventory.Container.INVENTORY_4, "物品"},
            {Inventory.Container.CURRENT_EQ, "当前装备"},
            {Inventory.Container.CRYSTALS, "物品-水晶"},
            {Inventory.Container.EXTRA_EQ, "货币"},
            {Inventory.Container.AC_MH, "兵装库-主手"},
            {Inventory.Container.AC_OH, "兵装库-副手"},
            {Inventory.Container.AC_HEAD, "兵装库-头部"},
            {Inventory.Container.AC_BODY, "兵装库-身体"},
            {Inventory.Container.AC_HANDS, "兵装库-手臂"},
            {Inventory.Container.AC_BELT, "兵装库-腰带"},
            {Inventory.Container.AC_LEGS, "兵装库-腿部"},
            {Inventory.Container.AC_FEET, "兵装库-脚部"},
            {Inventory.Container.AC_EARRINGS, "兵装库-耳部"},
            {Inventory.Container.AC_NECK, "兵装库-颈部"},
            {Inventory.Container.AC_WRISTS, "兵装库-腕部"},
            {Inventory.Container.AC_RINGS, "兵装库-戒指"},
            {Inventory.Container.AC_SOULS, "兵装库-灵魂水晶"},
            {Inventory.Container.HIRE_1, "雇员"},
            {Inventory.Container.HIRE_2, "雇员"},
            {Inventory.Container.HIRE_3, "雇员"},
            {Inventory.Container.HIRE_4, "雇员"},
            {Inventory.Container.HIRE_5, "雇员"},
            {Inventory.Container.HIRE_6, "雇员"},
            {Inventory.Container.HIRE_7, "雇员"},
            {Inventory.Container.COMPANY_1, "部队储物柜-1"},
            {Inventory.Container.COMPANY_2, "部队储物柜-2"},
            {Inventory.Container.COMPANY_3, "部队储物柜-3"},
            {Inventory.Container.COMPANY_CRYSTALS, "部队储物柜-水晶"},
            
            
        };
        private static Dictionary<String, List<ItemFullInfo>> _warehouse = new Dictionary<String, List<ItemFullInfo>>();
        private static void LoadWarehouse()
        {
            _warehouse.Clear();
            var userPath = Path.Combine(NameDatabase.filepath, "Users");
            DirectoryInfo di = new DirectoryInfo(userPath);

            if (di.Exists)
            {
                foreach (var fi in di.GetFiles("*.json", SearchOption.TopDirectoryOnly))
                {
                    string jsonContent = "";
                    using (StreamReader sr = new StreamReader(fi.FullName))
                    {
                        jsonContent = sr.ReadToEnd();
                        sr.Close();
                    }
                    List<ItemFullInfo> tlist = JsonConvert.DeserializeObject<List<ItemFullInfo>>(jsonContent);
                   
                    _warehouse.Add(fi.Name, tlist);
                    

                }
            }
        }
        public static List<ItemShow> SearchItemInItemWarehouse (ItemFilter filter)
        {
            LoadWarehouse();
            List<ItemShow> result = new List<ItemShow>();
            Regex regex = new Regex(@"-雇员(?<id>\d+)");
            foreach (KeyValuePair<String, List<ItemFullInfo>> catalog in _warehouse)
            {
				foreach (var item in catalog.Value)
                {

                    if (item.Desc != null && item.Desc.Name != null && filter.RegEx.IsMatch(item.Desc.Name))
                    {
                        if (_locationName.ContainsKey(item.Location.Type))
                        {
                            String lLocation = _locationName[item.Location.Type];
                            Match m = regex.Match(catalog.Key);
                            if (m.Success)
                            {
                                lLocation += m.Groups["id"].Value;

                            }
                            if (item.Info.IsHQ)
                            {
                                item.Desc.Name += "[HQ]";
                            }
                            ItemShow i = new ItemShow() { Key = item.Desc.Name, Character = item.Location.Character, Location = lLocation, Amount = item.Info.Amount };
                            result.Add(i);
                        }
                        
                        
                    }
                }
            }

            return result;
        }
    }
}
