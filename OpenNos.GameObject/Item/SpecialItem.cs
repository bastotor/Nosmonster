﻿/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core;
using OpenNos.Core.Extensions;
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Linq;
using OpenNos.GameObject.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenNos.GameObject.Extension;
using OpenNos.GameObject.Extension.Inventory;

namespace OpenNos.GameObject
{
    public class SpecialItem : Item
    {
        #region Instantiation

        public SpecialItem(ItemDTO item) : base(item)
        {
        }

        public MapNpc mapNpc { get; set; }

        public CharacterSkill characterSkill { get; set; }

        #endregion

        #region Methods

        public override void Use(ClientSession session, ref ItemInstance inv, byte Option = 0, string[] packetsplit = null)
        {
            short itemDesign = inv.Design;

            #region BoxItem

            List<BoxItemDTO> boxItemDTOs = ServerManager.Instance.BoxItems.Where(boxItem => boxItem.OriginalItemVNum == VNum && boxItem.OriginalItemDesign == itemDesign).ToList();

            if (boxItemDTOs.Any())
            {
                session.Character.Inventory.RemoveItemFromInventory(inv.Id);

                foreach (BoxItemDTO boxItemDTO in boxItemDTOs)
                {
                    if (ServerManager.RandomNumber() < boxItemDTO.Probability)
                    {
                        session.Character.GiftAdd(boxItemDTO.ItemGeneratedVNum, boxItemDTO.ItemGeneratedAmount, boxItemDTO.ItemGeneratedRare, boxItemDTO.ItemGeneratedUpgrade, boxItemDTO.ItemGeneratedDesign);
                    }
                }

                return;
            }

            #endregion

            if (VNum == 9723 || VNum == 9722 || VNum == 9725 || VNum == 9726 || VNum == 9727 || VNum == 9728 || VNum == 9729 || VNum == 9730 || VNum == 9731)
            {
                if (session.Character.LastUse.AddSeconds(2) >= DateTime.Now)//bastoi
                {
                    session.SendPacket(UserInterfaceHelper.GenerateMsg($"Need wait 2 seconds", 0));
                    return;
                }
            }

            if (VNum == 1456) //Perfection Reset
            {
                if (session.Character.UseSp)
                {
                    ItemInstance specialistInstance1 = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
                    if (specialistInstance1 != null)
                    {
                        specialistInstance1.SpDamage = 0;
                        specialistInstance1.SpDark = 0;
                        specialistInstance1.SpDefence = 0;
                        specialistInstance1.SpElement = 0;
                        specialistInstance1.SpFire = 0;
                        specialistInstance1.SpHP = 0;
                        specialistInstance1.SpLight = 0;
                        specialistInstance1.SpStoneUpgrade = 0;
                        specialistInstance1.SpWater = 0;
                        session.Character.Inventory.RemoveItemAmount(1456, 1);
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RESET_PERFECTION"), 10));
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("TRANSFORMATION_NEEDED"), 10));
                    }
                }
            }
            if (VNum == 1457) //Perfection Reset Permanent
            {
                if (session.Character.UseSp)
                {
                    ItemInstance specialistInstance1 = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
                    if (specialistInstance1 != null)
                    {
                        specialistInstance1.SpDamage = 0;
                        specialistInstance1.SpDark = 0;
                        specialistInstance1.SpDefence = 0;
                        specialistInstance1.SpElement = 0;
                        specialistInstance1.SpFire = 0;
                        specialistInstance1.SpHP = 0;
                        specialistInstance1.SpLight = 0;
                        specialistInstance1.SpStoneUpgrade = 0;
                        specialistInstance1.SpWater = 0;
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RESET_PERFECTION"), 10));
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("TRANSFORMATION_NEEDED"), 10));
                    }
                }
            }

            int CClassType = 0;
            if (VNum == 30039) // ChangeClass Swordman
            {
                CClassType = 1;
                if (session.Character.Level < 15 || session.Character.JobLevel < 20)
                {
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("LOW_LVL"), 0));
                    return;
                }
                if (CClassType == (byte)session.Character.Class)
                {
                    return;
                }
                if (session.Character.Inventory.All(i => i.Type != InventoryType.Wear))
                {
                    session.Character.Inventory.AddNewToInventory((short)(4 + (CClassType * 14)), 1, InventoryType.Wear);
                    session.Character.Inventory.AddNewToInventory((short)(81 + (CClassType * 13)), 1, InventoryType.Wear);
                    session.Character.Inventory.AddNewToInventory(68, 1, InventoryType.Wear);
                    session.Character.ChangeClass((ClassType)CClassType, false);
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                }
                else
                {
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("EQ_NOT_EMPTY"), 0));
                }
            }
            if (VNum == 30040)
            {
                CClassType = 2;
            }
            if (VNum == 30041)
            {
                CClassType = 3;
            }
            if (VNum == 30042)
            {
                CClassType = 4;
            }
            //Start ChangeClass
            if (CClassType != 0)
            {
                int error_check = 0; //so it does not dissapear if an error appears

                if (session.Character.Level < 15 || session.Character.JobLevel < 20)
                {

                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("LOW_LVL"), 0));
                    error_check = 1;
                    return;
                }
                if (session.Character.Level < 81 && CClassType == 4 || session.Character.JobLevel < 20 && CClassType == 4)
                {
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("LOW_LVL") + " (81+)", 0));
                    error_check = 1;
                    return;
                }
                if (CClassType > 4 || CClassType < 1)
                {
                    error_check = 1;
                    return;
                }
                if (CClassType == (byte)session.Character.Class)
                {
                    error_check = 1;
                    return;
                }
                if (session.Character.Inventory.All(i => i.Type != InventoryType.Wear))
                {
                    session.Character.Inventory.AddNewToInventory((short)(4 + (CClassType * 14)), 1, InventoryType.Wear);
                    session.Character.Inventory.AddNewToInventory((short)(81 + (CClassType * 13)), 1, InventoryType.Wear);

                    switch (CClassType)
                    {
                        case 1:
                            session.Character.Inventory.AddNewToInventory(68, 1, InventoryType.Wear);
                            break;

                        case 2:
                            session.Character.Inventory.AddNewToInventory(78, 1, InventoryType.Wear);
                            break;

                        case 3:
                            session.Character.Inventory.AddNewToInventory(86, 1, InventoryType.Wear);
                            break;
                        case 4:
                            //TODO add items
                            session.Character.SendGift(session.Character.CharacterId, 5832, 1, 0, 0, 0, false);
                            break;

                    }

                    session.CurrentMapInstance?.Broadcast(session.Character.GenerateEq());
                    session.SendPacket(session.Character.GenerateEquipment());
                    session.Character.ChangeClass((ClassType)CClassType, false);
                }
                else
                {
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("EQ_NOT_EMPTY"), 0));
                    error_check = 2;
                }

                if (error_check == 0)
                {
                    //session.SendPacket(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 6000));
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                }
                return;
            }
            //End Change Class

            if (session.Character.IsVehicled && Effect != 1000)
            {
                if (VNum == 5119 || VNum == 9071) // Speed Booster
                {
                    if (!session.Character.Buff.Any(s => s.Card.CardId == 336))
                    {
                        session.Character.VehicleItem.BCards.ForEach(s => s.ApplyBCards(session.Character.BattleEntity, session.Character.BattleEntity));
                        session.CurrentMapInstance.Broadcast($"eff 1 {session.Character.CharacterId} 885");
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    }
                }
                else
                {
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("CANT_DO_VEHICLED"), 10));
                }
                return;
            }

            if (inv.ItemVNum >= 9300 && inv.ItemVNum <= 9440)
            {
                session.Character.Title.Add(new CharacterTitleDTO
                {
                    CharacterId = session.Character.CharacterId,
                    Stat = 1,
                    TitleVnum = inv.ItemVNum
                });

                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NEW_TITLE"), 0));

                session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                session.SendPacket(session.Character.GenerateTitle());
            }

            if (VNum == 5511)
            {
                session.Character.GeneralLogs.Where(s => s.LogType == "InstanceEntry" && (short.Parse(s.LogData) == 16 || short.Parse(s.LogData) == 17) && s.Timestamp.Date == DateTime.Today).ToList().ForEach(s =>
                {
                    s.LogType = "NulledInstanceEntry";
                    DAOFactory.GeneralLogDAO.InsertOrUpdate(ref s);
                });
                session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                return;
            }

            if (session.CurrentMapInstance?.MapInstanceType != MapInstanceType.TalentArenaMapInstance
            && (VNum == 5936 || VNum == 5937 || VNum == 5938 || VNum == 5939 || VNum == 5940 || VNum == 5942 || VNum == 5943 || VNum == 5944 || VNum == 5945 || VNum == 5946))
            {
                return;
            }
            if (session.CurrentMapInstance?.MapInstanceType == MapInstanceType.TalentArenaMapInstance
            && VNum != 5936 && VNum != 5937 && VNum != 5938 && VNum != 5939 && VNum != 5940 && VNum != 5942 && VNum != 5943 && VNum != 5944 && VNum != 5945 && VNum != 5946)
            {
                return;
            }

            if (BCards.Count > 0 && Effect != 1000)
            {
                if (BCards.Any(s => s.Type == (byte)BCardType.CardType.Buff && s.SubType == 1 && new Buff((short)s.SecondData, session.Character.Level).Card.BCards.Any(newbuff => session.Character.Buff.GetAllItems().Any(b => b.Card.BCards.Any(buff =>
                    buff.CardId != newbuff.CardId
                 && ((buff.Type == 33 && buff.SubType == 5 && (newbuff.Type == 33 || newbuff.Type == 58)) || (newbuff.Type == 33 && newbuff.SubType == 5 && (buff.Type == 33 || buff.Type == 58))
                 || (buff.Type == 33 && (buff.SubType == 1 || buff.SubType == 3) && (newbuff.Type == 58 && (newbuff.SubType == 1))) || (buff.Type == 33 && (buff.SubType == 2 || buff.SubType == 4) && (newbuff.Type == 58 && (newbuff.SubType == 3)))
                 || (newbuff.Type == 33 && (newbuff.SubType == 1 || newbuff.SubType == 3) && (buff.Type == 58 && (buff.SubType == 1))) || (newbuff.Type == 33 && (newbuff.SubType == 2 || newbuff.SubType == 4) && (buff.Type == 58 && (buff.SubType == 3)))
                 || (buff.Type == 33 && newbuff.Type == 33 && buff.SubType == newbuff.SubType) || (buff.Type == 58 && newbuff.Type == 58 && buff.SubType == newbuff.SubType)))))))
                {
                    return;
                }
                BCards.ForEach(c => c.ApplyBCards(session.Character.BattleEntity, session.Character.BattleEntity));
                session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                return;
            }
            
            switch (Effect)

            {

                
                // Seal Mini-Game
                case 1717:
                    switch (EffectValue)
                    {
                        case 1:// King Ratufu Mini Game
                               // Not Created for moment .
                            break;
                        case 2: // Sheep Mini Game
                            session.SendPacket($"say 1 {session.Character.CharacterId} 10 Registration starts in 5 seconds.");
                            //Need keys
                            EventHelper.GenerateEvent(EventType.SHEEPGAME);
                            break;
                        case 3: // Meteor Mini Game
                            session.SendPacket($"say 1 {session.Character.CharacterId} 10 Registration starts in 5 seconds.");
                            EventHelper.GenerateEvent(EventType.METEORITEGAME);
                            break;
                    }
                    break;

                ////btk register
                case 1227:
                    if (ServerManager.Instance.CanRegisterRainbowBattle == true)
                    {
                        if (session.Character.Family != null)
                        {
                            if (session.Character.Family.FamilyCharacters.Where(s => s.CharacterId == session.Character.CharacterId).First().Authority == FamilyAuthority.Head || session.Character.Family.FamilyCharacters.Where(s => s.CharacterId == session.Character.CharacterId).First().Authority == FamilyAuthority.Familykeeper)
                            {
                                if (ServerManager.Instance.IsCharacterMemberOfGroup(session.Character.CharacterId))
                                {
                                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RAINBOWBATTLE_OPEN_GROUP"), 12));
                                    return;
                                }
                                Group group = new Group
                                {
                                    GroupType = GroupType.BigTeam
                                };
                                group.JoinGroup(session.Character.CharacterId);
                                ServerManager.Instance.AddGroup(group);
                                session.SendPacket(session.Character.GenerateFbt(2));
                                session.SendPacket(session.Character.GenerateFbt(0));
                                session.SendPacket(session.Character.GenerateFbt(1));
                                session.SendPacket(group.GenerateFblst());
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("RAINBOWBATTLE_LEADER"), session.Character.Name), 0));
                                session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("RAINBOWBATTLE_LEADER"), session.Character.Name), 10));

                                ServerManager.Instance.RainbowBattleMembers.Add(new RainbowBattleMember
                                {
                                    RainbowBattleType = EventType.RAINBOWBATTLE,
                                    Session = session,
                                    GroupId = group.GroupId,
                                });

                                ItemInstance RainbowBattleSeal = session.Character.Inventory.LoadBySlotAndType(inv.Slot, InventoryType.Main);
                                session.Character.Inventory.RemoveItemFromInventory(RainbowBattleSeal.Id);
                            }
                        }
                    }
                    break;

                //Wing Remover
                case 8999:
                    {
                        ItemInstance specialistInstance = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);

                        if (session.Character.UseSp && specialistInstance != null && !session.Character.IsSeal && session.Character.LastPotion < DateTime.Now)
                        {
                            {
                                if (specialistInstance.Design == 1)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(1685, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 2)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(1686, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 3)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5087, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 4)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5203, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 6)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5372, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 7)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(4531, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 8)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5432, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 9)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5498, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 10)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5499, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 11)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5553, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 12)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5560, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 13)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5591, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 14)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5837, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 15)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5702, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 16)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5800, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 17)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9176, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 18)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9212, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 19)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9242, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            {
                                if (specialistInstance.Design == 20)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9441, 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                            session.Character.LastPotion = DateTime.Now.AddSeconds(2);
                            session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPackets(session.Character.GenerateStatChar());
                            session.SendPacket(session.Character.GenerateLev());
                            session.Character.RemoveVehicle();
                        }
                    }
                    break;

                //Wing Remover Permanent
                case 8990:
                    {
                        ItemInstance specialistInstance = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);

                        if (session.Character.UseSp && specialistInstance != null && !session.Character.IsSeal && session.Character.LastPotion < DateTime.Now)
                        {
                            {
                                if (specialistInstance.Design == 1)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(1685, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 2)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(1686, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 3)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5087, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 4)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5203, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 6)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5372, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 7)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(4531, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 8)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5432, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 9)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5498, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 10)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5499, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 11)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5553, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 12)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5560, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 13)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5591, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 14)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5837, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 15)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5702, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 16)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(5800, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 17)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9176, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 18)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9212, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 19)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9242, 1);
                                }
                            }
                            {
                                if (specialistInstance.Design == 20)
                                {
                                    specialistInstance.Design = 0;
                                    session.Character.GiftAdd(9441, 1);
                                }
                            }
                            session.Character.LastPotion = DateTime.Now.AddSeconds(2);
                            session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPackets(session.Character.GenerateStatChar());
                            session.SendPacket(session.Character.GenerateLev());
                            session.Character.RemoveVehicle();
                        }
                    }
                    break;

                case 9777: //Retro Wings Box Kentao
                    {
                        short[] vnums =
                        vnums = new short[] { 9445, 9446, 9447, 9448, 9449, 9450, 9451 };
                        byte[] counts = { 1, 1, 1, 1, 1, 1, 1 };
                        int item = ServerManager.RandomNumber(0, 7);
                        session.Character.GiftAdd(vnums[item], counts[item]);
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    }
                    break;

                //Wing Change
                case 30034:
                    {
                        ItemInstance specialistInstance = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
                        if (session.Character.UseSp && specialistInstance != null && !session.Character.IsSeal && session.Character.LastPotion < DateTime.Now)
                        {
                            short rnd = (short)ServerManager.RandomNumber(1, 20);
                            specialistInstance.VisualEffect = rnd;
                            session.Character.MorphUpgrade2 = rnd;

                            session.Character.LastPotion = DateTime.Now.AddSeconds(2);
                            session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPackets(session.Character.GenerateStatChar());
                            session.SendPacket(session.Character.GenerateLev());

                        }
                    }
                    break;

                case 9781: //kentao retro wing changer
                    {
                        ItemInstance specialistInstance = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
                        if (session.Character.UseSp && specialistInstance != null && !session.Character.IsSeal && session.Character.LastPotion < DateTime.Now)
                        {
                            short rnd = (short)ServerManager.RandomNumber(21, 27);
                            specialistInstance.VisualEffect = rnd;
                            session.Character.MorphUpgrade2 = rnd;

                            session.Character.LastPotion = DateTime.Now.AddSeconds(1);
                            session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPackets(session.Character.GenerateStatChar());
                            session.SendPacket(session.Character.GenerateLev());

                        }
                    }
                    break;

                //Namechange
                case 767:
                    if (session.Character.Group != null && session.Character.Inventory.Any(x => x.Item.VNum == 1638)) // Enter VNum
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg("Leave your group to change your name", 0));
                    }
                    else
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateInbox($"#glmk^ 14 1 Charactername Charactername"));
                    }
                    break;

                case 930:
                    switch (EffectValue)
                    {
                        case 505:
                            //add delay
                            if (session.Character.MapId > 0)
                            {
                                int dist = Map.GetDistance(
                                    new MapCell { X = session.Character.PositionX, Y = session.Character.PositionY },
                                    new MapCell { X = 120, Y = 56 });

                                int dist1 = Map.GetDistance(
                                    new MapCell { X = session.Character.PositionX, Y = session.Character.PositionY },
                                    new MapCell { X = 120, Y = 56 });

                                if (dist < 6)
                                {
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    session.SendPacket(session.Character.GenerateEff(820));
                                    session.SendPacket(session.Character.GenerateEff(821));
                                    session.SendPacket(session.Character.GenerateEff(6008));
                                }
                                if (dist < 3 && session.Character.MapId == 1)
                                {
                                    session.SendPacket(session.Character.GenerateEff(822));
                                    Event.PTS.GeneratePTS(1805, session);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                                if (dist < 1 && session.Character.MapId == 5)
                                {
                                    Event.PTS.GeneratePTS(1824, session);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                                if (dist < 15)
                                {
                                    session.SendPacket(session.Character.GenerateEff(820));
                                    session.SendPacket(session.Character.GenerateEff(6008));
                                }
                                else
                                {
                                    //say 1 521919 10 Aucun signal ne peut être reçu, car la distance est trop élevée.
                                    session.SendPacket(session.Character.GenerateEff(820));
                                    session.SendPacket(session.Character.GenerateEff(6009));
                                }
                            }
                            break;
                    }
                    break;
                

                

                case 5836://Libreta del banco
                    session.SendPacket($"gb 0 {session.Character.GoldBank / 1000} {session.Character.Gold} 0 0");
                    session.SendPacket($"s_memo 6 [Account balance]: {session.Character.GoldBank} gold; [Owned]: {session.Character.Gold} gold\nWe will do our best. Thank you for using the services of Cuarry Bank.");
                    break;

                case 1400://No se que bob
                    Mate equipedMate = session.Character.Mates?.SingleOrDefault(s => s.IsTeamMember && s.MateType == MateType.Partner);

                    if (equipedMate != null)
                    {
                        equipedMate.RemoveTeamMember();
                        session.Character.MapInstance.Broadcast(equipedMate.GenerateOut());
                    }

                    Mate mate = new Mate(session.Character, ServerManager.GetNpcMonster(317), 24, MateType.Partner);
                    session.Character.Mates?.Add(mate);
                    mate.RefreshStats();
                    session.SendPacket($"ctl 2 {mate.PetId} 3");
                    session.Character.MapInstance.Broadcast(mate.GenerateIn());
                    session.SendPacket(UserInterfaceHelper.GeneratePClear());
                    session.SendPackets(session.Character.GenerateScP());
                    session.SendPackets(session.Character.GenerateScN());
                    session.SendPacket(session.Character.GeneratePinit());
                    session.SendPackets(session.Character.Mates.Where(s => s.IsTeamMember)
                        .OrderBy(s => s.MateType)
                        .Select(s => s.GeneratePst()));
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    break;
                case 1419:
                    Mate equipedMates = session.Character.Mates?.SingleOrDefault(s => s.IsTeamMember && s.MateType == MateType.Partner);

                    if (equipedMates != null)
                    {
                        equipedMates.RemoveTeamMember();
                        session.Character.MapInstance.Broadcast(equipedMates.GenerateOut());
                    }

                    Mate mates = new Mate(session.Character, ServerManager.GetNpcMonster(318), 31, MateType.Partner);
                    session.Character.Mates?.Add(mates);
                    mates.RefreshStats();
                    session.SendPacket($"ctl 2 {mates.PetId} 3");
                    session.Character.MapInstance.Broadcast(mates.GenerateIn());
                    session.SendPacket(UserInterfaceHelper.GeneratePClear());
                    session.SendPackets(session.Character.GenerateScP());
                    session.SendPackets(session.Character.GenerateScN());
                    session.SendPacket(session.Character.GeneratePinit());
                    session.SendPackets(session.Character.Mates.Where(s => s.IsTeamMember)
                        .OrderBy(s => s.MateType)
                        .Select(s => s.GeneratePst()));
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    break;
                case 1431:
                    Mate equipedMat = session.Character.Mates?.SingleOrDefault(s => s.IsTeamMember && s.MateType == MateType.Partner);

                    if (equipedMat != null)
                    {
                        equipedMat.RemoveTeamMember();
                        session.Character.MapInstance.Broadcast(equipedMat.GenerateOut());
                    }

                    Mate mat = new Mate(session.Character, ServerManager.GetNpcMonster(319), 48, MateType.Partner);
                    session.Character.Mates?.Add(mat);
                    mat.RefreshStats();
                    session.SendPacket($"ctl 2 {mat.PetId} 3");
                    session.Character.MapInstance.Broadcast(mat.GenerateIn());
                    session.SendPacket(UserInterfaceHelper.GeneratePClear());
                    session.SendPackets(session.Character.GenerateScP());
                    session.SendPackets(session.Character.GenerateScN());
                    session.SendPacket(session.Character.GeneratePinit());
                    session.SendPackets(session.Character.Mates.Where(s => s.IsTeamMember)
                        .OrderBy(s => s.MateType)
                        .Select(s => s.GeneratePst()));
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    break;
                case 5511:
                    session.Character.GeneralLogs.Where(s => s.LogType == "InstanceEntry" && (short.Parse(s.LogData) == 16 || short.Parse(s.LogData) == 17) && s.Timestamp.Date == DateTime.Today).ToList().ForEach(s =>
                    {
                        s.LogType = "NulledInstanceEntry";
                        DAOFactory.GeneralLogDAO.InsertOrUpdate(ref s);
                    });
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    break;
                case 5936://Si estas en arena no puedes usar este item
                case 5937://Si estas en arena no puedes usar este item
                case 5938://Si estas en arena no puedes usar este item
                case 5939://Si estas en arena no puedes usar este item
                case 5940://Si estas en arena no puedes usar este item
                case 5942://Si estas en arena no puedes usar este item
                case 5943://Si estas en arena no puedes usar este item
                case 5944://Si estas en arena no puedes usar este item
                case 5945://Si estas en arena no puedes usar este item
                case 5946://Si estas en arena no puedes usar este item
                    if (session.CurrentMapInstance?.MapInstanceType != MapInstanceType.TalentArenaMapInstance)
                    {
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("CANT_DO_NOT_IN_ARENA"), 10));
                        return;
                    }
                    break;
                default:
                    if (session.CurrentMapInstance?.MapInstanceType == MapInstanceType.TalentArenaMapInstance
                    && VNum != 5936 && VNum != 5937 && VNum != 5938 && VNum != 5939 && VNum != 5940 && VNum != 5942 && VNum != 5943 && VNum != 5944 && VNum != 5945 && VNum != 5946)
                    {
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("CANT_DO_NOT_OUT_ARENA"), 10));
                        return;
                    }
                    break;
            }

            switch (Effect)
            {
                // Honour Medals
                case 69:
                    session.Character.Reputation += ReputPrice;
                    session.SendPacket(session.Character.GenerateFd());
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("REPUT_INCREASE"), ReputPrice), 11));
                    session.CurrentMapInstance?.Broadcast(session, session.Character.GenerateIn(InEffect: 1), ReceiverType.AllExceptMe);
                    session.CurrentMapInstance?.Broadcast(session, session.Character.GenerateGidx(), ReceiverType.AllExceptMe);
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    break;

                // TimeSpace Stones
                case 140:
                    if (session.CurrentMapInstance.MapInstanceType == MapInstanceType.BaseMapInstance)
                    {
                        if (ServerManager.Instance.TimeSpaces.FirstOrDefault(s => s.Id == EffectValue) is ScriptedInstance timeSpace)
                        {
                            session.Character.EnterInstance(timeSpace);
                        }
                    }
                    break;

                // SP Potions
                case 150:
                case 151:
                    {
                        if (session.Character.SpAdditionPoint >= 1000000)
                        {
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("SP_POINTS_FULL"), 0));
                            break;
                        }

                        session.Character.SpAdditionPoint += EffectValue;

                        if (session.Character.SpAdditionPoint > 1000000)
                        {
                            session.Character.SpAdditionPoint = 1000000;
                        }

                        session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SP_POINTSADDED"), EffectValue), 0));
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateSpPoint());
                    }
                    break;

                // ArenaWinner Key   
                case 30000:
                    {
                        int arena = 0;
                        if (session.Character.ArenaWinner == 0)
                            arena = 1;
                        session.Character.ArenaWinner = arena;
                        session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                        
                    }
                    break;

                // Specialist Medal
                case 204:
                    {
                        if (session.Character.SpPoint >= 10000
                            && session.Character.SpAdditionPoint >= 1000000)
                        {
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("SP_POINTS_FULL"), 0));
                            break;
                        }

                        session.Character.SpPoint += EffectValue;

                        if (session.Character.SpPoint > 10000)
                        {
                            session.Character.SpPoint = 10000;
                        }

                        session.Character.SpAdditionPoint += EffectValue * 3;

                        if (session.Character.SpAdditionPoint > 1000000)
                        {
                            session.Character.SpAdditionPoint = 1000000;
                        }

                        session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SP_POINTSADDEDBOTH"), EffectValue, EffectValue * 3), 0));
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateSpPoint());
                    }
                    break;

                // Raid Seals
                case 301:
                    ItemInstance raidSeal = session.Character.Inventory.LoadBySlotAndType(inv.Slot, InventoryType.Main);

                    if (raidSeal != null)
                    {
                        ScriptedInstance raid = ServerManager.Instance.Raids.FirstOrDefault(s => s.Id == raidSeal.Item.EffectValue)?.Copy();

                        if (raid != null)
                        {
                            if (ServerManager.Instance.ChannelId == 51 || session.CurrentMapInstance.MapInstanceType != MapInstanceType.BaseMapInstance)
                            {
                                return;
                            }

                            if (ServerManager.Instance.IsCharacterMemberOfGroup(session.Character.CharacterId))
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RAID_OPEN_GROUP"), 12));
                                return;
                            }

                            var entries = raid.DailyEntries - session.Character.GeneralLogs.CountLinq(s => s.LogType == "InstanceEntry" && short.Parse(s.LogData) == raid.Id && s.Timestamp.Date == DateTime.Today);

                            if (raid.DailyEntries > 0 && entries <= 0)
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("INSTANCE_NO_MORE_ENTRIES"), 0));
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("INSTANCE_NO_MORE_ENTRIES"), 10));
                                return;
                            }

                            if (raid.Label == "Lord Draco")
                            {
                                ItemInstance amulet = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Amulet, InventoryType.Wear);
                                if (amulet != null)
                                {
                                    if (amulet.ItemVNum != 4503)
                                    {
                                        session.SendPacket($"info {Language.Instance.GetMessageFromKey("NO_AMULET_DRACO")}");
                                        return;
                                    }
                                }
                                else
                                {
                                    session.SendPacket($"info {Language.Instance.GetMessageFromKey("NO_AMULET_DRACO")}");
                                    return;
                                }
                            }
                            if (raid.Label == "Glacerus' the Ice Cold")
                            {
                                ItemInstance amulet = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Amulet, InventoryType.Wear);
                                if (amulet != null)
                                {
                                    if (amulet.ItemVNum != 4504)
                                    {
                                        session.SendPacket($"info {Language.Instance.GetMessageFromKey("NO_AMULET_GLACE")}");
                                        return;
                                    }
                                }
                                else
                                {
                                    session.SendPacket($"info {Language.Instance.GetMessageFromKey("NO_AMULET_GLACE")}");
                                    return;
                                }
                            }

                            if (session.Character.Level > raid.LevelMaximum || session.Character.Level < raid.LevelMinimum || session.Character.HeroLevel > raid.HeroMaximumLevel || session.Character.HeroLevel < raid.HeroMinimumLevel)
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RAID_LEVEL_INCORRECT"), 10));
                                return;
                            }

                            Group group = new Group
                            {
                                GroupType = raid.IsGiantTeam ? GroupType.GiantTeam : GroupType.BigTeam,
                                Raid = raid
                            };

                            if (group.JoinGroup(session))
                            {
                                ServerManager.Instance.AddGroup(group);
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("RAID_LEADER"), session.Character.Name), 0));
                                session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("RAID_LEADER"), session.Character.Name), 10));
                                session.SendPacket(session.Character.GenerateRaid(2));
                                session.SendPacket(session.Character.GenerateRaid(0));
                                session.SendPacket(session.Character.GenerateRaid(1));
                                session.SendPacket(group.GenerateRdlst());
                                session.Character.Inventory.RemoveItemFromInventory(raidSeal.Id);
                            }
                        }
#pragma warning disable 4014
                        DiscordWebhookHelper.DiscordEventRaid($"{session.Character.Name} to open the raid: {raid.Label}");
                    }
                    break;

                // Partner Suits/Skins
                case 305:
                    Mate mate = session.Character.Mates.Find(s => s.MateTransportId == int.Parse(packetsplit[3]));
                    if (mate != null && EffectValue == mate.NpcMonsterVNum && mate.Skin == 0)
                    {
                        mate.Skin = Morph;
                        session.SendPacket(mate.GenerateCMode(mate.Skin));
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    }
                    break;

                //suction Funnel (Quest Item / QuestId = 1724)
                case 400:
                    if (session.Character == null || session.Character.Quests.All(q => q.QuestId != 1724))
                    {
                        break;
                    }
                    if (session.Character.Quests.FirstOrDefault(q => q.QuestId == 1724) is CharacterQuest kenkoQuest)
                    {
                        MapMonster kenko = session.CurrentMapInstance?.Monsters.FirstOrDefault(m => m.MapMonsterId == session.Character.LastNpcMonsterId && m.MonsterVNum > 144 && m.MonsterVNum < 154);
                        if (kenko == null || session.Character.Inventory.CountItem(1174) > 0)
                        {
                            break;
                        }
                        if (session.Character.LastFunnelUse.AddSeconds(30) <= DateTime.Now)
                        {
                            if (kenko.CurrentHp / kenko.MaxHp * 100 < 30)
                            {
                                if (ServerManager.RandomNumber() < 30)
                                {
                                    kenko.SetDeathStatement();
                                    session.Character.MapInstance.Broadcast(StaticPacketHelper.Out(UserType.Monster, kenko.MapMonsterId));
                                    session.Character.Inventory.AddNewToInventory(1174); // Kenko Bead
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("KENKO_CATCHED"), 0));
                                }
                                else { session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("QUEST_CATCH_FAIL"), 0)); }
                                session.Character.LastFunnelUse = DateTime.Now;
                            }
                            else { session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("HP_TOO_HIGH"), 0)); }
                        }
                    }
                    break;

                // Fairy Booster
                case 250:
                    if (!session.Character.Buff.ContainsKey(131))
                    {
                        session.Character.AddStaticBuff(new StaticBuffDTO { CardId = 131 });
                        session.CurrentMapInstance?.Broadcast(session.Character.GeneratePairy());
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), inv.Item.Name), 0));
                        session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3014), session.Character.PositionX, session.Character.PositionY);
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    }
                    else
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_IN_USE"), 0));
                    }
                    break;

                // Rainbow Pearl/Magic Eraser
                case 666:
                    if (EffectValue == 1 && byte.TryParse(packetsplit[9], out byte islot))
                    {
                        ItemInstance wearInstance = session.Character.Inventory.LoadBySlotAndType(islot, InventoryType.Equipment);

                        if (wearInstance != null && (wearInstance.Item.ItemType == ItemType.Weapon || wearInstance.Item.ItemType == ItemType.Armor) && wearInstance.ShellEffects.Count != 0 && !wearInstance.Item.IsHeroic)
                        {
                            wearInstance.ShellEffects.Clear();
                            wearInstance.ShellRarity = null;
                            DAOFactory.ShellEffectDAO.DeleteByEquipmentSerialId(wearInstance.EquipmentSerialId);
                            if (wearInstance.EquipmentSerialId == Guid.Empty)
                            {
                                wearInstance.EquipmentSerialId = Guid.NewGuid();
                            }
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.SendPacket(UserInterfaceHelper.GenerateGuri(17, 1, session.Character.CharacterId));
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("OPTION_DELETE"), 0));
                        }
                    }
                    else
                    {
                        session.SendPacket("guri 18 0");
                    }
                    break;

                case 9581: //Kentao. Remove option cellon 
                    if (EffectValue == 1 && byte.TryParse(packetsplit[9], out byte islotcellon))
                    {
                        ItemInstance wearInstance = session.Character.Inventory.LoadBySlotAndType(islotcellon, InventoryType.Equipment);

                        if (wearInstance != null && (wearInstance.Item.ItemType == ItemType.Jewelery) && wearInstance.CellonOptions.Count != 0)
                        {
                            wearInstance.CellonOptions.Clear();
                            DAOFactory.CellonOptionDAO.DeleteByEquipmentSerialId(wearInstance.EquipmentSerialId);
                            if (wearInstance.EquipmentSerialId == Guid.Empty)
                            {
                                wearInstance.EquipmentSerialId = Guid.NewGuid();
                            }
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("OPTION_CELLON_DELETE"), 0));
                        }
                    }
                    else
                    {
                        session.SendPacket("guri 18 0");
                    } 
                    break;

                case 30049: //Kentao. Remove option cellon Permanent
                    if (EffectValue == 1 && byte.TryParse(packetsplit[9], out byte islotcellonP))
                    {
                        ItemInstance wearInstance = session.Character.Inventory.LoadBySlotAndType(islotcellonP, InventoryType.Equipment);

                        if (wearInstance != null && (wearInstance.Item.ItemType == ItemType.Jewelery) && wearInstance.CellonOptions.Count != 0)
                        {
                            wearInstance.CellonOptions.Clear();
                            DAOFactory.CellonOptionDAO.DeleteByEquipmentSerialId(wearInstance.EquipmentSerialId);
                            if (wearInstance.EquipmentSerialId == Guid.Empty)
                            {
                                wearInstance.EquipmentSerialId = Guid.NewGuid();
                            }
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("OPTION_CELLON_DELETE"), 0));
                        }
                    }
                    else
                    {
                        session.SendPacket("guri 18 0");
                    }
                    break;



                //Basic SP Potion
                case 1245:
                    session.Character.SpAdditionPoint = 10000;
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    session.SendPacket("msg 4 You received 10.000 Points");
                    break;

                //Stat Potions
                case 6600:
                    switch (EffectValue)
                    {
                        //Attack Potion
                        case 1:
                            session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3022), session.Character.PositionX, session.Character.PositionY);
                            session.Character.AddBuff(new Buff(116, session.Character.Level), session.Character.BattleEntity);
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            break;

                        //Defence Potion
                        case 2:
                            session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3022), session.Character.PositionX, session.Character.PositionY);
                            session.Character.AddBuff(new Buff(117, session.Character.Level), session.Character.BattleEntity);
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            break;

                        //Energy Potion
                        case 3:
                            session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3022), session.Character.PositionX, session.Character.PositionY);
                            session.Character.AddBuff(new Buff(118, session.Character.Level), session.Character.BattleEntity);
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            break;

                        //Experience Potion
                        case 4:
                            session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3022), session.Character.PositionX, session.Character.PositionY);
                            session.Character.AddBuff(new Buff(119, session.Character.Level), session.Character.BattleEntity);
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            break;
                    }
                    break;

                // Ancelloan's Blessing
                case 208:
                    if (!session.Character.Buff.ContainsKey(121))
                    {
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.Character.AddStaticBuff(new StaticBuffDTO { CardId = 121 });
                    }
                    else
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_IN_USE"), 0));
                    }
                    break;

                case 1116:
                    if (session.Character.StaticBonusList.Any(s => s.StaticBonusType == StaticBonusType.AutoLoot))
                    {
                        return;
                    }
                    session.Character.StaticBonusList.Remove(new StaticBonusDTO
                    {
                        CharacterId = session.Character.CharacterId,
                        StaticBonusType = StaticBonusType.AutoLoot
                    });
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    session.SendPacket(session.Character.GenerateExts());
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("AUTOLOOT_REMOVE"), Name), 12));
                    break;


                // AutoLoot Medal
                case 9284:
                    if (session.Character.StaticBonusList.Any(s => s.StaticBonusType == StaticBonusType.AutoLoot))
                    {
                        return;
                    }

                    session.Character.StaticBonusList.Add(new StaticBonusDTO
                    {
                        CharacterId = session.Character.CharacterId,
                        DateEnd = DateTime.Now.AddDays(30),
                        StaticBonusType = StaticBonusType.AutoLoot
                    });
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    session.SendPacket(session.Character.GenerateExts());
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("AUTOLOOT_ACTIVATED"), Name), 12));
                    break;



                // BUFF
                case 211:
                    {
                        session.Character.AddBuff(new Buff(151, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(152, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(153, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(155, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(157, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(72, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(91, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(67, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(134, session.Character.Level), session.Character.BattleEntity);
                        session.Character.AddBuff(new Buff(89, session.Character.Level), session.Character.BattleEntity);
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("BUFF_ACTIVATED"), Name), 12));
                    }
                    break;


                // Guardian Angel's Blessing
                case 210:
                    if (!session.Character.Buff.ContainsKey(122))
                    {
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.Character.AddStaticBuff(new StaticBuffDTO { CardId = 122 });
                    }
                    else
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_IN_USE"), 0));
                    }
                    break;

                case 2081:
                    if (!session.Character.Buff.ContainsKey(146))
                    {
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.Character.AddStaticBuff(new StaticBuffDTO { CardId = 146 });
                    }
                    else
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_IN_USE"), 0));
                    }
                    break;

                // Divorce letter
                case 6969:
                    if (session.Character.Group != null)
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ALLOWED_IN_GROUP"), 0));
                        return;
                    }
                    CharacterRelationDTO rel = session.Character.CharacterRelations.FirstOrDefault(s => s.RelationType == CharacterRelationType.Spouse);
                    if (rel != null)
                    {
                        session.Character.DeleteRelation(rel.CharacterId == session.Character.CharacterId ? rel.RelatedCharacterId : rel.CharacterId, CharacterRelationType.Spouse);
                        session.SendPacket(UserInterfaceHelper.GenerateInfo(Language.Instance.GetMessageFromKey("DIVORCED")));
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    }
                    break;

                // Cupid's arrow
                case 34:
                    if (packetsplit != null && packetsplit.Length > 3)
                    {
                        if (long.TryParse(packetsplit[3], out long characterId))
                        {
                            if (session.Character.CharacterId == characterId)
                            {
                                return;
                            }
                            if (session.Character.CharacterRelations.Any(s => s.RelationType == CharacterRelationType.Spouse))
                            {
                                session.SendPacket($"info {Language.Instance.GetMessageFromKey("ALREADY_MARRIED")}");
                                return;
                            }
                            if (session.Character.Group != null)
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ALLOWED_IN_GROUP"), 0));
                                return;
                            }
                            if (!session.Character.IsFriendOfCharacter(characterId))
                            {
                                session.SendPacket($"info {Language.Instance.GetMessageFromKey("MUST_BE_FRIENDS")}");
                                return;
                            }
                            ClientSession otherSession = ServerManager.Instance.GetSessionByCharacterId(characterId);
                            if (otherSession != null)
                            {
                                if (otherSession.Character.Group != null)
                                {
                                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("OTHER_PLAYER_IN_GROUP"), 0));
                                    return;
                                }
                                otherSession.SendPacket(UserInterfaceHelper.GenerateDialog(
                                    $"#fins^34^{session.Character.CharacterId} #fins^69^{session.Character.CharacterId} {string.Format(Language.Instance.GetMessageFromKey("MARRY_REQUEST"), session.Character.Name)}"));
                                session.Character.MarryRequestCharacters.Add(characterId);
                                session.Character.Inventory.RemoveItemFromInventory(inv.Id);

                            }
                        }
                    }
                    break;

                case 100: // Miniland Signpost
                    {
                        if (session.Character.BattleEntity.GetOwnedNpcs().Any(s => session.Character.BattleEntity.IsSignpost(s.NpcVNum)))
                        {
                            return;
                        }
                        if (session.CurrentMapInstance.MapInstanceType == MapInstanceType.BaseMapInstance && new short[] { 1, 145 }.Contains(session.CurrentMapInstance.Map.MapId))
                        {
                            MapNpc signPost = new MapNpc
                            {
                                NpcVNum = (short)EffectValue,
                                MapX = session.Character.PositionX,
                                MapY = session.Character.PositionY,
                                MapId = session.CurrentMapInstance.Map.MapId,
                                ShouldRespawn = false,
                                IsMoving = false,
                                MapNpcId = session.CurrentMapInstance.GetNextNpcId(),
                                Owner = session.Character.BattleEntity,
                                Dialog = 10000,
                                Position = 2,
                                Name = $"{session.Character.Name}'s^[Miniland]"
                            };
                            switch (EffectValue)
                            {
                                case 1428:
                                case 1499:
                                case 1519:
                                    signPost.AliveTime = 3600;
                                    break;
                                default:
                                    signPost.AliveTime = 1800;
                                    break;
                            }
                            signPost.Initialize(session.CurrentMapInstance);
                            session.CurrentMapInstance.AddNPC(signPost);
                            session.CurrentMapInstance.Broadcast(signPost.GenerateIn());
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        }
                    }
                    break;


                case 550: // Campfire and other craft npcs
                    {
                        if (session.CurrentMapInstance.MapInstanceType == MapInstanceType.BaseMapInstance)
                        {
                            short dialog = 10023;
                            switch (EffectValue)
                            {
                                case 956:
                                    dialog = 10023;
                                    break;
                                case 957:
                                    dialog = 10024;
                                    break;
                                case 959:
                                    dialog = 10026;
                                    break;
                            }
                            MapNpc campfire = new MapNpc
                            {
                                NpcVNum = (short)EffectValue,
                                MapX = session.Character.PositionX,
                                MapY = session.Character.PositionY,
                                MapId = session.CurrentMapInstance.Map.MapId,
                                ShouldRespawn = false,
                                IsMoving = false,
                                MapNpcId = session.CurrentMapInstance.GetNextNpcId(),
                                Owner = session.Character.BattleEntity,
                                Dialog = dialog,
                                Position = 2,
                            };
                            campfire.AliveTime = 180;
                            campfire.Initialize(session.CurrentMapInstance);
                            session.CurrentMapInstance.AddNPC(campfire);
                            session.CurrentMapInstance.Broadcast(campfire.GenerateIn());
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        }
                    }
                    break;

                // Faction Egg
                case 570:
                    if (session.Character.Faction == (FactionType)EffectValue)
                    {
                        return;
                    }
                    if (EffectValue < 3)
                    {
                        session.SendPacket(session.Character.Family == null
                            ? $"qna #guri^750^{EffectValue} {Language.Instance.GetMessageFromKey($"ASK_CHANGE_FACTION{EffectValue}")}"
                            : UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("IN_FAMILY"),
                            0));
                    }
                    else
                    {
                        session.SendPacket(session.Character.Family != null
                            ? $"qna #guri^750^{EffectValue} {Language.Instance.GetMessageFromKey($"ASK_CHANGE_FACTION{EffectValue}")}"
                            : UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NO_FAMILY"),
                            0));
                    }

                    break;

                // SP Wings
                case 650:
                    ItemInstance specialistInstance = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
                    if (session.Character.UseSp && specialistInstance != null && !session.Character.IsSeal)
                    {
                        if (Option == 0)
                        {
                            session.SendPacket($"qna #u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^3 {Language.Instance.GetMessageFromKey("ASK_WINGS_CHANGE")}");
                        }
                        else
                        {
                            void disposeBuff(short vNum)
                            {
                                if (session.Character.BuffObservables.ContainsKey(vNum))
                                {
                                    session.Character.BuffObservables[vNum].Dispose();
                                    session.Character.BuffObservables.Remove(vNum);
                                }
                                session.Character.RemoveBuff(vNum);
                            }

                            disposeBuff(387);
                            disposeBuff(395);
                            disposeBuff(396);
                            disposeBuff(397);
                            disposeBuff(398);
                            disposeBuff(410);
                            disposeBuff(411);
                            disposeBuff(444);
                            disposeBuff(663);
                            disposeBuff(686);

                            specialistInstance.Design = (byte)EffectValue;

                            session.Character.MorphUpgrade2 = EffectValue;
                            session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPackets(session.Character.GenerateStatChar());
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        }
                    }
                    else
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NO_SP"), 0));
                    }
                    break;

                // Self-Introduction
                case 203:
                    if (!session.Character.IsVehicled && Option == 0)
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateGuri(10, 2, session.Character.CharacterId, 1));
                    }
                    break;

                // Magic Lamp
                case 651:
                    if (session.Character.Inventory.All(i => i.Type != InventoryType.Wear))
                    {
                        if (Option == 0)
                        {
                            session.SendPacket($"qna #u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^3 {Language.Instance.GetMessageFromKey("ASK_USE")}");
                        }
                        else
                        {
                            session.Character.ChangeSex();
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        }
                    }
                    else
                    {
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("EQ_NOT_EMPTY"), 0));
                    }
                    break;

                // Vehicles
                case 1000:
                    if (EffectValue != 0
                     || session.CurrentMapInstance?.MapInstanceType == MapInstanceType.EventGameInstance
                     || session.CurrentMapInstance?.MapInstanceType == (MapInstanceType.TalentArenaMapInstance)
                     || session.CurrentMapInstance?.MapInstanceType == (MapInstanceType.IceBreakerInstance)
                     || session.Character.IsSeal || session.Character.IsMorphed || session.Character.HasShopOpened)
                    {
                        return;
                    }
                    short morph = Morph;
                    byte speed = Speed;
                    if (Morph < 0)
                    {
                        switch (VNum)
                        {
                            case 5923:
                                morph = 2513;
                                speed = 14;
                                break;
                        }
                    }
                    if (morph > 0)
                    {
                        if (Option == 0 && !session.Character.IsVehicled)
                        {
                            if (session.Character.Buff.Any(s => s.Card.BuffType == BuffType.Bad))
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("CANT_TRASFORM_WITH_DEBUFFS"),
                                    0));
                                return;
                            }
                            if (session.Character.IsSitting)
                            {
                                session.Character.IsSitting = false;
                                session.CurrentMapInstance?.Broadcast(session.Character.GenerateRest());
                            }
                            session.Character.LastDelay = DateTime.Now;
                            session.SendPacket(UserInterfaceHelper.GenerateDelay(3000, 3, $"#u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^2"));
                        }
                        else
                        {
                            if (!session.Character.IsVehicled && Option != 0)
                            {
                                DateTime delay = DateTime.Now.AddSeconds(-4);
                                if (session.Character.LastDelay > delay && session.Character.LastDelay < delay.AddSeconds(2))
                                {
                                    session.Character.IsVehicled = true;
                                    session.Character.VehicleSpeed = speed;
                                    session.Character.VehicleItem = this;
                                    session.Character.LoadSpeed();
                                    session.Character.MorphUpgrade = 0;
                                    session.Character.MorphUpgrade2 = 0;
                                    session.Character.Morph = morph + (byte)session.Character.Gender;
                                    if (session.Character.Morph == 3360) //basto morph sonju rabbit 
                                    {
                                        session.Character.Morph = 3359;
                                    }
                                    session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 196), session.Character.PositionX, session.Character.PositionY);
                                    session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                                    session.SendPacket(session.Character.GenerateCond());
                                    session.Character.LastSpeedChange = DateTime.Now;
                                    session.Character.Mates.Where(s => s.IsTeamMember).ToList()
                                        .ForEach(s => session.CurrentMapInstance?.Broadcast(s.GenerateOut()));
                                    if (Morph < 0)
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                }
                            }
                            else if (session.Character.IsVehicled)
                            {
                                session.Character.RemoveVehicle();
                                foreach (Mate teamMate in session.Character.Mates.Where(m => m.IsTeamMember))
                                {
                                    teamMate.PositionX =
                                        (short)(session.Character.PositionX + (teamMate.MateType == MateType.Partner ? -1 : 1));
                                    teamMate.PositionY = (short)(session.Character.PositionY + 1);
                                    if (session.Character.MapInstance.Map.IsBlockedZone(teamMate.PositionX, teamMate.PositionY))
                                    {
                                        teamMate.PositionX = session.Character.PositionX;
                                        teamMate.PositionY = session.Character.PositionY;
                                    }
                                    teamMate.UpdateBushFire();
                                    Parallel.ForEach(session.CurrentMapInstance.Sessions.Where(s => s.Character != null), s =>
                                    {
                                        if (ServerManager.Instance.ChannelId != 51 || session.Character.Faction == s.Character.Faction)
                                        {
                                            s.SendPacket(teamMate.GenerateIn(false, ServerManager.Instance.ChannelId == 51));
                                        }
                                        else
                                        {
                                            s.SendPacket(teamMate.GenerateIn(true, ServerManager.Instance.ChannelId == 51, s.Account.Authority));
                                        }
                                    });
                                }
                                session.SendPacket(session.Character.GeneratePinit());
                                session.Character.Mates.ForEach(s => session.SendPacket(s.GenerateScPacket()));
                                session.SendPackets(session.Character.GeneratePst());
                            }
                        }
                    }
                    break;

                // Sealed Vessel
                case 1002:
                    int type, secondaryType, inventoryType, slot;
                    if (packetsplit != null && int.TryParse(packetsplit[2], out type) && int.TryParse(packetsplit[3], out secondaryType) && int.TryParse(packetsplit[4], out inventoryType) && int.TryParse(packetsplit[5], out slot))
                    {
                        int packetType;
                        switch (EffectValue)
                        {
                            case 69:
                                if (int.TryParse(packetsplit[6], out packetType))
                                {
                                    switch (packetType)
                                    {
                                        case 0:
                                            session.SendPacket(UserInterfaceHelper.GenerateDelay(500, 7, $"#u_i^{type}^{secondaryType}^{inventoryType}^{slot}^1"));
                                            break;

                                        case 1:
                                            int rnd = ServerManager.RandomNumber(0, 1000);
                                            if (rnd < 5)
                                            {
                                                short[] vnums =
                                                {
                                                        1428, 5369, 1218, 1363, 1364
                                                    };
                                                byte[] counts = { 50, 5, 5, 5, 5 };
                                                int item = ServerManager.RandomNumber(0, 6);
                                                session.Character.GiftAdd(vnums[item], counts[item]);
                                            }
                                            else if (rnd < 30)
                                            {
                                                short[] vnums = { 361, 362, 363, 366, 367, 368, 371, 372, 373 };
                                                session.Character.GiftAdd(vnums[ServerManager.RandomNumber(0, 9)], 1);
                                            }
                                            else
                                            {
                                                short[] vnums =
                                                {
                                                       1030, 2282, 1013
                                                    };
                                                byte[] counts =
                                                {
                                                       15, 30, 50
                                                    };
                                                int item = ServerManager.RandomNumber(0, 3);
                                                session.Character.GiftAdd(vnums[item], counts[item]);
                                            }
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            break;
                                    }
                                }
                                break;
                            default:
                                if (int.TryParse(packetsplit[6], out packetType))
                                {
                                    if (session.Character.MapInstance.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) || session.Character.MapId == 147 || ServerManager.Instance.ChannelId == 51 || session.Character.MapId == 1)
                                    {
                                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("CANNOT_USE_HERE"), 0));
                                        return;
                                    }

                                    if (!session.Character.VerifiedLock)
                                    {
                                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ACTION_NOT_POSSIBLE_USE_UNLOCK"), 0));
                                        return;
                                    }


                                    switch (packetType)
                                    {
                                        case 0:
                                            session.SendPacket(UserInterfaceHelper.GenerateDelay(5000, 7, $"#u_i^{type}^{secondaryType}^{inventoryType}^{slot}^1"));
                                            break;

                                        case 1:
                                            if (session.HasCurrentMapInstance && (session.Character.MapInstance == session.Character.Miniland || session.CurrentMapInstance.MapInstanceType == MapInstanceType.BaseMapInstance) && (session.Character.LastVessel.AddSeconds(1) <= DateTime.Now || session.Character.StaticBonusList.Any(s => s.StaticBonusType == StaticBonusType.FastVessels)))
                                            {
                                                short[] vnums = { 1386, 1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394, 1395, 1396, 1397, 1398, 1399, 1400, 1401, 1402, 1403, 1404, 1405 };
                                                short vnum = vnums[ServerManager.RandomNumber(0, 20)];

                                                NpcMonster npcmonster = ServerManager.GetNpcMonster(vnum);
                                                if (npcmonster == null)
                                                {
                                                    return;
                                                }
                                                MapMonster monster = new MapMonster
                                                {
                                                    MonsterVNum = vnum,
                                                    MapX = session.Character.PositionX,
                                                    MapY = session.Character.PositionY,
                                                    MapId = session.Character.MapInstance.Map.MapId,
                                                    Position = session.Character.Direction,
                                                    IsMoving = true,
                                                    MapMonsterId = session.CurrentMapInstance.GetNextMonsterId(),
                                                    ShouldRespawn = false
                                                };
                                                monster.Initialize(session.CurrentMapInstance);
                                                session.CurrentMapInstance.AddMonster(monster);
                                                session.CurrentMapInstance.Broadcast(monster.GenerateIn());
                                                session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                                session.Character.LastVessel = DateTime.Now;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                    //Raid Seal Shop
                case 1150:
                    DAOFactory.ShopDAO.LoadById(1065);
                    session.Character.HasShopOpened = true;
                    session.Character.IsShopping = true;
                    //Session.CurrentMapInstance.Broadcast($"shop 2 {npc.MapNpcId} {npc.Shop.ShopId} {npc.Shop.MenuType} {npc.Shop.ShopType} {npc.Shop.Name}");
                    session.SendPacket($"shop 2 1065 20006 0 10 Raid Seal Merchant");
                    break;
                
                                //Fix FamilyXP Item for Lvl 20
                                // family xp 5K
                                   case 9722:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(5000);
                                        }

                                    }
                                    break;
                                // family xp 10K
                                case 9723:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(10000);
                                        }

                                    }
                                    break;

                                // family xp 30K
                                case 9725:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(30000);
                                        }

                                    }
                                    break;

                                // family xp 50K
                                case 9726:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(50000);
                                        }

                                    }
                                    break;

                                // family xp 100K
                                case 9727:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(100000);
                                        }

                                    }
                                    break;

                                // family xp 150K
                                case 9728:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(150000);
                                        }

                                    }
                                    break;

                                // family xp 200K
                                case 9729:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(200000);
                                        }

                                    }
                                    break;

                                // family xp 300K
                                case 9730:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("You cant use that Item, because you Family is already Level 20"), 0));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(300000);
                                        }

                                    }
                                    break;

                                // family xp 500K
                                case 9731:
                                    {
                                        if (session.Character.Family.FamilyLevel == 20 || session.Character.Family.FamilyLevel == 21)
                                        {
                                            session.SendPacket(session.Character.GenerateSay("You cant use that Item, because you Family is already Level 20", 10));
                                            return;
                                        }
                                        else
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.GenerateFamilyXp(500000);
                                        }
                                    }
                                    break;
                case 10516: //kentao family up lvl20 with ticket.//bastoi
                    {
                        if (session.Character.Family.FamilyLevel == 20)
                        {
                            session.SendPacket(session.Character.GenerateSay("You cant use that Item, because you Family is already Level 20", 10));
                            return;
                        }
                        else
                        {
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.Character.Family.FamilyLevel = 20;
                        }
                    }
                    break;



                //Medal of Erenia
                case 9999:
                    if (!session.Character.StaticBonusList.Any(s => s.StaticBonusType == StaticBonusType.MedalOfErenia))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddDays(EffectValue),
                            StaticBonusType = StaticBonusType.MedalOfErenia
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay("This Item is already in use!", 11));
                    }
                    break;

                // Golden Bazaar Medal // kentao nosbazar médaille
                case 1003:
                    if (!session.Character.StaticBonusList.Any(s => s.StaticBonusType == StaticBonusType.BazaarMedalGold || s.StaticBonusType == StaticBonusType.BazaarMedalSilver))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddDays(EffectValue),
                            StaticBonusType = StaticBonusType.BazaarMedalGold
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // Silver Bazaar Medal
                case 1004:
                    if (!session.Character.StaticBonusList.Any(s => s.StaticBonusType == StaticBonusType.BazaarMedalGold || s.StaticBonusType == StaticBonusType.BazaarMedalGold))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddDays(EffectValue),
                            StaticBonusType = StaticBonusType.BazaarMedalSilver
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // Pet Slot Expansion
                case 1006:
                    if (Option == 0)
                    {
                        session.SendPacket($"qna #u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^2 {Language.Instance.GetMessageFromKey("ASK_PET_MAX")}");
                    }
                    else if ((inv.Item?.IsSoldable == true && session.Character.MaxMateCount < 90) || session.Character.MaxMateCount < 30)
                    {
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.Character.MaxMateCount += 10;
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("GET_PET_PLACES"), 10));
                        session.SendPacket(session.Character.GenerateScpStc());
                    }
                    break;

                // Permanent Backpack Expansion
                case 601:
                    if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.BackPack))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddYears(15),
                            StaticBonusType = StaticBonusType.BackPack
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateExts());
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // Permanent Partner's Backpack
                case 602:
                    if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.PetBackPack))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddYears(15),
                            StaticBonusType = StaticBonusType.PetBackPack
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateExts());
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // Permanent Pet Basket
                case 603:
                    if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.PetBasket))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddYears(15),
                            StaticBonusType = StaticBonusType.PetBasket
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateExts());
                        session.SendPacket("ib 1278 1");
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // New backpack
                case 604:
                    {
                        if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.NewBackPack))
                        {
                            session.Character.StaticBonusList.Add(new StaticBonusDTO
                            {
                                CharacterId = session.Character.CharacterId,
                                DateEnd = DateTime.Now.AddYears(99),
                                StaticBonusType = StaticBonusType.NewBackPack
                            });
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.SendPacket(session.Character.GenerateExts());
                            session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                        }
                        break;
                    }

                // New backpack (not permanant)
                case 605:
                    if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.NewBackPack))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddDays(EffectValue),
                            StaticBonusType = StaticBonusType.NewBackPack
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateExts());
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                    //Tattoo Patterns
               /* case 606:
                    if (session.Character.TattooCount == 2)
                    {
                        session.SendPacket("msg 4 You already have 2 Tattoos!");
                        return;
                    }
                    switch (EffectValue)
                    {
                        //Bear Loa Tattoo Pattern
                        case 1:
                            #region Add Tattoo
                            int rnd = ServerManager.RandomNumber(0, 1000);
                            if (rnd < 50)
                            {
                                session.Character.AddTattoo(692, 0);
                            }
                            else if (rnd < 100)
                            {
                                session.Character.AddTattoo(691, 0);
                            }
                            else if (rnd < 300)
                            {
                                session.Character.AddTattoo(690, 0);
                            }
                            else if (rnd < 500)
                            {
                                session.Character.AddTattoo(689, 0);
                            }
                            else if (rnd < 700)
                            {
                                session.Character.AddTattoo(688, 0);
                            }
                            else if (rnd < 1000)
                            {
                                session.Character.AddTattoo(687, 0);
                            }
                            else
                            {
                                session.Character.AddTattoo(686, 0);
                            }
                            #endregion
                            session.SendPackets(session.Character.GenerateQuicklist());
                            session.SendPacket(session.Character.GenerateSki());
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.Character.TattooCount += 1;
                            break;

                        //Lion Loa Tattoo Pattern
                        case 2:
                            #region Add Tattoo
                            int rnd2 = ServerManager.RandomNumber(0, 1000);
                            if (rnd2 < 50)
                            {
                                session.Character.AddSkill(700);
                            }
                            else if (rnd2 < 100)
                            {
                                session.Character.AddSkill(699);
                            }
                            else if (rnd2 < 200)
                            {
                                session.Character.AddSkill(698);
                            }
                            else if (rnd2 < 400)
                            {
                                session.Character.AddSkill(697);
                            }
                            else if (rnd2 < 500)
                            {
                                session.Character.AddSkill(696);
                            }
                            else if (rnd2 < 700)
                            {
                                session.Character.AddSkill(695);
                            }
                            else if (rnd2 < 1000)
                            {
                                session.Character.AddSkill(694);
                            }
                            else
                            {
                                session.Character.AddSkill(693);
                            }
                            #endregion
                            session.SendPackets(session.Character.GenerateQuicklist());
                            session.SendPacket(session.Character.GenerateSki());
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.Character.TattooCount += 1;
                            break;

                        //Eagle Loa Tattoo Pattern
                        case 3:
                            #region Add Tattoo
                            int rnd3 = ServerManager.RandomNumber(0, 1000);
                            if (rnd3 < 50)
                            {
                                session.Character.AddSkill(708);
                            }
                            else if (rnd3 < 100)
                            {
                                session.Character.AddSkill(707);
                            }
                            else if (rnd3 < 200)
                            {
                                session.Character.AddSkill(706);
                            }
                            else if (rnd3 < 300)
                            {
                                session.Character.AddSkill(705);
                            }
                            else if (rnd3 < 400)
                            {
                                session.Character.AddSkill(704);
                            }
                            else if (rnd3 < 700)
                            {
                                session.Character.AddSkill(703);
                            }
                            else if (rnd3 < 1000)
                            {
                                session.Character.AddSkill(702);
                            }
                            else
                            {
                                session.Character.AddSkill(701);
                            }
                            #endregion
                            session.SendPackets(session.Character.GenerateQuicklist());
                            session.SendPacket(session.Character.GenerateSki());
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.Character.TattooCount += 1;
                            break;

                        //Snake Loa Tattoo Pattern
                        case 4:
                            #region Add Tattoo
                            int rnd4 = ServerManager.RandomNumber(0, 1000);
                            if (rnd4 < 50)
                            {
                                session.Character.AddSkill(716);
                            }
                            else if (rnd4 < 100)
                            {
                                session.Character.AddSkill(715);
                            }
                            else if (rnd4 < 200)
                            {
                                session.Character.AddSkill(714);
                            }
                            else if (rnd4 < 300)
                            {
                                session.Character.AddSkill(713);
                            }
                            else if (rnd4 < 400)
                            {
                                session.Character.AddSkill(712);
                            }
                            else if (rnd4 < 700)
                            {
                                session.Character.AddSkill(711);
                            }
                            else if (rnd4 < 1000)
                            {
                                session.Character.AddSkill(710);
                            }
                            else
                            {
                                session.Character.AddSkill(709);
                            }
                            #endregion
                            session.SendPackets(session.Character.GenerateQuicklist());
                            session.SendPacket(session.Character.GenerateSki());
                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            session.Character.TattooCount += 1;
                            break;
                    }
                    break;*/  // block parchemin tattoo

                // Pet Basket
                case 1007:
                    if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.PetBasket))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddDays(EffectValue),
                            StaticBonusType = StaticBonusType.PetBasket
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateExts());
                        session.SendPacket("ib 1278 1");
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // Partner's Backpack
                case 1008:
                    if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.PetBackPack))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddDays(EffectValue),
                            StaticBonusType = StaticBonusType.PetBackPack
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateExts());
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // Backpack Expansion
                case 1009:
                    if (session.Character.StaticBonusList.All(s => s.StaticBonusType != StaticBonusType.BackPack))
                    {
                        session.Character.StaticBonusList.Add(new StaticBonusDTO
                        {
                            CharacterId = session.Character.CharacterId,
                            DateEnd = DateTime.Now.AddDays(EffectValue),
                            StaticBonusType = StaticBonusType.BackPack
                        });
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                        session.SendPacket(session.Character.GenerateExts());
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("EFFECT_ACTIVATED"), Name), 12));
                    }
                    break;

                // Sealed Tarot Card
                case 1005:
                    session.Character.GiftAdd((short)(VNum - Effect), 1);
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    break;

                // Tarot Card Game
                case 1894:
                    if (EffectValue == 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            session.Character.GiftAdd((short)(Effect + ServerManager.RandomNumber(0, 10)), 1);
                        }
                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    }
                    break;

                // Sealed Tarot Card
                case 2152:
                    session.Character.GiftAdd((short)(VNum + Effect), 1);
                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                    break;

                // Transformation scrolls
                case 1001:
                    if (session.Character.IsMorphed)
                    {
                        session.Character.IsMorphed = false;
                        session.Character.Morph = 0;
                        session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                    }
                    else if (!session.Character.UseSp && !session.Character.IsVehicled)
                    {
                        if (Option == 0)
                        {
                            session.Character.LastDelay = DateTime.Now;
                            session.SendPacket(UserInterfaceHelper.GenerateDelay(3000, 3, $"#u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^1"));
                        }
                        else
                        {
                            int[] possibleTransforms = null;

                            switch (EffectValue)
                            {
                                case 1: // Halloween
                                    possibleTransforms = new int[]
                                    {
                                    404, //Torturador pellizcador
                                    405, //Torturador enrollador
                                    406, //Torturador de acero
                                    446, //Guerrero yak
                                    447, //Mago yak
                                    441, //Guerrero de la muerte
                                    276, //Rey polvareda
                                    324, //Princesa Catrisha
                                    248, //Bruja oscura
                                    249, //Bruja de sangre
                                    438, //Bruja blanca fuerte
                                    236, //Guerrero esqueleto
                                    245, //Sombra nocturna
                                    439, //Guerrero esqueleto resucitado
                                    272, //Arquero calavera
                                    274, //Guerrero calavera
                                    2691, //Frankenstein
                                    };
                                    break;

                                case 2: // Ice Costume
                                    possibleTransforms = new int []
                                    {
                                    543,
                                    544,
                                    545,
                                    552,
                                    666,
                                    668,
                                    727,
                                    753,
                                    754
                                    
                                    ,
                                    };
                                    break;

                                case 3: // Bushtail Costume
                                case 4:
                                    possibleTransforms = new int[]
                                    {
                                        156,
                                    };
                                    break;
                            }

                            if (possibleTransforms != null)
                            {
                                session.Character.IsMorphed = true;
                                session.Character.Morph = 1000 + possibleTransforms[ServerManager.RandomNumber(0, possibleTransforms.Length)];
                                session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
                                if (VNum != 1914)
                                {
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                }
                            }
                        }
                    }
                    break;

                default:
                    switch (EffectValue)
                    {
                        // Angel Base Flag
                        case 965:
                        // Demon Base Flag
                        case 966:
                            if (ServerManager.Instance.ChannelId == 51 && session.CurrentMapInstance?.Map.MapId != 130 && session.CurrentMapInstance?.Map.MapId != 131 && EffectValue - 964 == (short)session.Character.Faction)
                            {
                                session.CurrentMapInstance?.SummonMonster(new MonsterToSummon((short)EffectValue, new MapCell { X = session.Character.PositionX, Y = session.Character.PositionY }, null, false, isHostile: false, aliveTime: 1800));
                                session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                            }
                            break;

                        default:
                            switch (VNum)
                            {
                                case 5856: // Partner Slot Expansion
                                case 9113: // Partner Slot Expansion (Limited)
                                    {
                                        if (Option == 0)
                                        {
                                            session.SendPacket($"qna #u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^2 {Language.Instance.GetMessageFromKey("ASK_PARTNER_MAX")}");
                                        }
                                        else if (session.Character.MaxPartnerCount < 12)
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                            session.Character.MaxPartnerCount++;
                                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("GET_PARTNER_PLACES"), 10));
                                            session.SendPacket(session.Character.GenerateScpStc());
                                        }
                                    }
                                    break;

                                case 5931: // Partner skill ticket (One)
                                    {
                                        if (session?.Character?.Mates == null)
                                        {
                                            return;
                                        }

                                        if (packetsplit.Length != 10 || !byte.TryParse(packetsplit[8], out byte petId) || !byte.TryParse(packetsplit[9], out byte castId))
                                        {
                                            return;
                                        }

                                        if (castId < 0 || castId > 2)
                                        {
                                            return;
                                        }

                                        Mate partner = session.Character.Mates.ToList().FirstOrDefault(s => s.IsTeamMember && s.MateType == MateType.Partner && s.PetId == petId);

                                        if (partner?.Sp == null || partner.IsUsingSp)
                                        {
                                            return;
                                        }

                                        PartnerSkill skill = partner.Sp.GetSkill(castId);

                                        if (skill?.Skill == null)
                                        {
                                            return;
                                        }

                                        if (skill.Level == (byte)PartnerSkillLevelType.S)
                                        {
                                            return;
                                        }

                                        if (partner.Sp.RemoveSkill(castId))
                                        {
                                            session.Character.Inventory.RemoveItemFromInventory(inv.Id);

                                            partner.Sp.ReloadSkills();
                                            partner.Sp.FullXp();

                                            session.SendPacket(UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("PSP_SKILL_RESETTED"), 1));
                                        }

                                        session.SendPacket(partner.GenerateScPacket());
                                    }
                                    break;
                                case 5932: // Partner skill ticket (all)
                                    {
                                        if (packetsplit.Length != 10
                                            || session?.Character?.Mates == null)
                                        {
                                            return;
                                        }

                                        if (!byte.TryParse(packetsplit[8], out byte petId) || !byte.TryParse(packetsplit[9], out byte castId))
                                        {
                                            return;
                                        }

                                        if (castId < 0 || castId > 2)
                                        {
                                            return;
                                        }

                                        Mate partner = session.Character.Mates.ToList().FirstOrDefault(s => s.IsTeamMember && s.MateType == MateType.Partner && s.PetId == petId);

                                        if (partner?.Sp == null || partner.IsUsingSp)
                                        {
                                            return;
                                        }

                                        if (partner.Sp.GetSkillsCount() < 1)
                                        {
                                            return;
                                        }

                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);

                                        partner.Sp.ClearSkills();
                                        partner.Sp.FullXp();

                                        session.SendPacket(UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("PSP_ALL_SKILLS_RESETTED"), 1));

                                        session.SendPacket(partner.GenerateScPacket());
                                    }
                                    break;

                                    // Empty Box A : Add your gift
                                case 1289:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        //session.Character.GiftAdd(???, 1);
                                    }
                                    break;
                                // Empty Box B : Add your gift
                                case 1290:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        //session.Character.GiftAdd(???, 1);
                                    }
                                    break;
                                // Empty Box C : Add your gift
                                case 1291:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        //session.Character.GiftAdd(???, 1);
                                    }
                                    break;
                                // Empty Box A + B + C : Add your gift
                                case 1372:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        //session.Character.GiftAdd(???, 1);
                                    }
                                    break;
                                // Empty box
                                case 1438:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        //session.Character.GiftAdd(???, 1);
                                    }
                                    break;
                                // Box of gifts
                                case 1508:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(1286, 3);
                                        session.Character.GiftAdd(1285, 2);
                                        session.Character.GiftAdd(2159, 1);
                                        session.Character.GiftAdd(2160, 5);
                                    }
                                    break;
                                // Dalmatian costume box set (30 Days)
                                case 1517:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(783, 1);
                                        session.Character.GiftAdd(835, 1);
                                    }
                                    break;
                                // Rottweiler costume box set (Perm)
                                case 1518:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(787, 1);
                                        session.Character.GiftAdd(837, 1);
                                    }
                                    break;
                                // Cat siamois costume box set (30 Days)
                                case 1519:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(789, 1);
                                        session.Character.GiftAdd(841, 1);
                                    }
                                    break;
                                // Blue russian costume box set (30 Days)
                                case 1520:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(793, 1);
                                        session.Character.GiftAdd(845, 1);
                                    }
                                    break;
                                // Dark lion costume box set (30 Days)
                                case 1521:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(808, 1);
                                        session.Character.GiftAdd(856, 1);
                                    }
                                    break;
                                // Light lion costume box set (Perm)
                                case 1522:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(806, 1);
                                        session.Character.GiftAdd(854, 1);
                                    }
                                    break;
                                // Bouledog costume box set (30 Days)
                                case 1523:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(811, 1);
                                        session.Character.GiftAdd(859, 1);
                                    }
                                    break;
                                // St Bernard costume box set (Perm)
                                case 1524:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(815, 1);
                                        session.Character.GiftAdd(863, 1);
                                    }
                                    break;
                                // Birman cat costume box set (30 Days)
                                case 1525:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(817, 1);
                                        session.Character.GiftAdd(865, 1);
                                    }
                                    break;
                                // Korat costume box set (Perm)
                                case 1526:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(821, 1);
                                        session.Character.GiftAdd(869, 1);
                                    }
                                    break;
                                // Black lion costume box set (30 Days)
                                case 1527:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(832, 1);
                                        session.Character.GiftAdd(880, 1);
                                    }
                                    break;
                                // Gold lion costume box set (Perm)
                                case 1528:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(830, 1);
                                        session.Character.GiftAdd(878, 1);
                                    }
                                    break;
                                // Mars hare costume box set (30 Days)
                                case 1529:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(823, 1);
                                        session.Character.GiftAdd(871, 1);
                                    }
                                    break;
                                // White mars hare costume box set (Perm)
                                case 1530:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(827, 1);
                                        session.Character.GiftAdd(875, 1);
                                    }
                                    break;
                                // Mars hare costume box set (30 Days)
                                case 1531:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(798, 1);
                                        session.Character.GiftAdd(850, 1);
                                    }
                                    break;
                                // Mars hare costume box set (Perm)
                                case 1532:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(796, 1);
                                        session.Character.GiftAdd(848, 1);
                                    }
                                    break;
                                // Eva Halloween Event Box (Make what you want in relation with the event)      
                                case 1539:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        // session.Character.GiftAdd(???, 1);
                                        // session.Character.GiftAdd(???, 1);
                                    }
                                    break;
                                // Eva Halloween Event Box (Make what you want in relation with the event)     
                                case 1540:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        // session.Character.GiftAdd(???, 1);
                                        // session.Character.GiftAdd(???, 1);
                                    }
                                    break;
                                // Soraya Halloween Event Box (Make what you want in relation with the event)
                                case 1541:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                       // session.Character.GiftAdd(???, 1);
                                       // session.Character.GiftAdd(???, 1);
                                    }
                                    break;

                                case 9577: //Rocketeer (Permanent)  BOX kentao
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);

                                        // costume
                                        session.Character.GiftAdd(4679, 1);

                                        // chapeau
                                        session.Character.GiftAdd(4681, 1);

                                        // aile
                                        session.Character.GiftAdd(4683, 1);
                                    }
                                    break;

                                // Style box A & E (Costume) & (Hat) - (Male) (Female)
                                case 1542:
                                case 1543:
                                case 1544:
                                case 1545:
                                case 1546:
                                case 1547:
                                case 1548:
                                case 1549:
                                    Item ItemBox = inv.Item;
                                    if (ItemBox.VNum == 1542)
                                    {
                                    session.Character.GiftAdd(881, 1);
                                    session.Character.GiftAdd(879, 1);
                                    }
                                    if (ItemBox.VNum == 1543)
                                    {
                                     session.Character.GiftAdd(831, 1);
                                     session.Character.GiftAdd(833, 1);
                                    }
                                    if (ItemBox.VNum == 1544)
                                    {
                                    session.Character.GiftAdd(867, 1);
                                    session.Character.GiftAdd(869, 1);
                                    }
                                    if (ItemBox.VNum == 1545)
                                    {
                                    session.Character.GiftAdd(819, 1);
                                    session.Character.GiftAdd(821, 1);
                                    }
                                    if (ItemBox.VNum == 1546)
                                    {
                                        session.Character.GiftAdd(855, 1);
                                        session.Character.GiftAdd(857, 1);
                                    }
                                    if (ItemBox.VNum == 1547)
                                    {
                                        session.Character.GiftAdd(807, 1);
                                        session.Character.GiftAdd(809, 1);
                                    }
                                    if (ItemBox.VNum == 1548)
                                    {
                                        session.Character.GiftAdd(843, 1);
                                        session.Character.GiftAdd(845, 1);
                                    }
                                    if (ItemBox.VNum == 1549)
                                    {
                                        session.Character.GiftAdd(791, 1);
                                        session.Character.GiftAdd(793, 1);
                                    }
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    session.Character.GiftAdd(1004, 1);
                                    session.Character.GiftAdd(1007, 1);
                                    session.Character.GiftAdd(1010, 1);
                                    session.Character.GiftAdd(1078, 1);
                                    break;

                                case 1550:
                                case 1551:
                                case 1552:
                                case 1553:
                                    // Stopped at 1553 : TODO : Add all boxes after vnum 1553
                                    // Style box C (Costume) & (Hat) - (Male) (Female)
                                    Item ItemBox1 = inv.Item;
                                    if (ItemBox1.VNum == 1550)
                                    {
                                        session.Character.GiftAdd(873, 1);
                                        session.Character.GiftAdd(875, 1);
                                    }
                                    if (ItemBox1.VNum == 1551)
                                    {
                                        session.Character.GiftAdd(825, 1);
                                        session.Character.GiftAdd(827, 1);
                                    }
                                    if (ItemBox1.VNum == 1552)
                                    {
                                        session.Character.GiftAdd(870, 1);
                                        session.Character.GiftAdd(872, 1);
                                    }
                                    if (ItemBox1.VNum == 1553)
                                    {
                                        session.Character.GiftAdd(822, 1);
                                        session.Character.GiftAdd(824, 1);
                                    }
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    session.Character.GiftAdd(1004, 30);
                                    session.Character.GiftAdd(1007, 30);
                                    session.Character.GiftAdd(1010, 30);
                                    session.Character.GiftAdd(1078, 1);
                                    break;

                                //Weeding box
                                case 5018:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        // Cupid's Arrow
                                        session.Character.GiftAdd(1981, 1);
                                        // Wedding Clothes (7 Days)
                                        session.Character.GiftAdd(982, 1);
                                        session.Character.GiftAdd(982, 1);
                                        // Wedding Hairstyle (7 Days)
                                        session.Character.GiftAdd(986, 1);
                                        session.Character.GiftAdd(986, 1);
                                        // Double Fireworks
                                        session.Character.GiftAdd(1984, 10);
                                        // Fountain Firework (Yellow)
                                        session.Character.GiftAdd(1986, 10);
                                        // Heart Firework (Red)
                                        session.Character.GiftAdd(1988, 10);
                                        //Sp : Active only for event etc
                                       /* session.Character.GiftAdd(4416, 1);
                                        session.Character.GiftAdd(4416, 1);*/
                                    }
                                    break;

                                // Angel wings small box
                                case 5101:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(2282, 20);
                                    }
                                    break;
                                // Angel wings big box
                                case 5102:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(2282, 50);
                                    }
                                    break;
                                // Pegasus box
                                case 5280:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(929, 1);
                                    }
                                    break;
                                case 5610:// Viking Costume Set
                                    {
                                        session.Character.GiftAdd(4301, 1);
                                        session.Character.GiftAdd(4303, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5051: // Aqua Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4064, 1);
                                        session.Character.GiftAdd(4065, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5080: // Christmas Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4074, 1);
                                        session.Character.GiftAdd(4077, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5183: // Blue Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4107, 1);
                                        session.Character.GiftAdd(4114, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5184: // Green Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4108, 1);
                                        session.Character.GiftAdd(4115, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5185: // Green Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4109, 1);
                                        session.Character.GiftAdd(4116, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5186: // Red Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4110, 1);
                                        session.Character.GiftAdd(4117, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5187: // Pink Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4111, 1);
                                        session.Character.GiftAdd(4118, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5188: // Light blue Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4112, 1);
                                        session.Character.GiftAdd(4119, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5189: // Yellow Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(4113, 1);
                                        session.Character.GiftAdd(4120, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5190: // Classic Bushtail Costume Set
                                    {
                                        session.Character.GiftAdd(970, 1);
                                        session.Character.GiftAdd(972, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5267: // Cute rabbit (h) Costume Set
                                    {
                                        session.Character.GiftAdd(4138, 1);
                                        session.Character.GiftAdd(4146, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5268: // Cute rabbit (f) Costume Set
                                    {
                                        session.Character.GiftAdd(4142, 1);
                                        session.Character.GiftAdd(4150, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5592: // Illusionist Costume Set
                                    {
                                        session.Character.GiftAdd(4266, 1);
                                        session.Character.GiftAdd(4268, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5302: // Fox Oto Costume Set
                                    {
                                        session.Character.GiftAdd(4177, 1);
                                        session.Character.GiftAdd(4179, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5486: // Adorable tiger Costume Set
                                    {
                                        session.Character.GiftAdd(4244, 1);
                                        session.Character.GiftAdd(4252, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5487: // White tiger Costume Set
                                    {
                                        session.Character.GiftAdd(4248, 1);
                                        session.Character.GiftAdd(4256, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5733: // Easter Rabbit Costume Set
                                    {
                                        session.Character.GiftAdd(4429, 1);
                                        session.Character.GiftAdd(4433, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5737: // Fairy costume Set
                                    {
                                        session.Character.GiftAdd(4439, 1);
                                        session.Character.GiftAdd(4441, 1);
                                        session.Character.GiftAdd(4443, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5572: // Illusionist Costume Set
                                    {
                                        session.Character.GiftAdd(4258, 1);
                                        session.Character.GiftAdd(4260, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                case 5789: // Tropical Costume Set
                                    {
                                        session.Character.GiftAdd(4527, 1);
                                        session.Character.GiftAdd(4529, 1);
                                        session.Character.GiftAdd(4531, 1);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        break;
                                    }
                                // Football costume pack permanant
                                case 5441:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4195, 1);
                                        session.Character.GiftAdd(4196, 1);
                                    }
                                    break;

                                    // Scooter box
                                case 1926:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(1906, 1);
                                    }
                                    break;
                                // Tapis box
                                case 1927:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(1907, 1);
                                    }
                                    break;
                                // White tiger box
                                case 1966:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(1965, 1);
                                    }
                                    break;
                                // Balai box
                                case 5153:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5152, 1);
                                    }
                                    break;
                                // Yakari box
                                case 5181:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5173, 1);
                                    }
                                    break;
                                // Mac Umulonimbus box
                                case 5118:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5117, 1);
                                    }
                                    break;
                                // Nossi box
                                case 5197:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5196, 1);
                                    }
                                    break;
                                // Chameau box
                                case 5915:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5914, 1);
                                    }
                                    break;
                                // Rollers box
                                case 5235:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5234, 1);
                                    }
                                    break;
                                // VTT box
                                case 5233:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5232, 1);
                                    }
                                    break;
                                // Skateboard box
                                case 5237:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5236, 1);
                                    }
                                    break;
                                // Pack Amélioration 
                                case 5229:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);                                  
                                        session.Character.GiftAdd(2282, 999);
                                        session.Character.GiftAdd(1030, 999);
                                        session.Character.GiftAdd(2630, 999);
                                       
                                    }
                                    break;
                                    // Pack Super Amélioration
                                case 5230:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(2282, 999);
                                        session.Character.GiftAdd(1030, 999);
                                        session.Character.GiftAdd(2630, 999);
                                        session.Character.GiftAdd(2512, 999);
                                        session.Character.GiftAdd(2513, 999);
                                        session.Character.GiftAdd(2511, 999);
                                        session.Character.GiftAdd(2630, 999);

                                    }
                                    break;

                                case 1717: // Donation Tatouage
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);

                                        // 25x Parchemin de préservation de tatouage
                                        session.Character.GiftAdd(5815, 100);

                                        // 10x Détatoueur
                                        session.Character.GiftAdd(5799, 100);

                                        // 75x Branche du bois des esprits
                                        session.Character.GiftAdd(2460, 300);

                                        // 75x Coeur de golem
                                        session.Character.GiftAdd(2406, 300);


                                        session.Character.GiftAdd(2406, 300);


                                        // 75X poudre runique Loa
                                        session.Character.GiftAdd(2411, 300);


                                        // 75X cuir animal de  mourice
                                        session.Character.GiftAdd(2459, 300);

                                        // 75x obsidienne de mourice
                                        session.Character.GiftAdd(2416, 300);


                                        session.Character.GiftAdd(2464, 300);


                                        session.Character.GiftAdd(2413, 300);


                                        session.Character.GiftAdd(2366, 300);


                                        session.Character.GiftAdd(2406, 300);


                                        session.Character.GiftAdd(2410, 300);


                                        session.Character.GiftAdd(5762, 300);


                                        session.Character.GiftAdd(2412, 300);


                                        session.Character.GiftAdd(2466, 300);


                                        session.Character.GiftAdd(2408, 300);

                                        session.Character.GiftAdd(2460, 300);


                                    }
                                    break;

                                case 1719: // Pack Fée Zénas
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);


                                        session.Character.GiftAdd(10536, 1);


                                        session.Character.GiftAdd(10540, 1);


                                        session.Character.GiftAdd(10544, 1);


                                        session.Character.GiftAdd(10548, 1);

                                    }
                                    break;

                                case 1720: // Pack Fée érénia
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);


                                        session.Character.GiftAdd(10552, 1);


                                        session.Character.GiftAdd(10556, 1);


                                        session.Character.GiftAdd(10560, 1);


                                        session.Character.GiftAdd(10564, 1);

                                    }
                                    break;

                                // Snowboard box
                                case 5241:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5240, 1);
                                    }
                                    break;
                                // Pack Débutant
                                case 5239:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(1125, 1);
                                        session.Character.GiftAdd(5948, 1);
                                        session.Character.GiftAdd(1126, 1);
                                    }
                                    break;
                                // Pack Intermédiaire
                                case 5227:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(1125, 4);
                                        session.Character.GiftAdd(5948, 2);
                                        session.Character.GiftAdd(1126, 4);
                                        session.Character.GiftAdd(1144, 1);
                                    }
                                    break;
                                // Pack Expert
                                case 5361:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(1125, 9);
                                        session.Character.GiftAdd(5948, 4);
                                        session.Character.GiftAdd(1126, 9);
                                        session.Character.GiftAdd(1144, 1);
                                        session.Character.GiftAdd(5950, 1);
                                        session.Character.GiftAdd(5949, 1);
                                    }
                                    break;
                                // Jaguar box
                                case 5835:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5834, 1);
                                    }
                                    break;
                                // Traineau box
                                case 5713:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5712, 1);
                                    }
                                    break;
                                // Ski invisible box
                                case 5744:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(5743, 1);
                                    }
                                    break;

                                // Magic light costume set box
                                case 5358:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4185, 1);
                                        session.Character.GiftAdd(4181, 1);
                                    }
                                    break;

                                //  Magic Dark costume set box
                                case 5359:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4187, 1);
                                        session.Character.GiftAdd(4183, 1);
                                    }
                                    break;

                                // Devil Fire Horn costume set box
                                case 5716:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4409, 1);
                                        session.Character.GiftAdd(4411, 1);
                                        session.Character.GiftAdd(4435, 1);
                                    }
                                    break;

                                // Desert costume set box
                                case 5638:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4317, 1);
                                        session.Character.GiftAdd(4321, 1);
                                    }
                                    break;

                                // Dancing costume set box
                                case 5639:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4319, 1);
                                        session.Character.GiftAdd(4323, 1);
                                    }
                                    break;

                                // Policeman costume set box
                                case 5599:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4283, 1);
                                        session.Character.GiftAdd(4285, 1);
                                    }
                                    break;

                                // Hotel manager costume set box
                                case 5604:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4287, 1);
                                        session.Character.GiftAdd(4289, 1);
                                    }
                                    break;

                                // Nutcracker costume set box
                                case 5878:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4827, 1);
                                        session.Character.GiftAdd(4829, 1);
                                    }
                                    break;

                                // Ice Witch costume set box
                                case 5816:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4534, 1);
                                        session.Character.GiftAdd(4536, 1);
                                        session.Character.GiftAdd(4538, 1);
                                    }
                                    break;

                                // Pumpkin Witch/Pumpkin Knight Set
                                case 9187:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4553, 1);
                                        session.Character.GiftAdd(4555, 1);
                                        session.Character.GiftAdd(4557, 1);
                                    }
                                    break;
                               

                                // Death Lancer Set
                                case 9215:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4568, 1);
                                        session.Character.GiftAdd(4570, 1);
                                        session.Character.GiftAdd(4572, 1);
                                    }
                                    break;

                                // Honeybee Costume Set
                                case 9263:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4596, 1);
                                        session.Character.GiftAdd(4598, 1);
                                        session.Character.GiftAdd(4600, 1);
                                    }
                                    break;

                                // Plague Doctor/Nurse Set
                                case 9292:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4614, 1);
                                        session.Character.GiftAdd(4616, 1);
                                    }
                                    break;

                                // Wonderland Set
                                case 9234:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4577, 1);
                                        session.Character.GiftAdd(4579, 1);
                                    }
                                    break;

                                // Magician Noz costume set box
                                case 9147:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(4542, 1);
                                        session.Character.GiftAdd(4543, 1);
                                    }
                                    break;

                                // Turtlecopter box
                                case 9151:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(9149, 1);
                                    }
                                    break;

                                // Event Upgrade Scrolls
                                case 5107:
                                    session.SendPacket("wopen 35 0 0");
                                    break;
                                case 5207:
                                    session.SendPacket("wopen 38 0 0");
                                    break;
                                case 5519:
                                    session.SendPacket("wopen 42 0 0");
                                    break;
                                case 10000:
                                    if (EffectValue != 0)
                                    {
                                        if (session.Character.IsSitting)
                                        {
                                            session.Character.IsSitting = false;
                                            session.SendPacket(session.Character.GenerateRest());
                                        }
                                        session.SendPacket(UserInterfaceHelper.GenerateGuri(12, 1, session.Character.CharacterId, EffectValue));
                                    }
                                    break;

                                // Martial Artist Starter Pack
                                case 5832:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);

                                        // Steel Punch
                                        session.Character.GiftAdd(4756, 1, 5);

                                        // Trainee Martial Artist's Uniform
                                        session.Character.GiftAdd(4757, 1, 5);

                                        // Mystical Glacier Stone
                                        session.Character.GiftAdd(4504, 1);

                                        // SP1 MARTIAL
                                        session.Character.GiftAdd(4486, 1);

                                        // MEDAL REPUTATION
                                        session.Character.GiftAdd(9101, 1);

                                        // Hero's Amulet of Fire
                                        session.Character.GiftAdd(4503, 1);

                                        // Fairy Fire/Water/Light/Dark (30%)
                                        for (short itemVNum = 884; itemVNum <= 887; itemVNum++)
                                        {
                                            session.Character.GiftAdd(itemVNum, 1);
                                        }
                                    }
                                    break;

                                // Soulstone Blessing
                                case 1362:
                                case 5195:
                                case 5211:
                                case 9075:
                                    if (!session.Character.Buff.ContainsKey(146))
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.AddStaticBuff(new StaticBuffDTO { CardId = 146 });
                                    }
                                    else
                                    {
                                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_IN_USE"), 0));
                                    }
                                    break;
                                case 1428:
                                    session.SendPacket("guri 18 1");
                                    break;
                                case 1429:
                                    session.SendPacket("guri 18 0");
                                    break;
                                case 1904:
                                    short[] items = { 1894, 1895, 1896, 1897, 1898, 1899, 1900, 1901, 1902, 1903 };
                                    for (int i = 0; i < 5; i++)
                                    {
                                        session.Character.GiftAdd(items[ServerManager.RandomNumber(0, items.Length)], 1);
                                    }
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    break;
                                case 5370:
                                case 9116:
                                    if (session.Character.Buff.Any(s => s.Card.CardId == 393))
                                    {
                                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("ALREADY_GOT_BUFF"), session.Character.Buff.FirstOrDefault(s => s.Card.CardId == 393)?.Card.Name), 10));
                                        return;
                                    }
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    session.Character.AddStaticBuff(new StaticBuffDTO { CardId = 393 });
                                    break;

                                    //Event Lunar New Year Random box
                                case 5083:
                                    int rnd110 = ServerManager.RandomNumber(0, 100);
                                    Item Item4 = inv.Item;
                                    short[] vnums110 = null;
                                    if (rnd110 < 2)
                                    {
                                        vnums110 = new short[] { 448 };
                                        byte[] counts = { 1, };
                                        int item = ServerManager.RandomNumber(0, 1);
                                        session.Character.GiftAdd(vnums110[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd110 < 10)
                                    {
                                        vnums110 = new short[] { 930 };
                                        byte[] counts = { 1, };
                                        int item = ServerManager.RandomNumber(0, 1);
                                        session.Character.GiftAdd(vnums110[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else
                                    {
                                        vnums110 = new short[] { 822, 870, 797, 849, 2399, 2400, 2401, 5084 };
                                        byte[] counts = { 1, 1, 1, 1, 15, 15, 15, 2};
                                        int item = ServerManager.RandomNumber(0, 8);
                                        session.Character.GiftAdd(vnums110[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    break;

                                //Sellaim, Woondine, Eperial, Turik random boxes
                                case 5004:
                                case 5005:
                                case 5006:
                                case 5007:
                                    int rnd100 = ServerManager.RandomNumber(0, 100);
                                    Item Item = inv.Item;
                                    short[] vnums100 = null;
                                    if (rnd100 < 15 && Item.VNum == 5004)
                                    {
                                        vnums100 = new short[] { 274, 1218 };
                                        byte[] counts = { 1, 1, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums100[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd100 < 15 && Item.VNum == 5005)
                                    {
                                        vnums100 = new short[] { 275, 1218 };
                                        byte[] counts = { 1, 1, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums100[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd100 < 15 && Item.VNum == 5006)
                                    {
                                        vnums100 = new short[] { 276, 1218 };
                                        byte[] counts = { 1, 1, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums100[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd100 < 15 && Item.VNum == 5007)
                                    {
                                        vnums100 = new short[] { 277, 1218 };
                                        byte[] counts = { 1, 1, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums100[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else
                                    {
                                        short[] vnums2 = null;
                                        vnums2 = new short[] { 1904, 1296, 1296, 1122, 2282, 1219, 1119, 2158 };
                                        byte[] counts2 = { 1, 5, 3, 45, 40, 1, 2, 5 };
                                        int item2 = ServerManager.RandomNumber(0, 8);
                                        session.Character.GiftAdd(vnums2[item2], counts2[item2]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    break;

                                //Great Sellaim, Great Woondine, Great Eperial, Great Turik random boxes
                                case 5014:
                                case 5015:
                                case 5016:
                                case 5017:
                                    int rnd101 = ServerManager.RandomNumber(0, 100);
                                    Item Item1 = inv.Item;
                                    short[] vnums101 = null;
                                    if (rnd101 < 10 && Item1.VNum == 5014)
                                    {
                                        vnums101 = new short[] { 278, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums101[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd101 < 10 && Item1.VNum == 5015)
                                    {
                                        vnums101 = new short[] { 279, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums101[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd101 < 10 && Item1.VNum == 5016)
                                    {
                                        vnums101 = new short[] { 280, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums101[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd101 < 10 && Item1.VNum == 5017)
                                    {
                                        vnums101 = new short[] { 281, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums101[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else
                                    {
                                        short[] vnums2 = null;
                                        vnums2 = new short[] { 1904, 1296, 1286, 1122, 2282, 1219, 1119, 2158 };
                                        byte[] counts2 = { 2, 10, 6, 90, 80, 2, 2, 10 };
                                        int item2 = ServerManager.RandomNumber(0, 8);
                                        session.Character.GiftAdd(vnums2[item2], counts2[item2]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    break;

                                //Ratufu random boxes
                                case 5019:
                                case 5020:
                                case 5021:
                                case 5022:
                                case 5023:
                                case 5024:
                                case 5044:
                                case 5045:
                                case 5046:
                                case 5047:
                                case 5048:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(10568, 1);
                                        session.Character.GiftAdd(10572, 1);
                                        session.Character.GiftAdd(10576, 1);
                                        session.Character.GiftAdd(10580, 1);

                                    }
                                    break;
                                case 5049:

                                    int rnd102 = ServerManager.RandomNumber(0, 100);
                                    Item Item2 = inv.Item;
                                    short[] vnums102 = null;
                                    if (rnd102 < 10 && Item2.VNum == 5019)
                                    {
                                        vnums102 = new short[] { 888, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5020)
                                    {
                                        vnums102 = new short[] { 399, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5021)
                                    {
                                        vnums102 = new short[] { 447, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5022)
                                    {
                                        vnums102 = new short[] { 397, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5023)
                                    {
                                        vnums102 = new short[] { 192, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5024)
                                    {
                                        vnums102 = new short[] { 980, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5044)
                                    {
                                        vnums102 = new short[] { 398, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5045)
                                    {
                                        vnums102 = new short[] { 943, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5046)
                                    {
                                        vnums102 = new short[] { 4066, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5047)
                                    {
                                        vnums102 = new short[] { 444, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5048)
                                    {
                                        vnums102 = new short[] { 4061, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd102 < 10 && Item2.VNum == 5049)
                                    {
                                        vnums102 = new short[] { 4062, 1218 };
                                        byte[] counts = { 1, 2, };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums102[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else
                                    {
                                        short[] vnums2 = null;
                                        vnums2 = new short[] { 1904, 1296, 1286, 1122, 2282, 1219, 1119, 2158 };
                                        byte[] counts2 = { 2, 10, 6, 90, 80, 2, 2, 10 };
                                        int item2 = ServerManager.RandomNumber(0, 8);
                                        session.Character.GiftAdd(vnums2[item2], counts2[item2]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    break;


                                //God fairy random box
                                case 5279:
                                    {
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        session.Character.GiftAdd(10568, 1);
                                        session.Character.GiftAdd(10572, 1);
                                        session.Character.GiftAdd(10576, 1);
                                        session.Character.GiftAdd(10580, 1);

                                    }
                                    break;



                                // Agua bushitail random box 1
                                case 5052:
                                    int rnd103 = ServerManager.RandomNumber(0, 100);
                                    short[] vnums103 = null;
                                    short[] vnums104 = null;
                                    short[] vnums107 = null;
                                    if (rnd103 < 5)
                                    {
                                        vnums103 = new short[] { 5050 };
                                        vnums104 = new short[] { 5051 };
                                        byte[] counts = { 1, 1,};
                                        int item = ServerManager.RandomNumber(0, 1);
                                        session.Character.GiftAdd(vnums103[item], counts[item]);
                                        session.Character.GiftAdd(vnums104[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    if (rnd103 > 4 && rnd103 < 15)
                                    {
                                        vnums107 = new short[] { 5050, 5051, 1218 };
                                        byte[] counts = { 1, 1, 2 };
                                        int item = ServerManager.RandomNumber(0, 3);
                                        session.Character.GiftAdd(vnums107[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else if (rnd103 > 15)
                                    {
                                        short[] vnums2 = null;
                                        vnums2 = new short[] { 4064, 4065, 1296, 1945 };
                                        byte[] counts2 = { 1, 1, 8, 60 };
                                        int item2 = ServerManager.RandomNumber(0, 4);
                                        session.Character.GiftAdd(vnums2[item2], counts2[item2]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    break;

                                // Agua bushitail random box 2
                                case 5304:
                                    int rnd105 = ServerManager.RandomNumber(0, 100);
                                    short[] vnums105 = null;
                                    if (rnd105 < 5)
                                    {
                                        vnums105 = new short[] { 5051, 284 };
                                        byte[] counts = { 1, 1 };
                                        int item = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums105[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else
                                    {
                                        short[] vnums2 = null;
                                        vnums2 = new short[] { 1279, 2160, 1119, 1285, 1904, 1296, 1945 };
                                        byte[] counts2 = { 1, 40, 1, 16, 1, 10, 14};
                                        int item2 = ServerManager.RandomNumber(0, 7);
                                        session.Character.GiftAdd(vnums2[item2], counts2[item2]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    break;

                                // Unisex & co random box
                                case 5457:
                                    int rnd106 = ServerManager.RandomNumber(0, 100);
                                    short[] vnums106 = null;
                                    if (rnd106 < 15)
                                    {
                                        vnums106 = new short[] { 5183, 5184, 5185, 5186, 5187, 5188, 5189, 5190 };
                                        byte[] counts = { 1, 1, 1, 1 ,1 ,1 ,1 , 1};
                                        int item = ServerManager.RandomNumber(0, 8);
                                        session.Character.GiftAdd(vnums106[item], counts[item]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    else
                                    {
                                        short[] vnums2 = null;
                                        vnums2 = new short[] { 2282, 1030};
                                        byte[] counts2 = { 25, 25};
                                        int item2 = ServerManager.RandomNumber(0, 2);
                                        session.Character.GiftAdd(vnums2[item2], counts2[item2]);
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    }
                                    break;

                                case 5841:
                                    int rnd = ServerManager.RandomNumber(0, 1000);
                                    short[] vnums = null;
                                    if (rnd < 900)
                                    {
                                        vnums = new short[] { 4356, 4357, 4358, 4359 };
                                    }
                                    else
                                    {
                                        vnums = new short[] { 4360, 4361, 4362, 4363 };
                                    }
                                    session.Character.GiftAdd(vnums[ServerManager.RandomNumber(0, 4)], 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    break;
                                case 5916:
                                case 5927:
                                    session.Character.AddStaticBuff(new StaticBuffDTO
                                    {
                                        CardId = 340,
                                        CharacterId = session.Character.CharacterId,
                                        RemainingTime = 7200
                                    });
                                    session.Character.RemoveBuff(339);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    break;
                                case 5929:
                                case 5930:
                                    session.Character.AddStaticBuff(new StaticBuffDTO
                                    {
                                        CardId = 340,
                                        CharacterId = session.Character.CharacterId,
                                        RemainingTime = 600
                                    });
                                    session.Character.RemoveBuff(339);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    break;
                                case 5974:
                                    int rnd10000 = ServerManager.RandomNumber(0, 100);
                                    short[] vnums10000 = null;
                                    if (rnd10000 < 10)
                                    {
                                        vnums10000 = new short[] { 9212, 5560, 5800, 1270 };
                                        byte[] counts = { 1, 1, 1, 1, };
                                        int item = ServerManager.RandomNumber(0, 4);
                                        session.Character.GiftAdd(vnums10000[item], counts[item]);
                                        session.Character.DailyRewardChest += 1;
                                        session.SendPacket("msg 4 Congratulations! You received a Special Reward!");
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        if (session.Character.DailyRewardChest > 1)
                                        {
                                            session.SendPacket(session.Character.GenerateSay($"{session.Character.DailyRewardChest} Boxes opened.", 12));
                                        }
                                        else if (session.Character.DailyRewardChest < 1)
                                        {
                                            session.SendPacket(session.Character.GenerateSay($"{session.Character.DailyRewardChest} Boxes opened.", 12));
                                        }
                                    }
                                    else
                                    {
                                        short[] vnums2 = null;
                                        vnums2 = new short[] {  2282, 1030, 5871, 2480, 2032, 5916 };
                                        byte[] counts2 = {  100, 100, 100, 100, 100, 10 };
                                        int item2 = ServerManager.RandomNumber(0, 7);
                                        session.Character.GiftAdd(vnums2[item2], counts2[item2]);
                                        session.Character.DailyRewardChest += 1;
                                        session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                        if (session.Character.DailyRewardChest > 1)
                                        {
                                            session.SendPacket(session.Character.GenerateSay($"{session.Character.DailyRewardChest} Boxes opened.", 12));
                                        }
                                        else if (session.Character.DailyRewardChest < 1)
                                        {
                                            session.SendPacket(session.Character.GenerateSay($"{session.Character.DailyRewardChest} Boxes opened.", 12));
                                        }
                                    }

                                    switch (session.Character.DailyRewardChest)
                                    {
                                        case 7:
                                            break;

                                        case 14:
                                            break;

                                        case 21:
                                            break;

                                        case 28:
                                            session.Character.DailyRewardChest = 0; //<-- Reset to 0, so you can start over again
                                            break;
                                    }
                                    break;
                                case 5924: // prestige basto
                                    session.Character.PrestigeFunction(inv);
                                    break;

                                    


                                // BUFF BOOK
                                case 211:
                                    {
                                        session.Character.AddBuff(new Buff(151, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(152, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(153, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(155, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(157, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(72, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(91, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(67, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(134, session.Character.Level), session.Character.BattleEntity);
                                        session.Character.AddBuff(new Buff(89, session.Character.Level), session.Character.BattleEntity);
                                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("BUFF_ACTIVATED"), Name), 12));
                                    }
                                    break;

                                // Mother Nature's Rune Pack (limited)
                                case 9117:
                                    rnd = ServerManager.RandomNumber(0, 1000);
                                    vnums = null;
                                    if (rnd < 900)
                                    {
                                        vnums = new short[] { 8312, 8313, 8314, 8315 };
                                    }
                                    else
                                    {
                                        vnums = new short[] { 8316, 8317, 8318, 8319 };
                                    }
                                    session.Character.GiftAdd(vnums[ServerManager.RandomNumber(0, 4)], 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    break;

                                // Akamur gift
                                case 5909:
                                    rnd = ServerManager.RandomNumber(0, 1000);
                                    vnums = null;
                                    if (rnd < 500)
                                    {
                                        vnums = new short[] { 2390, 2506, 2503, 2504, 2505, 5910, 2379 };
                                    }
                                    else
                                    {
                                        vnums = new short[] { 1875, 2383, 2382, 2381, 5916, 2348, 2349 };
                                    }
                                    session.Character.GiftAdd(vnums[ServerManager.RandomNumber(0, 7)], 1);
                                    session.Character.Inventory.RemoveItemFromInventory(inv.Id);
                                    break;

                                default:
                                    IEnumerable<RollGeneratedItemDTO> roll = DAOFactory.RollGeneratedItemDAO.LoadByItemVNum(VNum);
                                    IEnumerable<RollGeneratedItemDTO> rollGeneratedItemDtos = roll as IList<RollGeneratedItemDTO> ?? roll.ToList();
                                    if (!rollGeneratedItemDtos.Any())
                                    {
                                        Logger.Warn(string.Format(Language.Instance.GetMessageFromKey("NO_HANDLER_ITEM"), GetType(), VNum, Effect, EffectValue));
                                        return;
                                    }
                                    int probabilities = rollGeneratedItemDtos.Where(s => s.Probability != 10000).Sum(s => s.Probability);
                                    int rnd2 = ServerManager.RandomNumber(0, probabilities);
                                    int currentrnd = 0;
                                    foreach (RollGeneratedItemDTO rollitem in rollGeneratedItemDtos.Where(s => s.Probability == 10000))
                                    {
                                        sbyte rare = 0;
                                        if (rollitem.IsRareRandom)
                                        {
                                            rnd = ServerManager.RandomNumber(0, 100);

                                            for (int j = ItemHelper.RareRate.Length - 1; j >= 0; j--)
                                            {
                                                if (rnd < ItemHelper.RareRate[j])
                                                {
                                                    rare = (sbyte)j;
                                                    break;
                                                }
                                            }
                                            if (rare < 1)
                                            {
                                                rare = 1;
                                            }
                                        }
                                        session.Character.GiftAdd(rollitem.ItemGeneratedVNum, rollitem.ItemGeneratedAmount, (byte)rare, design: rollitem.ItemGeneratedDesign);
                                    }
                                    foreach (RollGeneratedItemDTO rollitem in rollGeneratedItemDtos.Where(s => s.Probability != 10000).OrderBy(s => ServerManager.RandomNumber()))
                                    {
                                        sbyte rare = 0;
                                        if (rollitem.IsRareRandom)
                                        {
                                            rnd = ServerManager.RandomNumber(0, 100);

                                            for (int j = ItemHelper.RareRate.Length - 1; j >= 0; j--)
                                            {
                                                if (rnd < ItemHelper.RareRate[j])
                                                {
                                                    rare = (sbyte)j;
                                                    break;
                                                }
                                            }
                                            if (rare < 1)
                                            {
                                                rare = 1;
                                            }
                                        }

                                        currentrnd += rollitem.Probability;
                                        if (currentrnd < rnd2)
                                        {
                                            continue;
                                        }
                                        /*if (rollitem.IsSuperReward)
                                        {
                                            CommunicationServiceClient.Instance.SendMessageToCharacter(new SCSCharacterMessage
                                            {
                                                DestinationCharacterId = null,
                                                SourceCharacterId = session.Character.CharacterId,
                                                SourceWorldId = ServerManager.Instance.WorldId,
                                                Message = Language.Instance.GetMessageFromKey("SUPER_REWARD"),
                                                Type = MessageType.Shout
                                            });
                                        }*/
                                        session.Character.GiftAdd(rollitem.ItemGeneratedVNum, rollitem.ItemGeneratedAmount, (byte)rare, design: rollitem.ItemGeneratedDesign);//, rollitem.ItemGeneratedUpgrade);
                                        break;
                                    }
                                    session.Character.Inventory.RemoveItemAmount(VNum);
                                    break;
                            }
                            break;
                    }
                    break;
            }
            session.Character.IncrementQuests(QuestType.Use, inv.ItemVNum);

        }

        #endregion
    }
}