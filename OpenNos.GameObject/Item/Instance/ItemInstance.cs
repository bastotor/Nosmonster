﻿/*1
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
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenNos.GameObject.Networking;
using System.Text;
using OpenNos.GameObject.ConfigEXT;

namespace OpenNos.GameObject
{
    public class ItemInstance : ItemInstanceDTO
    {
        #region Members

        private Item _item;
        private long _transportId;
        private List<CellonOptionDTO> _cellonOptions;
        private List<ShellEffectDTO> _shellEffects;
        private List<RuneEffectDTO> _runeEffects;

   

        #endregion

        #region Instantiation

        public ItemInstance()
        {

        }

        public ItemInstance(short vNum, short amount)
        {
            ItemVNum = vNum;
            Amount = amount;
            Type = Item.Type;
        }

        public ItemInstance(ItemInstanceDTO input)
        {
            Ammo = input.Ammo;
            Amount = input.Amount;
            BoundCharacterId = input.BoundCharacterId;
            Cellon = input.Cellon;
            CharacterId = input.CharacterId;
            CloseDefence = input.CloseDefence;
            Concentrate = input.Concentrate;
            CriticalDodge = input.CriticalDodge;
            CriticalLuckRate = input.CriticalLuckRate;
            CriticalRate = input.CriticalRate;
            DamageMaximum = input.DamageMaximum;
            DamageMinimum = input.DamageMinimum;
            DarkElement = input.DarkElement;
            DarkResistance = input.DarkResistance;
            DefenceDodge = input.DefenceDodge;
            Design = input.Design;
            DistanceDefence = input.DistanceDefence;
            DistanceDefenceDodge = input.DistanceDefenceDodge;
            DurabilityPoint = input.DurabilityPoint;
            ElementRate = input.ElementRate;
            EquipmentSerialId = input.EquipmentSerialId;
            FireElement = input.FireElement;
            FireResistance = input.FireResistance;
            HitRate = input.HitRate;
            HoldingVNum = input.HoldingVNum;
            HP = input.HP;
            Id = input.Id;
            IsEmpty = input.IsEmpty;
            IsFixed = input.IsFixed;
            IsPartnerEquipment = input.IsPartnerEquipment;
            ItemDeleteTime = input.ItemDeleteTime;
            ItemVNum = input.ItemVNum;
            LightElement = input.LightElement;
            LightResistance = input.LightResistance;
            MagicDefence = input.MagicDefence;
            MaxElementRate = input.MaxElementRate;
            MP = input.MP;
            Rare = input.Rare;
            ShellRarity = input.ShellRarity;
            SlDamage = input.SlDamage;
            SlDefence = input.SlDefence;
            SlElement = input.SlElement;
            SlHP = input.SlHP;
            Slot = input.Slot;
            SpDamage = input.SpDamage;
            SpDark = input.SpDark;
            SpDefence = input.SpDefence;
            SpElement = input.SpElement;
            SpFire = input.SpFire;
            SpHP = input.SpHP;
            SpLevel = input.SpLevel;
            SpLight = input.SpLight;
            SpStoneUpgrade = input.SpStoneUpgrade;
            SpWater = input.SpWater;
            Type = input.Type;
            Upgrade = input.Upgrade;
            WaterElement = input.WaterElement;
            WaterResistance = input.WaterResistance;
            XP = input.XP;
            RuneAmount = input.RuneAmount;
            IsBreaked = input.IsBreaked;
            ElementSpType = input.ElementSpType;
        }

        #endregion

        #region Properties

        public bool IsBound => BoundCharacterId.HasValue && Item.ItemType != ItemType.Armor && Item.ItemType != ItemType.Weapon;

        public Item Item => _item ?? (_item = IsPartnerEquipment && HoldingVNum != 0 ? ServerManager.GetItem(HoldingVNum) : ServerManager.GetItem(ItemVNum));

        public long TransportId
        {
            get
            {
                if (_transportId == 0)
                {
                    // create transportId thru factory
                    _transportId = TransportFactory.Instance.GenerateTransportId();
                }

                return _transportId;
            }
        }

        public List<CellonOptionDTO> CellonOptions => _cellonOptions ?? (_cellonOptions = DAOFactory.CellonOptionDAO.GetOptionsByWearableInstanceId(EquipmentSerialId == Guid.Empty ? EquipmentSerialId = Guid.NewGuid() : EquipmentSerialId).ToList());

        public List<ShellEffectDTO> ShellEffects => _shellEffects ?? (_shellEffects = DAOFactory.ShellEffectDAO.LoadByEquipmentSerialId(EquipmentSerialId == Guid.Empty ? EquipmentSerialId = Guid.NewGuid() : EquipmentSerialId).ToList());

        public List<RuneEffectDTO> RuneEffects => _runeEffects ?? (_runeEffects = DAOFactory.RuneEffectDAO
                                                      .LoadByEquipmentSerialId(
                                                          EquipmentSerialId == Guid.Empty
                                                              ? EquipmentSerialId = Guid.NewGuid()
                                                              : EquipmentSerialId).ToList());

        #endregion

        #region Methods

        public ItemInstance DeepCopy() => (ItemInstance)MemberwiseClone();

        public string GenerateFStash() => $"f_stash {GenerateStashPacket()}";

        public string GenerateInventoryAdd()
        {
            switch (Type)
            {
                case InventoryType.Equipment:
                    return $"ivn 0 {Slot}.{ItemVNum}.{Rare}.{(Item.IsColored ? Design : Upgrade)}.{SpStoneUpgrade}.{RuneAmount}";

                case InventoryType.Main:
                    return $"ivn 1 {Slot}.{ItemVNum}.{Amount}.0";

                case InventoryType.Etc:
                    return $"ivn 2 {Slot}.{ItemVNum}.{Amount}.0";

                case InventoryType.Miniland:
                    return $"ivn 3 {Slot}.{ItemVNum}.{Amount}";

                case InventoryType.Specialist:
                    return $"ivn 6 {Slot}.{ItemVNum}.{Rare}.{Upgrade}.{SpStoneUpgrade}";

                case InventoryType.Costume:
                    return $"ivn 7 {Slot}.{ItemVNum}.{Rare}.{Upgrade}.0";
            }
            return "";
        }

        public string GeneratePStash() => $"pstash {GenerateStashPacket()}";

        public string GenerateStash() => $"stash {GenerateStashPacket()}";

        public string GenerateStashPacket()
        {
            string packet = $"{Slot}.{ItemVNum}.{(byte)Item.Type}";
            switch (Item.Type)
            {
                case InventoryType.Equipment:
                    return packet + $".{Amount}.{Rare}.{Upgrade}";

                case InventoryType.Specialist:
                    return packet + $".{Upgrade}.{SpStoneUpgrade}.0";

                default:
                    return packet + $".{Amount}.0.0";
            }
        }

        public string GeneratePslInfo()
        {
            PartnerSp partnerSp = new PartnerSp(this);

            return $"pslinfo {Item.VNum} {Item.Element} {Item.ElementRate} {Item.LevelJobMinimum} {Item.Speed} {Item.FireResistance} {Item.WaterResistance} {Item.LightResistance} {Item.DarkResistance}{partnerSp.GenerateSkills()}";
        }

        public string GenerateSlInfo(ClientSession session = null)
        {
            int freepoint = CharacterHelper.SPPoint(SpLevel, Upgrade) - SlDamage - SlHP - SlElement - SlDefence;
            int slElementShell = 0;
            int slHpShell = 0;
            int slDefenceShell = 0;
            int slHitShell = 0;

            if (session != null)
            {
                ItemInstance mainWeapon = session.Character?.Inventory.LoadBySlotAndType((byte)EquipmentType.MainWeapon, InventoryType.Wear);
                ItemInstance secondaryWeapon = session.Character?.Inventory.LoadBySlotAndType((byte)EquipmentType.SecondaryWeapon, InventoryType.Wear);

                List<ShellEffectDTO> effects = new List<ShellEffectDTO>();
                if (mainWeapon?.ShellEffects != null)
                {
                    effects.AddRange(mainWeapon.ShellEffects);
                }
                if (secondaryWeapon?.ShellEffects != null)
                {
                    effects.AddRange(secondaryWeapon.ShellEffects);
                }

                int GetShellWeaponEffectValue(ShellWeaponEffectType effectType)
                {
                    return effects?.Where(s => s.Effect == (byte)effectType)?.OrderByDescending(s => s.Value)?.FirstOrDefault()?.Value ?? 0;
                }

                slElementShell = GetShellWeaponEffectValue(ShellWeaponEffectType.SLElement) + GetShellWeaponEffectValue(ShellWeaponEffectType.SLGlobal);
                slHpShell = GetShellWeaponEffectValue(ShellWeaponEffectType.SLHP) + GetShellWeaponEffectValue(ShellWeaponEffectType.SLGlobal);
                slDefenceShell = GetShellWeaponEffectValue(ShellWeaponEffectType.SLDefence) + GetShellWeaponEffectValue(ShellWeaponEffectType.SLGlobal);
                slHitShell = GetShellWeaponEffectValue(ShellWeaponEffectType.SLDamage) + GetShellWeaponEffectValue(ShellWeaponEffectType.SLGlobal);
            }

            int slElement = CharacterHelper.SlPoint(SlElement, 2);
            int slHp = CharacterHelper.SlPoint(SlHP, 3);
            int slDefence = CharacterHelper.SlPoint(SlDefence, 1);
            int slHit = CharacterHelper.SlPoint(SlDamage, 0);

            StringBuilder skills = new StringBuilder();

            List<CharacterSkill> skillsSp = new List<CharacterSkill>();

            var morphUpdate = 31;

            if (ItemVNum == 4485 || ItemVNum == 4437 || ItemVNum == 4416)
            {
                morphUpdate = 30;
            }

            foreach (Skill ski in ServerManager.GetAllSkill().Where(ski => ski.Class == Item.Morph + morphUpdate && ski.LevelMinimum <= SpLevel))
            {
                skillsSp.Add(new CharacterSkill
                {
                    SkillVNum = ski.SkillVNum,
                    CharacterId = CharacterId
                });
            }

            if (Item.ItemType == ItemType.Box)
            {
                // should! Prevent but only for boxinstances. idk if raidboxes are they but i look another packet
                if (Amount < 0)
                {
                    session.SendPacket(UserInterfaceHelper.GenerateMsg($"Im so stupid", 0));
                    Logger.Log.Info($"Cenk's Logging : A accident happened, someone tried to dupe with raidboxes or any boxes");
                }
            }

            byte spdestroyed = 0;

            if (Rare == -2)
            {
                spdestroyed = 1;
            }

            if (skillsSp.Count == 0)
            {
                skills.Append("-1");
            }
            else
            {
                short firstSkillVNum = skillsSp[0].SkillVNum;

                for (int i = 1; i < 11; i++)
                {
                    if (skillsSp.Count >= i + 1 && skillsSp[i].SkillVNum <= firstSkillVNum + 10)
                    {
                        if (skills.Length > 0)
                        {
                            skills.Append(".");
                        }

                        skills.Append($"{skillsSp[i].SkillVNum}");
                    }
                }
            }

            // 10 9 8 '0 0 0 0'<- bonusdamage bonusarmor bonuselement bonushpmp its after upgrade and
            // 3 first values are not important

            return $"slinfo {(Type == InventoryType.Wear || Type == InventoryType.Specialist || Type == InventoryType.Equipment ? "0" : "2")} {ItemVNum} {Item.Morph} {SpLevel} {Item.LevelJobMinimum} {Item.ReputationMinimum} 0 {Item.Speed} 0 0 0 0 0 {Item.SpType} {Item.FireResistance} {Item.WaterResistance} {Item.LightResistance} {Item.DarkResistance} {XP} {CharacterHelper.SPXPData[SpLevel == 0 ? 0 : SpLevel - 1]} {skills} {TransportId} {freepoint} {slHit} {slDefence} {slElement} {slHp} {Upgrade} 0 0 {spdestroyed} {slHitShell} {slDefenceShell} {slElementShell} {slHpShell} {SpStoneUpgrade} {SpDamage} {SpDefence} {SpElement} {SpHP} {SpFire} {SpWater} {SpLight} {SpDark} {(byte)ElementSpType}";
        }

        public string GenerateReqInfo()
        {
            byte type = 0;
            if (BoundCharacterId != null && BoundCharacterId != CharacterId)
            {
                type = 2;
            }
            return $"r_info {ItemVNum} {type} {0}";
        }

        public bool PerfectSP(ClientSession session)
        {
            short[] upsuccess = { 50, 40, 30, 20, 10 };

            int[] goldprice = { 5000, 10000, 20000, 50000, 100000 };
            byte[] stoneprice = { 1, 2, 3, 4, 5 };
            short stonevnum;
            byte upmode = 1;

            switch ((SpecialistMorphType)Item.Morph)
            {
                case SpecialistMorphType.Warrior:
                case SpecialistMorphType.RedMage:
                case SpecialistMorphType.Jajamaru:
                case SpecialistMorphType.BombArtificer:
                case SpecialistMorphType.Drakenfer:
                    stonevnum = 2514;
                    break;

                case SpecialistMorphType.Ninja:
                case SpecialistMorphType.Ranger:
                case SpecialistMorphType.IceMage:
                case SpecialistMorphType.MysticalArts:
                case SpecialistMorphType.NewNinja:
                case SpecialistMorphType.NewRanger:
                case SpecialistMorphType.NewIceMage:
                    stonevnum = 2515;
                    break;

                case SpecialistMorphType.Assassin:
                case SpecialistMorphType.Berserker:
                case SpecialistMorphType.DarkGunner:
                case SpecialistMorphType.DemonWarrior:
                    stonevnum = 2516;
                    break;

                case SpecialistMorphType.Crusader:
                case SpecialistMorphType.WildKeeper:
                case SpecialistMorphType.HolyMage:
                case SpecialistMorphType.WolfMaster:
                    stonevnum = 2517;
                    break;

                case SpecialistMorphType.Gladiator:
                case SpecialistMorphType.Canoneer:
                case SpecialistMorphType.Volcanor:
                    stonevnum = 2518;
                    break;

                case SpecialistMorphType.BlueMonk:
                case SpecialistMorphType.Scout:
                case SpecialistMorphType.TideLord:
                case SpecialistMorphType.Glacerus:
                    stonevnum = 2519;
                    break;

                case SpecialistMorphType.DeathRipper:
                case SpecialistMorphType.DemonHunter:
                case SpecialistMorphType.Seer:
                    stonevnum = 2520;
                    break;

                case SpecialistMorphType.Renegade:
                case SpecialistMorphType.AvengingAngel:
                case SpecialistMorphType.ArchMage:
                    stonevnum = 2521;
                    break;

                case SpecialistMorphType.WaterfallBerserker:
                case SpecialistMorphType.Sunchaser:
                case SpecialistMorphType.VoodoPriest:
                case SpecialistMorphType.FlameDruid:
                    stonevnum = 2624;
                    break;

                default:
                    return false;
            }

            if (SpStoneUpgrade > 99)
            {
                return false;
            }
            if (SpStoneUpgrade > 80)
            {
                upmode = 5;
            }
            else if (SpStoneUpgrade > 60)
            {
                upmode = 4;
            }
            else if (SpStoneUpgrade > 40)
            {
                upmode = 3;
            }
            else if (SpStoneUpgrade > 20)
            {
                upmode = 2;
            }

            if (IsFixed)
            {
                return false;
            }
            if (session.Character.Gold < goldprice[upmode - 1])
            {
                return false;
            }
            if (session.Character.Inventory.CountItem(stonevnum) < stoneprice[upmode - 1])
            {
                return false;
            }

            ItemInstance specialist = session.Character.Inventory.GetItemInstanceById(Id);

            int rnd = ServerManager.RandomNumber();
            if (rnd < upsuccess[upmode - 1])
            {
                byte type = (byte)ServerManager.RandomNumber(0, 16), count = 1;
                if (upmode == 4)
                {
                    count = 2;
                }
                if (upmode == 5)
                {
                    count = (byte)ServerManager.RandomNumber(3, 6);
                }

                session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3005), session.Character.PositionX, session.Character.PositionY);

                if (type < 3)
                {
                    specialist.SpDamage += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ATTACK"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ATTACK"), count), 0));
                }
                else if (type < 6)
                {
                    specialist.SpDefence += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_DEFENSE"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_DEFENSE"), count), 0));
                }
                else if (type < 9)
                {
                    specialist.SpElement += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ELEMENT"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ELEMENT"), count), 0));
                }
                else if (type < 12)
                {
                    specialist.SpHP += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_HPMP"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_HPMP"), count), 0));
                }
                else if (type == 12)
                {
                    specialist.SpFire += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_FIRE"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_FIRE"), count), 0));
                }
                else if (type == 13)
                {
                    specialist.SpWater += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_WATER"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_WATER"), count), 0));
                }
                else if (type == 14)
                {
                    specialist.SpLight += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_LIGHT"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_LIGHT"), count), 0));
                }
                else if (type == 15)
                {
                    specialist.SpDark += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_SHADOW"), count), 12));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_SHADOW"), count), 0));
                }
                specialist.SpStoneUpgrade++;
            }
            else
            {
                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("PERFECTSP_FAILURE"), 11));
                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("PERFECTSP_FAILURE"), 0));
            }
            session.SendPacket(specialist.GenerateInventoryAdd());
            session.Character.Gold -= goldprice[upmode - 1];
            session.SendPacket(session.Character.GenerateGold());
            session.Character.Inventory.RemoveItemAmount(stonevnum, stoneprice[upmode - 1]);
            session.SendPacket("shop_end 1");
            return true;
        }

        public bool UpgradeSp(ClientSession session, UpgradeProtection protect)
        {
            if (Upgrade >= 20) //fixsp kentao +20
            {
                return false;
            }

            int[] goldprice = { 200000, 200000, 200000, 200000, 200000, 500000, 500000, 500000, 500000, 500000, 1000000, 1000000, 1000000, 1000000, 1000000, 1250000, 1500000, 1750000, 2000000, 2250000 };
            byte[] feather = { 3, 5, 8, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 70, 80, 90, 100, 110, 120 };
            byte[] fullmoon = { 1, 3, 5, 7, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40 };
            byte[] soul = { 2, 4, 6, 8, 10, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            byte[] dragongem = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 7, 8, 9, 10 };
            const short featherVnum = 2282;
            const short fullmoonVnum = 1030;
            const short dragongemVnum = 2630;
            const short limitatedFullmoonVnum = 9131;
            const short greenSoulVnum = 2283;
            const short redSoulVnum = 2284;
            const short blueSoulVnum = 2285;
            const short dragonSkinVnum = 2511;
            const short dragonBloodVnum = 2512;
            const short dragonHeartVnum = 2513;
            const short blueScrollVnum = 1363;
            const short redScrollVnum = 1364;
            const short DragonCardProtectionScroll = 9287;
            SpecialistMorphType[] soulSpecialists =
             {
                 SpecialistMorphType.Pajama,
                 SpecialistMorphType.Warrior,
                 SpecialistMorphType.Ninja,
                 SpecialistMorphType.Ranger,
                 SpecialistMorphType.Assassin,
                 SpecialistMorphType.RedMage,
                 SpecialistMorphType.HolyMage,
                 SpecialistMorphType.Chicken,
                 SpecialistMorphType.Jajamaru,
                 SpecialistMorphType.Crusader,
                 SpecialistMorphType.Berserker,
                 SpecialistMorphType.BombArtificer,
                 SpecialistMorphType.WildKeeper,
                 SpecialistMorphType.IceMage,
                 SpecialistMorphType.DarkGunner,
                 SpecialistMorphType.Pirate,
                 SpecialistMorphType.Gladiator,
                 SpecialistMorphType.Canoneer,
                 SpecialistMorphType.Volcanor,
                 SpecialistMorphType.BlueMonk,
                 SpecialistMorphType.Scout,
                 SpecialistMorphType.TideLord,
                 SpecialistMorphType.DeathRipper,
                 SpecialistMorphType.DemonHunter,
                 SpecialistMorphType.Seer,
                 SpecialistMorphType.Renegade,
                 SpecialistMorphType.AvengingAngel,
                 SpecialistMorphType.ArchMage,              
                 SpecialistMorphType.Drakenfer,
                 SpecialistMorphType.MysticalArts,
                 SpecialistMorphType.Wedding,
                 SpecialistMorphType.WolfMaster,
                 SpecialistMorphType.DemonWarrior,
                 SpecialistMorphType.NewNinja,
                 SpecialistMorphType.NewIceMage,
                 SpecialistMorphType.NewRanger,
                 SpecialistMorphType.FlameDruid,
                 SpecialistMorphType.Sunchaser,
                 SpecialistMorphType.WaterfallBerserker,
                 SpecialistMorphType.VoodoPriest,
                 SpecialistMorphType.Fishing,
                 SpecialistMorphType.FishingSkin
             };


            if (!session.HasCurrentMapInstance)
            {
                return false;
            }
            short itemToRemove = 2283;
            if (protect != UpgradeProtection.Event)
            {
                if (session.Character.Inventory.CountItem(fullmoonVnum) < fullmoon[Upgrade] && session.Character.Inventory.CountItem(limitatedFullmoonVnum) < fullmoon[Upgrade])
                {
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(fullmoonVnum).Name, fullmoon[Upgrade])), 10));
                    return false;
                }
                if (session.Character.Inventory.CountItem(featherVnum) < feather[Upgrade])
                {
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(featherVnum).Name, feather[Upgrade])), 10));
                    return false;
                }
                if (session.Character.Gold < goldprice[Upgrade])
                {
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
                    return false;
                }

                if (Upgrade < 5)
                {
                    if (SpLevel > 20) //fixsp kentao +20
                    {
                        if (Item.Morph <= 16)/*(soulSpecialists.Any(s => s == (SpecialistMorphType)Item.Morph))*/ //modif basto
                        {
                            if (session.Character.Inventory.CountItem(greenSoulVnum) < soul[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(greenSoulVnum).Name, soul[Upgrade])), 10));
                                return false;
                            }
                            if (protect == UpgradeProtection.Protected)
                            {
                                if (session.Character.Inventory.CountItem(blueScrollVnum) < 1)
                                {
                                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(blueScrollVnum).Name, 1)), 10));
                                    return false;
                                }
                                session.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                        }
                        else
                        {
                            if (session.Character.Inventory.CountItem(dragonSkinVnum) < soul[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(dragonSkinVnum).Name, soul[Upgrade])), 10));
                                return false;
                            }
                            if (protect == UpgradeProtection.Protected)
                            {
                                if (session.Character.Inventory.CountItem(blueScrollVnum) < 1)
                                {
                                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(blueScrollVnum).Name, 1)), 10));
                                    return false;
                                }
                                session.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                            itemToRemove = dragonSkinVnum;
                        }
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LVL_REQUIRED"), 21), 11));
                        return false;
                    }
                }
                else if (Upgrade < 10)
                {
                    if (SpLevel > 40)
                    {
                        if (Item.Morph <= 16)
                        {
                            if (session.Character.Inventory.CountItem(redSoulVnum) < soul[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(redSoulVnum).Name, soul[Upgrade])), 10));
                                return false;
                            }
                            if (protect == UpgradeProtection.Protected)
                            {
                                if (session.Character.Inventory.CountItem(blueScrollVnum) < 1)
                                {
                                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(blueScrollVnum).Name, 1)), 10));
                                    return false;
                                }
                                session.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                            itemToRemove = redSoulVnum;
                        }
                        else
                        {
                            if (session.Character.Inventory.CountItem(dragonBloodVnum) < soul[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(dragonBloodVnum).Name, soul[Upgrade])), 10));
                                return false;
                            }
                            if (protect == UpgradeProtection.Protected)
                            {
                                if (session.Character.Inventory.CountItem(blueScrollVnum) < 1)
                                {
                                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(blueScrollVnum).Name, 1)), 10));
                                    return false;
                                }
                                session.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                            itemToRemove = dragonBloodVnum;
                        }
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LVL_REQUIRED"), 41), 11));
                        return false;
                    }
                }
                else if (Upgrade < 15)
                {
                    if (SpLevel > 50)
                    {
                        if (Item.Morph <= 16)
                        {
                            if (session.Character.Inventory.CountItem(blueSoulVnum) < soul[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(blueSoulVnum).Name, soul[Upgrade])), 10));
                                return false;
                            }
                            if (protect == UpgradeProtection.Protected)
                            {
                                if (session.Character.Inventory.CountItem(redScrollVnum) < 1)
                                {
                                    return false;
                                }
                                session.Character.Inventory.RemoveItemAmount(redScrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                            itemToRemove = blueSoulVnum;
                        }
                        else
                        {
                            if (session.Character.Inventory.CountItem(dragonHeartVnum) < soul[Upgrade])
                            {
                                return false;
                            }
                            if (protect == UpgradeProtection.Protected)
                            {
                                if (session.Character.Inventory.CountItem(redScrollVnum) < 1)
                                {
                                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(redScrollVnum).Name, 1)), 10));
                                    return false;
                                }
                                session.Character.Inventory.RemoveItemAmount(redScrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                            itemToRemove = dragonHeartVnum;
                        }
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LVL_REQUIRED"), 51), 11));
                        return false;
                    }
                }
                else if (Upgrade > 16)
                {
                    if (SpLevel > 61)
                    {
                        /*if (Item.Morph <= 16)*/

                        if (session.Character.Inventory.CountItem(dragongemVnum) < soul[Upgrade])
                        {
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(dragongemVnum).Name, soul[Upgrade])), 10));
                            return false;
                        }
                        if (protect == UpgradeProtection.Protected)
                        {
                            if (session.Character.Inventory.CountItem(DragonCardProtectionScroll) < 1) //DragonCardProtectionScroll
                            {
                                return false;
                            }
                            session.Character.Inventory.RemoveItemAmount(DragonCardProtectionScroll);
                            session.SendPacket("shop_end 2");
                        }
                        itemToRemove = dragongemVnum;

                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LVL_REQUIRED"), 51), 11));
                        return false;
                    }
                }
                session.Character.Gold -= goldprice[Upgrade];
                // remove feather and fullmoon before upgrading
                session.Character.Inventory.RemoveItemAmount(featherVnum, feather[Upgrade]);
                if (session.Character.Inventory.CountItem(limitatedFullmoonVnum) >= fullmoon[Upgrade])
                {
                    session.Character.Inventory.RemoveItemAmount(limitatedFullmoonVnum, fullmoon[Upgrade]);
                }
                else
                {
                    session.Character.Inventory.RemoveItemAmount(fullmoonVnum, fullmoon[Upgrade]);
                }
            }
            else
            {
                session.SendPacket("shop_end 2");
                itemToRemove = -1;
                short eventScrollVnum = -1;
                switch (ItemVNum)
                {
                    case 900: // Pyjama
                        eventScrollVnum = 5207;
                        break;
                    case 907: // Chicken
                        eventScrollVnum = 5107;
                        break;
                    case 4099: // Pirate
                        eventScrollVnum = 5519;
                        break;
                }
                if (eventScrollVnum < 0)
                {
                    return false;
                }
                if (session.Character.Inventory.CountItem(eventScrollVnum) > 0)
                {
                    session.Character.Inventory.RemoveItemAmount(eventScrollVnum);
                }
                else
                {
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(eventScrollVnum).Name, 1)), 10));
                    return false;
                }
            }

            ItemInstance wearable = session.Character.Inventory.GetItemInstanceById(Id);
            ItemInstance inventory = session.Character.Inventory.GetItemInstanceById(Id);
            int rnd = ServerManager.RandomNumber();
            if (rnd < ItemHelper.SpDestroyRate[Upgrade])
            {
                if (protect == UpgradeProtection.Protected || protect == UpgradeProtection.Event)
                {
                    session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3004), session.Character.PositionX, session.Character.PositionY);
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED_SAVED"), 11));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED_SAVED"), 0));
                }
                else
                {
                    session.Character.Inventory.RemoveItemAmount(itemToRemove, soul[Upgrade]);
                    wearable.Rare = -2;
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_DESTROYED"), 11));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_DESTROYED"), 0));
                    session.SendPacket(wearable.GenerateInventoryAdd());
                }
            }
            else if (rnd < ItemHelper.SpUpFailRate[Upgrade])
            {
                if (protect == UpgradeProtection.Protected || protect == UpgradeProtection.Event)
                {
                    session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3004), session.Character.PositionX, session.Character.PositionY);
                }
                else
                {
                    session.Character.Inventory.RemoveItemAmount(itemToRemove, soul[Upgrade]);
                }
                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED"), 11));
                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED"), 0));
            }
            else
            {
                if (protect == UpgradeProtection.Protected || protect == UpgradeProtection.Event)
                {
                    session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3004), session.Character.PositionX, session.Character.PositionY);
                }
                session.Character.Inventory.RemoveItemAmount(itemToRemove, soul[Upgrade]);
                session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3005), session.Character.PositionX, session.Character.PositionY);
                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_SUCCESS"), 12));
                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_SUCCESS"), 0));
                wearable.Upgrade++;
                if (wearable.Upgrade > 8)
                {
                    session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name, itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                }
                session.SendPacket(wearable.GenerateInventoryAdd());
            }
            session.SendPacket(session.Character.GenerateGold());
            session.SendPacket(session.Character.GenerateEq());
            session.SendPacket("shop_end 1");
            return true;    
        }

        public string GenerateEInfo()
        {
            EquipmentType equipmentslot = Item.EquipmentSlot;
            ItemType itemType = Item.ItemType;
            byte itemClass = Item.Class;
            byte subtype = Item.ItemSubType;
            DateTime test = ItemDeleteTime ?? DateTime.Now;
            long time = ItemDeleteTime != null ? (long)test.Subtract(DateTime.Now).TotalSeconds : 0;
            long seconds = IsBound ? time : Item.ItemValidTime;
            if (seconds < 0)
            {
                seconds = 0;
            }
            var rune =
               $"{RuneAmount} {(IsBreaked ? "1" : "0")} {RuneEffects.Count()} " +
               $"{RuneEffects.Where(x => x.Type != BCardType.CardType.A7Powers1 && x.Type != BCardType.CardType.A7Powers2).Aggregate("", (result, effect) => result += $"{(byte)effect.Type}.{(byte)effect.SubType}.{effect.FirstData * 4}.{effect.SecondData * 4}.{effect.ThirdData} ")} " +
               $"{RuneEffects.Where(x => x.Type == BCardType.CardType.A7Powers1 || x.Type == BCardType.CardType.A7Powers2).Aggregate("", (result, effect) => result += $"{(byte)effect.Type}.{(byte)effect.SubType}.{effect.FirstData * 4}.{effect.SecondData * 4}.{effect.ThirdData} ")}";
            switch (itemType)
            {
                case ItemType.Weapon:
                    switch (equipmentslot)
                    {
                        case EquipmentType.MainWeapon:
                            return
                                $"e_info {(itemClass == 4 ? 1 : itemClass == 8 ? 5 : 0)} {ItemVNum} {Rare} {Upgrade} {(IsFixed ? 1 : 0)} {Item.LevelMinimum} {Item.DamageMinimum + DamageMinimum} {Item.DamageMaximum + DamageMaximum} {Item.HitRate + HitRate} {Item.CriticalLuckRate + CriticalLuckRate} {Item.CriticalRate + CriticalRate} {Ammo} {Item.MaximumAmmo} {Item.SellToNpcPrice} {(IsPartnerEquipment ? $"{HoldingVNum}" : "-1")} {(ShellRarity == null ? "0" : $"{ShellRarity}")} {BoundCharacterId ?? 0} {ShellEffects.Count} {ShellEffects.Aggregate("", (result, effect) => result += $"{(byte)effect.EffectLevel}.{effect.Effect}.{(byte)effect.Value} ")} {rune}";

                        case EquipmentType.SecondaryWeapon:
                            return $"e_info {(itemClass <= 2 ? 1 : 0)} {ItemVNum} {Rare} {Upgrade} {(IsFixed ? 1 : 0)} {Item.LevelMinimum} {Item.DamageMinimum + DamageMinimum} {Item.DamageMaximum + DamageMaximum} {Item.HitRate + HitRate} {Item.CriticalLuckRate + CriticalLuckRate} {Item.CriticalRate + CriticalRate} {Ammo} {Item.MaximumAmmo} {Item.SellToNpcPrice} {(IsPartnerEquipment ? $"{HoldingVNum}" : "-1")} {(ShellRarity == null ? "0" : $"{ShellRarity}")} {BoundCharacterId ?? 0} {ShellEffects.Count} {ShellEffects.Aggregate("", (result, effect) => result += $"{(byte)effect.EffectLevel}.{effect.Effect}.{(byte)effect.Value} ")}";
                    }
                    break;

                case ItemType.Armor:
                    return $"e_info 2 {ItemVNum} {Rare} {Upgrade} {(IsFixed ? 1 : 0)} {Item.LevelMinimum} {Item.CloseDefence + CloseDefence} {Item.DistanceDefence + DistanceDefence} {Item.MagicDefence + MagicDefence} {Item.DefenceDodge + DefenceDodge} {Item.SellToNpcPrice} {(IsPartnerEquipment ? $"{HoldingVNum}" : "-1")} {(ShellRarity == null ? "0" : $"{ShellRarity}")} {BoundCharacterId ?? 0} {ShellEffects.Count} {ShellEffects.Aggregate("", (result, effect) => result += $"{((byte)effect.EffectLevel > 12 ? (byte)effect.EffectLevel - 12 : (byte)effect.EffectLevel)}.{(effect.Effect > 50 ? effect.Effect - 50 : effect.Effect)}.{(byte)effect.Value} ")}";

                case ItemType.Fashion:
                    switch (equipmentslot)
                    {
                        case EquipmentType.CostumeHat:
                            return $"e_info 3 {ItemVNum} {Item.LevelMinimum} {Item.CloseDefence + CloseDefence} {Item.DistanceDefence + DistanceDefence} {Item.MagicDefence + MagicDefence} {Item.DefenceDodge + DefenceDodge} {Item.FireResistance + FireResistance} {Item.WaterResistance + WaterResistance} {Item.LightResistance + LightResistance} {Item.DarkResistance + DarkResistance} {Item.SellToNpcPrice} {(Item.ItemValidTime == 0 ? -1 : 0)} 2 {(Item.ItemValidTime == 0 ? -1 : seconds / 3600)}";

                        case EquipmentType.CostumeSuit:
                            return $"e_info 2 {ItemVNum} {Rare} {Upgrade} {(IsFixed ? 1 : 0)} {Item.LevelMinimum} {Item.CloseDefence + CloseDefence} {Item.DistanceDefence + DistanceDefence} {Item.MagicDefence + MagicDefence} {Item.DefenceDodge + DefenceDodge} {Item.SellToNpcPrice} {(Item.ItemValidTime == 0 ? -1 : 0)} 1 {(Item.ItemValidTime == 0 ? -1 : seconds / 3600)}"; // 1 = IsCosmetic -1 = no shells

                        default:
                            return $"e_info 3 {ItemVNum} {Item.LevelMinimum} {Item.CloseDefence + CloseDefence} {Item.DistanceDefence + DistanceDefence} {Item.MagicDefence + MagicDefence} {Item.DefenceDodge + DefenceDodge} {Item.FireResistance + FireResistance} {Item.WaterResistance + WaterResistance} {Item.LightResistance + LightResistance} {Item.DarkResistance + DarkResistance} {Item.SellToNpcPrice} {Upgrade} 0 -1"; // after Item.Price theres TimesConnected {(Item.ItemValidTime == 0 ? -1 : Item.ItemValidTime / (3600))}
                    }

                case ItemType.Jewelery:
                    switch (equipmentslot)
                    {
                        case EquipmentType.Amulet:
                            if (DurabilityPoint > 0)
                            {
                                return $"e_info 4 {ItemVNum} {Item.LevelMinimum} {DurabilityPoint} 100 0 {Item.SellToNpcPrice}";
                            }
                            return $"e_info 4 {ItemVNum} {Item.LevelMinimum} {seconds * 10} 0 0 {Item.SellToNpcPrice}";

                        case EquipmentType.Fairy:
                            return $"e_info 4 {ItemVNum} {Item.Element} {ElementRate + Item.ElementRate} 0 0 0 0 0"; // last IsNosmall

                        default:
                            string cellon = "";
                            foreach (CellonOptionDTO option in CellonOptions)
                            {
                                cellon += $" {(byte)option.Type} {option.Level} {option.Value}";
                            }
                            return $"e_info 4 {ItemVNum} {Item.LevelMinimum} {Item.MaxCellonLvl} {Item.MaxCellon} {CellonOptions.Count} {Item.SellToNpcPrice}{cellon}";
                    }
                case ItemType.Specialist:
                    return $"e_info 8 {ItemVNum}";

                //Fix PKH
                case ItemType.Box:
                    switch (subtype)
                    {
                        case 0:
                        case 1:
                            return HoldingVNum == 0 ?
                                $"e_info 7 {ItemVNum} 0" : $"e_info 7 {ItemVNum} 1 {HoldingVNum} {SpLevel} {XP} 100 {SpDamage} {SpDefence}";

                        case 2:
                            Item spitem = ServerManager.GetItem(HoldingVNum);
                            return HoldingVNum == 0 ?
                                $"e_info 7 {ItemVNum} 0" :
                                $"e_info 7 {ItemVNum} 1 {HoldingVNum} {SpLevel} {XP} {CharacterHelper.SPXPData[SpLevel == 0 ? 0 : SpLevel - 1]} {Upgrade} {CharacterHelper.SlPoint(SlDamage, 0)} {CharacterHelper.SlPoint(SlDefence, 1)} {CharacterHelper.SlPoint(SlElement, 2)} {CharacterHelper.SlPoint(SlHP, 3)} {CharacterHelper.SPPoint(SpLevel, Upgrade) - SlDamage - SlHP - SlElement - SlDefence} {SpStoneUpgrade} {spitem.FireResistance} {spitem.WaterResistance} {spitem.LightResistance} {spitem.DarkResistance} {SpDamage} {SpDefence} {SpElement} {SpHP} {SpFire} {SpWater} {SpLight} {SpDark}";

                        case 4:
                            return HoldingVNum == 0 ?
                                $"e_info 11 {ItemVNum} 0" :
                                $"e_info 11 {ItemVNum} 1 {HoldingVNum}";

                        case 5:
                            Item fairyitem = ServerManager.GetItem(HoldingVNum);
                            return HoldingVNum == 0 ?
                                $"e_info 12 {ItemVNum} 0" :
                                $"e_info 12 {ItemVNum} 1 {HoldingVNum} {ElementRate + fairyitem.ElementRate}";

                        case 6:
                            var packet = string.Empty;
                            foreach (var skill in DAOFactory.PartnerSkillDAO.LoadByEquipmentSerialId(EquipmentSerialId).ToList())
                            {
                                packet += $"{skill.SkillVNum} {skill.Level} ";
                            }
                            var data = packet.Split();
                            var output = (
                                data.Length == 1 ? "0 0 0 0 0 0" :
                                data.Length == 3 ? $"{packet}0 0 0 0" :
                                data.Length == 5 ? $"{packet}0 0" : $"{packet}");
                            return HoldingVNum == 0 ?
                                $"e_info 13 {ItemVNum} 0" :
                                $"e_info 13 {ItemVNum} 1 {HoldingVNum} 1 {output}";

                        default:
                            return $"e_info 8 {ItemVNum} {Design} {Rare}";
                    }

                case ItemType.Shell:
                    return $"e_info 9 {ItemVNum} {Upgrade} {Rare} {Item.SellToNpcPrice} {ShellEffects.Count}{ShellEffects.Aggregate("", (current, option) => current + $" {((byte)option.EffectLevel > 12 ? (byte)option.EffectLevel - 12 : (byte)option.EffectLevel)}.{(option.Effect > 50 ? option.Effect - 50 : option.Effect)}.{option.Value}")}";
            }
            return "";
        }

        public void OptionItem(ClientSession session, short cellonVNum)
        {
            if (EquipmentSerialId == Guid.Empty)
            {
                EquipmentSerialId = Guid.NewGuid();
            }
            if (Item.MaxCellon <= CellonOptions.Count)
            {
                session.SendPacket($"info {Language.Instance.GetMessageFromKey("MAX_OPTIONS")}");
                session.SendPacket("shop_end 1");
                return;
            }
            if (session.Character.Inventory.CountItem(cellonVNum) > 0)
            {
                byte dataIndex = 0;
                int goldAmount = 0;
                switch (cellonVNum)
                {
                    case 1017:
                        dataIndex = 0;
                        goldAmount = 700;
                        break;

                    case 1018:
                        dataIndex = 1;
                        goldAmount = 1400;
                        break;

                    case 1019:
                        dataIndex = 2;
                        goldAmount = 3000;
                        break;

                    case 1020:
                        dataIndex = 3;
                        goldAmount = 5000;
                        break;

                    case 1021:
                        dataIndex = 4;
                        goldAmount = 10000;
                        break;

                    case 1022:
                        dataIndex = 5;
                        goldAmount = 20000;
                        break;

                    case 1023:
                        dataIndex = 6;
                        goldAmount = 32000;
                        break;

                    case 1024:
                        dataIndex = 7;
                        goldAmount = 58000;
                        break;

                    case 1025:
                        dataIndex = 8;
                        goldAmount = 95000;
                        break;

                    case 1026:
                        
                        dataIndex = 9;
                        goldAmount = 95000;
                        break;
                }

                if (Item.MaxCellonLvl > dataIndex && session.Character.Gold >= goldAmount)
                {
                    short[][] minimumData = {
                   new short[] { 30, 50, 5, 8, 0, 0 },             //lv1
                    new short[] { 120, 150, 14, 16, 0, 0 },         //lv2
                    new short[] { 220, 280, 22, 28, 0, 0 },         //lv3
                    new short[] { 330, 350, 30, 38, 0, 0 },         //lv4
                    new short[] { 430, 450, 40, 50, 0, 0 },         //lv5
                    new short[] { 600, 600, 55, 65, 1, 1 },         //lv6
                    new short[] { 800, 800, 75, 75, 8, 11 },        //lv7
                    new short[] { 1000, 1000, 100, 100, 13, 21 },   //lv8
                    new short[] { 1100, 1100, 110, 110, 14, 22 },   //lv9
                    new short[] { 1300, 1300, 155, 155, 19, 29 }    //lv10
                    };
                    short[][] maximumData = {
                    new short[] { 100, 150, 10, 15, 0, 0 },         //lv1
                    new short[] { 200, 250, 20, 25, 0, 0 },         //lv1
                    new short[] { 300, 330, 28, 35, 0, 0 },         //lv1
                    new short[] { 400, 420, 38, 45, 0, 0 },         //lv1
                    new short[] { 550, 550, 50, 60, 0, 0 },         //lv1
                    new short[] { 750, 750, 70, 80, 7, 10 },        //lv1
                    new short[] { 1000, 1000, 90,90, 12, 20 },      //lv1
                    new short[] { 1300, 1300, 120, 120, 17, 35 },   //lv1
                    new short[] { 1500, 1500, 135, 135, 21, 45 },   //lv1
                    new short[] { 2059, 2100, 199, 199, 28, 58 }    //lv10
                    };

                    short[] generateOption()
                    {
                        byte option = 0;
                        if (dataIndex < 5)
                        {
                            option = (byte)ServerManager.RandomNumber(0, 4);
                        }
                        else
                        {
                            option = (byte)ServerManager.RandomNumber(0, 6);
                        }

                        if (CellonOptions.Any(s => s.Type == (CellonOptionType)option))
                        {
                            return new short[] { -1, -1 };
                        }

                        return new short[] { option, (short)ServerManager.RandomNumber(minimumData[dataIndex][option], maximumData[dataIndex][option] + 1) };
                    }

                    short[] value = generateOption();
                    Logger.LogUserEvent("OPTION", session.GenerateIdentity(), $"[OptionItem]Serial: {EquipmentSerialId} Type: {value[0]} Value: {value[1]}");
                    if (value[0] != -1)
                    {
                        CellonOptionDTO cellonOptionDTO = new CellonOptionDTO
                        {
                            EquipmentSerialId = EquipmentSerialId,
                            Level = (byte)(dataIndex + 1),
                            Type = (CellonOptionType)value[0],
                            Value = value[1]
                        };

                        CellonOptions.Add(cellonOptionDTO);

                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("OPTION_SUCCESS"), Rare), 12));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("OPTION_SUCCESS"), Rare), 0));
                        session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, CharacterId, 3005), session.Character.PositionX, session.Character.PositionY);
                        session.SendPacket("shop_end 1");
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("OPTION_FAIL"), Rare), 11));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("OPTION_FAIL"), Rare), 0));
                        session.SendPacket("shop_end 1");
                    }
                    session.Character.Inventory.RemoveItemAmount(cellonVNum);
                    session.Character.Gold -= goldAmount;
                    session.SendPacket(session.Character.GenerateGold());
                }
            }

            foreach (CellonOptionDTO effect in CellonOptions)
            {
                effect.EquipmentSerialId = EquipmentSerialId;
                effect.CellonOptionId = DAOFactory.CellonOptionDAO.InsertOrUpdate(effect).CellonOptionId;
            }
        }

        public void GenerateHeroicShell(RarifyProtection protection, bool forced = false)
        {
            if (protection != RarifyProtection.RandomHeroicAmulet && !forced)
            {
                return;
            }
            if (!Item.IsHeroic || Rare <= 0)
            {
                return;
            }
            byte shellType = (byte)(Item.ItemType == ItemType.Armor ? 11 : 10);
            if (shellType != 11 && shellType != 10)
            {
                return;
            }
            ShellEffects.Clear();
            int shellLevel = Item.LevelMinimum == 25 ? 101 : 106;
            ShellEffects.AddRange(ShellGeneratorHelper.Instance.GenerateShell(shellType, Rare == 8 ? 7 : Rare, shellLevel));
        }

        public void RarifyItem(ClientSession session, RarifyMode mode, RarifyProtection protection, bool isCommand = false, byte forceRare = 0)
        {
            const short goldprice = 500;
            const double reducedpricefactor = 0.5;
            const double reducedchancefactor = 1.1;
            const byte cella = 5;
            const int cellaVnum = 1014;
            const int scrollVnum = 1218;
            double rnd;
            if (ServerManager.Instance.Configuration.dobleapuestaUp == true)
            {
                byte[] rarifyRate = new byte[ServerManager.Instance.RateItem().RarifyRateEvent.Length];
                ServerManager.Instance.RateItem().RarifyRateEvent.CopyTo(rarifyRate, 0);

                if (session?.HasCurrentMapInstance == false)
                {
                    return;
                }
                if (mode != RarifyMode.Drop || Item.ItemType == ItemType.Shell)
                {
                    rarifyRate[0] = 0;
                    rarifyRate[1] = 0;
                    rarifyRate[2] = 0;
                    rnd = ServerManager.RandomNumber(0, 80);
                }
                else
                {
                    rnd = ServerManager.RandomNumber(0, 1000) / 10D;
                }
                if (protection == RarifyProtection.RedAmulet ||
                    protection == RarifyProtection.HeroicAmulet ||
                    protection == RarifyProtection.RandomHeroicAmulet)
                {
                    for (byte i = 0; i < rarifyRate.Length; i++)
                    {
                        rarifyRate[i] = (byte)(rarifyRate[i] * reducedchancefactor);
                    }
                }
                if (session != null)
                {
                    switch (mode)
                    {
                        case RarifyMode.Free:
                            break;

                        case RarifyMode.Success:
                            if (Item.IsHeroic && Rare >= 8 || !Item.IsHeroic && Rare <= 7)
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("ALREADY_MAX_RARE"), 10));
                                return;
                            }
                            Rare += 1;
                            SetRarityPoint();
                            ItemInstance inventory = session?.Character.Inventory.GetItemInstanceById(Id);
                            if (inventory != null)
                            {
                                session.SendPacket(inventory.GenerateInventoryAdd());
                            }
                            return;

                        case RarifyMode.Reduced:
                            if (session.Character.Gold < (long)(goldprice * reducedpricefactor))
                            {
                                return;
                            }
                            if (session.Character.Inventory.CountItem(cellaVnum) < cella * reducedpricefactor)
                            {
                                return;
                            }
                            session.Character.Inventory.RemoveItemAmount(cellaVnum, (int)(cella * reducedpricefactor));
                            session.Character.Gold -= (long)(goldprice * reducedpricefactor);
                            session.SendPacket(session.Character.GenerateGold());
                            break;

                        case RarifyMode.Normal:
                            if (session.Character.Gold < goldprice)
                            {
                                return;
                            }
                            if (session.Character.Inventory.CountItem(cellaVnum) < cella)
                            {
                                return;
                            }
                            if (protection == RarifyProtection.Scroll && !isCommand
                                && session.Character.Inventory.CountItem(scrollVnum) < 1)
                            {
                                return;
                            }

                            if ((protection == RarifyProtection.Scroll || protection == RarifyProtection.BlueAmulet || protection == RarifyProtection.RedAmulet) && !isCommand && Item.IsHeroic)
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_IS_HEROIC"), 0));
                                return;
                            }
                            if ((protection == RarifyProtection.HeroicAmulet ||
                                 protection == RarifyProtection.RandomHeroicAmulet) && !Item.IsHeroic)
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_NOT_HEROIC"), 0));
                                return;
                            }
                            if (Item.IsHeroic && Rare == 10)
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ALREADY_MAX_RARE"), 0));
                                return;
                            }

                            if (protection == RarifyProtection.Scroll && !isCommand)
                            {
                                session.Character.Inventory.RemoveItemAmount(scrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                            session.Character.Gold -= goldprice;
                            session.Character.Inventory.RemoveItemAmount(cellaVnum, cella);
                            session.SendPacket(session.Character.GenerateGold());
                            break;

                        case RarifyMode.Drop:
                            break;

                        case RarifyMode.HeroEquipmentDowngrade:
                            {
                                rarify(9, true);
                                return;
                            }

                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                    }
                }

                void rarify(sbyte rarity, bool isHeroEquipmentDowngrade = false)
                {
                    Rare = rarity;
                    if (mode != RarifyMode.Drop)
                    {
                        Logger.LogUserEvent("GAMBLE", session.GenerateIdentity(), $"[RarifyItem]Protection: {protection.ToString()} IIId: {Id} ItemVnum: {ItemVNum} Result: Success");

                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey(isHeroEquipmentDowngrade ? "RARIFY_DOWNGRADE_SUCCESS" : "RARIFY_SUCCESS"), Rare), 12));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey(isHeroEquipmentDowngrade ? "RARIFY_DOWNGRADE_SUCCESS" : "RARIFY_SUCCESS"), Rare), 0));
                        session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, CharacterId, 3005), session.Character.PositionX, session.Character.PositionY);
                        session.SendPacket("shop_end 1");
                    }
                    SetRarityPoint();

                    if (!isHeroEquipmentDowngrade)
                    {
                        GenerateHeroicShell(protection);
                    }
                }

                if (forceRare != 0)
                {
                    rarify((sbyte)forceRare);
                    return;
                }
                if (Item.IsHeroic && protection != RarifyProtection.None)
                {
                    if (rnd < rarifyRate[12])
                    {
                        rarify(10);
                        if (mode != RarifyMode.Drop && session != null)
                        {
                            ItemInstance inventory = session.Character.Inventory.GetItemInstanceById(Id);
                            if (inventory != null)
                            {
                                session.SendPacket(inventory.GenerateInventoryAdd());
                            }
                        }
                        return;
                    }
                }
                if (rnd < rarifyRate[12] && !(protection == RarifyProtection.Scroll && Rare >= 10))
                {
                    rarify(10);
                }
                if (rnd < rarifyRate[11] && !(protection == RarifyProtection.Scroll && Rare >= 9))
                {
                    rarify(9);
                }
                if (rnd < rarifyRate[10] && !(protection == RarifyProtection.Scroll && Rare >= 8))
                {
                    rarify(8);
                }
                if (rnd < rarifyRate[9] && !(protection == RarifyProtection.Scroll && Rare >= 7))
                {
                    rarify(7);
                }
                else if (rnd < rarifyRate[8] && !(protection == RarifyProtection.Scroll && Rare >= 6))
                {
                    rarify(6);
                }
                else if (rnd < rarifyRate[7] && !(protection == RarifyProtection.Scroll && Rare >= 5))
                {
                    rarify(5);
                }
                else if (rnd < rarifyRate[6] && !(protection == RarifyProtection.Scroll && Rare >= 4))
                {
                    rarify(4);
                }
                else if (rnd < rarifyRate[5] && !(protection == RarifyProtection.Scroll && Rare >= 3))
                {
                    rarify(3);
                }
                else if (rnd < rarifyRate[4] && !(protection == RarifyProtection.Scroll && Rare >= 2))
                {
                    rarify(2);
                }
                else if (rnd < rarifyRate[3] && !(protection == RarifyProtection.Scroll && Rare >= 1))
                {
                    rarify(1);
                }
                else if (rnd < rarifyRate[2] && !(protection == RarifyProtection.Scroll && Rare >= 0))
                {
                    rarify(0);
                }
                else if (rnd < rarifyRate[1] && !(protection == RarifyProtection.Scroll && Rare >= -1))
                {
                    rarify(-1);
                }
                else if (rnd < rarifyRate[0] && !(protection == RarifyProtection.Scroll && Rare >= -2))
                {
                    rarify(-2);
                }
                else if (Rare < 1 && Item.ItemType == ItemType.Shell)
                {
                    Rare = 1;
                }
                else if (mode != RarifyMode.Drop && session != null)
                {
                    switch (protection)
                    {
                        case RarifyProtection.BlueAmulet:
                        case RarifyProtection.RedAmulet:
                        case RarifyProtection.HeroicAmulet:
                        case RarifyProtection.RandomHeroicAmulet:
                            session.Character.RemoveBuff(62);
                            ItemInstance amulet = session.Character.Inventory.LoadBySlotAndType((short)EquipmentType.Amulet, InventoryType.Wear);
                            amulet.DurabilityPoint -= 1;
                            if (amulet.DurabilityPoint <= 0)
                            {
                                session.Character.DeleteItemByItemInstanceId(amulet.Id);
                                session.SendPacket($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                                session.SendPacket(session.Character.GenerateEquipment());
                            }
                            else
                            {
                                session.Character.AddBuff(new Buff(62, session.Character.Level), session.Character.BattleEntity);
                            }
                            break;
                        case RarifyProtection.None:
                            session.Character.DeleteItemByItemInstanceId(Id);
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RARIFY_FAILED"), 11));
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("RARIFY_FAILED"), 0));
                            return;
                    }
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RARIFY_FAILED_ITEM_SAVED"), 11));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("RARIFY_FAILED_ITEM_SAVED"), 0));
                    session.CurrentMapInstance.Broadcast(session.Character.GenerateEff(3004), session.Character.PositionX, session.Character.PositionY);
                    return;
                }
                if (mode != RarifyMode.Drop && session != null)
                {
                    ItemInstance inventory = session.Character.Inventory.GetItemInstanceById(Id);
                    if (inventory != null)
                    {
                        session.SendPacket(inventory.GenerateInventoryAdd());
                    }
                }
            }
            else
            {
                byte[] rarifyRate = new byte[ServerManager.Instance.RateItem().RarifyRate.Length];
                ServerManager.Instance.RateItem().RarifyRate.CopyTo(rarifyRate, 0);
                if (session?.HasCurrentMapInstance == false)
                {
                    return;
                }
                if (mode != RarifyMode.Drop || Item.ItemType == ItemType.Shell)
                {
                    rarifyRate[0] = 0;
                    rarifyRate[1] = 0;
                    rarifyRate[2] = 0;
                    rnd = ServerManager.RandomNumber(0, 80);
                }
                else
                {
                    rnd = ServerManager.RandomNumber(0, 1000) / 10D;
                }
                if (protection == RarifyProtection.RedAmulet ||
                    protection == RarifyProtection.HeroicAmulet ||
                    protection == RarifyProtection.RandomHeroicAmulet)
                {
                    for (byte i = 0; i < rarifyRate.Length; i++)
                    {
                        rarifyRate[i] = (byte)(rarifyRate[i] * reducedchancefactor);
                    }
                }
                if (session != null)
                {
                    switch (mode)
                    {
                        case RarifyMode.Free:
                            break;

                        case RarifyMode.Success:
                            if (Item.IsHeroic && Rare >= 10 || !Item.IsHeroic && Rare <= 9)
                            {
                                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("ALREADY_MAX_RARE"), 10));
                                return;
                            }
                            Rare += 1;
                            SetRarityPoint();
                            ItemInstance inventory = session?.Character.Inventory.GetItemInstanceById(Id);
                            if (inventory != null)
                            {
                                session.SendPacket(inventory.GenerateInventoryAdd());
                            }
                            return;

                        case RarifyMode.Reduced:
                            if (session.Character.Gold < (long)(goldprice * reducedpricefactor))
                            {
                                return;
                            }
                            if (session.Character.Inventory.CountItem(cellaVnum) < cella * reducedpricefactor)
                            {
                                return;
                            }
                            session.Character.Inventory.RemoveItemAmount(cellaVnum, (int)(cella * reducedpricefactor));
                            session.Character.Gold -= (long)(goldprice * reducedpricefactor);
                            session.SendPacket(session.Character.GenerateGold());
                            break;

                        case RarifyMode.Normal:
                            if (session.Character.Gold < goldprice)
                            {
                                return;
                            }
                            if (session.Character.Inventory.CountItem(cellaVnum) < cella)
                            {
                                return;
                            }
                            if (protection == RarifyProtection.Scroll && !isCommand
                                && session.Character.Inventory.CountItem(scrollVnum) < 1)
                            {
                                return;
                            }

                            if ((protection == RarifyProtection.Scroll || protection == RarifyProtection.BlueAmulet || protection == RarifyProtection.RedAmulet) && !isCommand && Item.IsHeroic)
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_IS_HEROIC"), 0));
                                return;
                            }
                            if ((protection == RarifyProtection.HeroicAmulet ||
                                 protection == RarifyProtection.RandomHeroicAmulet) && !Item.IsHeroic)
                            {
                                session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("ITEM_NOT_HEROIC"), 0));
                                return;
                            }
                            if (Item.IsHeroic && Rare == 10)
                            {
                                rarify(9, true);
                                ItemInstance inv = session?.Character.Inventory.GetItemInstanceById(Id);
                                if (inv != null)
                                {
                                    session.SendPacket(inv.GenerateInventoryAdd());
                                }
                                return;
                            }

                            if (protection == RarifyProtection.Scroll && !isCommand)
                            {
                                session.Character.Inventory.RemoveItemAmount(scrollVnum);
                                session.SendPacket("shop_end 2");
                            }
                            session.Character.Gold -= goldprice;
                            session.Character.Inventory.RemoveItemAmount(cellaVnum, cella);
                            session.SendPacket(session.Character.GenerateGold());
                            break;

                        case RarifyMode.Drop:
                            break;

                        case RarifyMode.HeroEquipmentDowngrade:
                            {
                                rarify(7, true);
                                return;
                            }

                        default:
                            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                    }
                }

                void rarify(sbyte rarity, bool isHeroEquipmentDowngrade = false)
                {
                    Rare = rarity;
                    if (mode != RarifyMode.Drop)
                    {
                        Logger.LogUserEvent("GAMBLE", session.GenerateIdentity(), $"[RarifyItem]Protection: {protection.ToString()} IIId: {Id} ItemVnum: {ItemVNum} Result: Success");

                        session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey(isHeroEquipmentDowngrade ? "RARIFY_DOWNGRADE_SUCCESS" : "RARIFY_SUCCESS"), Rare), 12));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey(isHeroEquipmentDowngrade ? "RARIFY_DOWNGRADE_SUCCESS" : "RARIFY_SUCCESS"), Rare), 0));
                        session.CurrentMapInstance?.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, CharacterId, 3005), session.Character.PositionX, session.Character.PositionY);
                        session.SendPacket("shop_end 1");
                    }
                    SetRarityPoint();

                    if (!isHeroEquipmentDowngrade)
                    {
                        GenerateHeroicShell(protection);
                    }
                }

                if (forceRare != 0)
                {
                    rarify((sbyte)forceRare);
                    return;
                }
                if (Item.IsHeroic && protection != RarifyProtection.None)
                {
                    if (rnd < rarifyRate[10])
                    {
                        rarify(8);
                        if (mode != RarifyMode.Drop && session != null)
                        {
                            ItemInstance inventory = session.Character.Inventory.GetItemInstanceById(Id);
                            if (inventory != null)
                            {
                                session.SendPacket(inventory.GenerateInventoryAdd());
                            }
                        }
                        return;
                    }
                }
                /*if (rnd < rare[10] && !(protection == RarifyProtection.Scroll && Rare >= 8))
                {
                    rarify(8);
                }*/
                            if (rnd < rarifyRate[9] && !(protection == RarifyProtection.Scroll && Rare >= 7))
                {
                    rarify(7);
                }
                else if (rnd < rarifyRate[8] && !(protection == RarifyProtection.Scroll && Rare >= 6))
                {
                    rarify(6);
                }
                else if (rnd < rarifyRate[7] && !(protection == RarifyProtection.Scroll && Rare >= 5))
                {
                    rarify(5);
                }
                else if (rnd < rarifyRate[6] && !(protection == RarifyProtection.Scroll && Rare >= 4))
                {
                    rarify(4);
                }
                else if (rnd < rarifyRate[5] && !(protection == RarifyProtection.Scroll && Rare >= 3))
                {
                    rarify(3);
                }
                else if (rnd < rarifyRate[4] && !(protection == RarifyProtection.Scroll && Rare >= 2))
                {
                    rarify(2);
                }
                else if (rnd < rarifyRate[3] && !(protection == RarifyProtection.Scroll && Rare >= 1))
                {
                    rarify(1);
                }
                else if (rnd < rarifyRate[2] && !(protection == RarifyProtection.Scroll && Rare >= 0))
                {
                    rarify(0);
                }
                else if (rnd < rarifyRate[1] && !(protection == RarifyProtection.Scroll && Rare >= -1))
                {
                    rarify(-1);
                }
                else if (rnd < rarifyRate[0] && !(protection == RarifyProtection.Scroll && Rare >= -2))
                {
                    rarify(-2);
                }
                else if (Rare < 1 && Item.ItemType == ItemType.Shell)
                {
                    Rare = 1;
                }
                else if (mode != RarifyMode.Drop && session != null)
                {
                    switch (protection)
                    {
                        case RarifyProtection.BlueAmulet:
                        case RarifyProtection.RedAmulet:
                        case RarifyProtection.HeroicAmulet:
                        case RarifyProtection.RandomHeroicAmulet:
                            session.Character.RemoveBuff(62);
                            ItemInstance amulet = session.Character.Inventory.LoadBySlotAndType((short)EquipmentType.Amulet, InventoryType.Wear);
                            amulet.DurabilityPoint -= 1;
                            if (amulet.DurabilityPoint <= 0)
                            {
                                session.Character.DeleteItemByItemInstanceId(amulet.Id);
                                session.SendPacket($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                                session.SendPacket(session.Character.GenerateEquipment());
                            }
                            else
                            {
                                session.Character.AddBuff(new Buff(62, session.Character.Level), session.Character.BattleEntity);
                            }
                            break;
                        case RarifyProtection.None:
                            session.Character.DeleteItemByItemInstanceId(Id);
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RARIFY_FAILED"), 11));
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("RARIFY_FAILED"), 0));
                            return;
                    }
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("RARIFY_FAILED_ITEM_SAVED"), 11));
                    session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("RARIFY_FAILED_ITEM_SAVED"), 0));
                    session.CurrentMapInstance.Broadcast(session.Character.GenerateEff(3004), session.Character.PositionX, session.Character.PositionY);
                    return;
                }
                if (mode != RarifyMode.Drop && session != null)
                {
                    ItemInstance inventory = session.Character.Inventory.GetItemInstanceById(Id);
                    if (inventory != null)
                    {
                        session.SendPacket(inventory.GenerateInventoryAdd());
                    }
                }
            }

        }
        
        public void SetShellEffects()
        {
            byte CNormCount = 0;
            byte BNormCount = 0;
            byte ANormCount = 0;
            byte SNormCount = 0;
            byte CBonusMax = 0;
            byte BBonusMax = 0;
            byte ABonusMax = 0;
            byte SBonusMax = 0;
            byte CPVPMax = 0;
            byte BPVPMax = 0;
            byte APVPMax = 0;
            byte SPVPMax = 0;

            byte ShellType = 0;
            bool IsWeapon = true;

            switch (ItemVNum)
            {
                case 589:
                case 590:
                case 591:
                case 592:
                case 593:
                case 594:
                case 595:
                case 596:
                case 597:
                case 598:
                    ShellType = 0;
                    break;

                case 565:
                case 566:
                case 567:
                    ShellType = 1;
                    break;

                case 568:
                case 569:
                case 570:
                    ShellType = 2;
                    break;

                case 571:
                case 572:
                case 573:
                    ShellType = 3;
                    break;

                case 574:
                case 575:
                case 576:
                    ShellType = 4;
                    break;

                case 599:
                case 656:
                case 657:
                case 658:
                case 659:
                case 660:
                case 661:
                case 662:
                case 663:
                case 664:
                    ShellType = 0;
                    IsWeapon = false;
                    break;

                case 577:
                case 578:
                case 579:
                    ShellType = 1;
                    IsWeapon = false;
                    break;

                case 580:
                case 581:
                case 582:
                    ShellType = 2;
                    IsWeapon = false;
                    break;

                case 583:
                case 584:
                case 585:
                    ShellType = 3;
                    IsWeapon = false;
                    break;

                case 586:
                case 587:
                case 588:
                    ShellType = 4;
                    IsWeapon = false;
                    break;
            }

            if (Item.EquipmentSlot == EquipmentType.Armor || Item.EquipmentSlot == EquipmentType.MainWeapon || Item.EquipmentSlot == EquipmentType.SecondaryWeapon)
            {
                if (Item.EquipmentSlot == EquipmentType.Armor)
                {
                    ShellType = 4;
                    IsWeapon = false;
                }
                else
                {
                    ShellType = 4;
                }
            }

            switch (Rare)
            {
                case 1:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 1;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 1;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 1;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 1;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 4:
                            CNormCount = 1;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 1;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 1;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;
                    }
                    break;

                case 2:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 2;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 2;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 1;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 2;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 4:
                            CNormCount = 2;
                            BNormCount = 0;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 1;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 1;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;
                    }
                    break;

                case 3:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 2;
                            BNormCount = 1;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 2;
                            BNormCount = 1;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 1;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 1;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 4:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 1;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;
                    }
                    break;

                case 4:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 1;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 2;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 4:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 0;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 1;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 1;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;
                    }
                    break;

                case 5:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 1;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 1;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 1;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 1;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 1;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 2;
                            APVPMax = 1;
                            SPVPMax = 0;
                            break;

                        case 4:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 1;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 1;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 1;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;
                    }
                    break;

                case 6:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 2;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 1;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 2;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 2;
                            APVPMax = 2;
                            SPVPMax = 0;
                            break;

                        case 4:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 1;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 1;
                            APVPMax = 1;
                            SPVPMax = 0;
                            break;
                    }
                    break;

                case 7:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 1;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 2;
                            SNormCount = 1;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 1;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 2;
                            SNormCount = 1;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 2;
                            APVPMax = 2;
                            SPVPMax = 2;
                            break;

                        case 4:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 1;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 1;
                            CPVPMax = 0;
                            BPVPMax = 1;
                            APVPMax = 1;
                            SPVPMax = 1;
                            break;
                    }
                    break;

                case 8:
                    switch (ShellType)
                    {
                        case 0:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 0;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 1:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 2;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 2:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 2;
                            SNormCount = 2;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 2;
                            CPVPMax = 0;
                            BPVPMax = 0;
                            APVPMax = 0;
                            SPVPMax = 0;
                            break;

                        case 3:
                            CNormCount = 1;
                            BNormCount = 1;
                            ANormCount = 2;
                            SNormCount = 2;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 0;
                            CPVPMax = 0;
                            BPVPMax = 2;
                            APVPMax = 2;
                            SPVPMax = 3;
                            break;

                        case 4:
                            CNormCount = 2;
                            BNormCount = 2;
                            ANormCount = 2;
                            SNormCount = 2;
                            CBonusMax = 0;
                            BBonusMax = 0;
                            ABonusMax = 0;
                            SBonusMax = 2;
                            CPVPMax = 0;
                            BPVPMax = 1;
                            APVPMax = 1;
                            SPVPMax = 2;
                            break;
                    }
                    break;
            }

            List<ShellEffectDTO> effectsList = new List<ShellEffectDTO>();

            if (!IsWeapon && SPVPMax == 3)
            {
                SPVPMax = 2;
            }
            if (EquipmentSerialId == Guid.Empty)
            {
                EquipmentSerialId = Guid.NewGuid();
            }
            short CalculateEffect(short maximum)
            {
                if (maximum == 0)
                {
                    return 1;
                }
                else
                {
                    double multiplier = 0;
                    switch (Rare)
                    {
                        case 0:
                        case 1:
                        case 2:
                            multiplier = 0.6;
                            break;

                        case 3:
                            multiplier = 0.65;
                            break;

                        case 4:
                            multiplier = 0.75;
                            break;

                        case 5:
                            multiplier = 0.85;
                            break;

                        case 6:
                            multiplier = 0.95;
                            break;

                        case 7:
                        case 8:
                            multiplier = 1;
                            break;
                    }

                    short min = (short)((maximum / 80D) * (Item.EquipmentSlot == EquipmentType.Armor ? 65 : Item.EquipmentSlot == EquipmentType.MainWeapon ? 65 : Item.EquipmentSlot == EquipmentType.SecondaryWeapon ? 65 : Upgrade - 20) * multiplier);
                    short max = (short)((maximum / 80D) * (Item.EquipmentSlot == EquipmentType.Armor ? 85 : Item.EquipmentSlot == EquipmentType.MainWeapon ? 85 : Item.EquipmentSlot == EquipmentType.SecondaryWeapon ? 85 : Upgrade) * multiplier);
                    if (min == 0)
                    {
                        min = 1;
                    }
                    if (max <= min)
                    {
                        max = (short)(min + 1);
                    }
                    return (short)ServerManager.RandomNumber(min, max);
                }
            }

            void AddEffect(ShellEffectLevelType levelType)
            {
                int i = 0;
                while (i < 10)
                {
                    i++;
                    switch (levelType)
                    {
                        case ShellEffectLevelType.CNormal:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.DamageImproved,
                                    (byte)ShellWeaponEffectType.CriticalChance,
                                    (byte)ShellWeaponEffectType.CriticalDamage,
                                    (byte)ShellWeaponEffectType.Blackout,
                                    (byte)ShellWeaponEffectType.MinorBleeding,
                                };
                                short[] maximum = { 80, 10, 50, 4, 4 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.CloseDefence,
                                        (byte)ShellArmorEffectType.DistanceDefence,
                                        (byte)ShellArmorEffectType.MagicDefence,
                                        (byte)ShellArmorEffectType.ReducedCritChanceRecive,
                                        (byte)ShellArmorEffectType.ReducedStun,
                                        (byte)ShellArmorEffectType.ReducedMinorBleeding,
                                        (byte)ShellArmorEffectType.ReducedShock,
                                        (byte)ShellArmorEffectType.ReducedPoisonParalysis,
                                        (byte)ShellArmorEffectType.ReducedBlind,
                                        (byte)ShellArmorEffectType.RecoveryHPOnRest,
                                        (byte)ShellArmorEffectType.RecoveryMPOnRest
                                    };
                                    maximum = new short[] { 55, 55, 55, 8, 30, 30, 45, 15, 30, 48, 48 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.CNormal, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.BNormal:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.DamageImproved,
                                    (byte)ShellWeaponEffectType.CriticalChance,
                                    (byte)ShellWeaponEffectType.CriticalDamage,
                                    (byte)ShellWeaponEffectType.Freeze,
                                    (byte)ShellWeaponEffectType.Bleeding,
                                    (byte)ShellWeaponEffectType.IncreasedFireProperties,
                                    (byte)ShellWeaponEffectType.IncreasedWaterProperties,
                                    (byte)ShellWeaponEffectType.IncreasedLightProperties,
                                    (byte)ShellWeaponEffectType.IncreasedDarkProperties
                                };
                                short[] maximum = { 150, 16, 16, 4, 4, 120, 120, 120, 120 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.CloseDefence,
                                        (byte)ShellArmorEffectType.DistanceDefence,
                                        (byte)ShellArmorEffectType.MagicDefence,
                                        (byte)ShellArmorEffectType.ReducedCritChanceRecive,
                                        (byte)ShellArmorEffectType.ReducedBlind,
                                        (byte)ShellArmorEffectType.RecoveryHPOnRest,
                                        (byte)ShellArmorEffectType.RecoveryMPOnRest,
                                        (byte)ShellArmorEffectType.ReducedBleedingAndMinorBleeding,
                                        (byte)ShellArmorEffectType.ReducedArmorDeBuff,
                                        (byte)ShellArmorEffectType.ReducedFreeze,
                                        (byte)ShellArmorEffectType.ReducedParalysis,
                                        (byte)ShellArmorEffectType.IncreasedFireResistence,
                                        (byte)ShellArmorEffectType.IncreasedWaterResistence,
                                        (byte)ShellArmorEffectType.IncreasedLightResistence,
                                        (byte)ShellArmorEffectType.IncreasedDarkResistence
                                    };
                                    maximum = new short[] { 95, 95, 95, 13, 40, 85, 85, 27, 42, 38, 27, 8, 8, 8, 8 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.BNormal, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.ANormal:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.DamageImproved,
                                    (byte)ShellWeaponEffectType.HeavyBleeding,
                                    (byte)ShellWeaponEffectType.IncreasedFireProperties,
                                    (byte)ShellWeaponEffectType.IncreasedWaterProperties,
                                    (byte)ShellWeaponEffectType.IncreasedLightProperties,
                                    (byte)ShellWeaponEffectType.IncreasedDarkProperties,
                                    (byte)ShellWeaponEffectType.SLDamage,
                                    (byte)ShellWeaponEffectType.SLDefence,
                                    (byte)ShellWeaponEffectType.SLElement,
                                    (byte)ShellWeaponEffectType.SLHP,
                                };
                                short[] maximum = { 250, 1, 200, 200, 200, 200, 15, 15, 15, 15 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.CloseDefence,
                                        (byte)ShellArmorEffectType.DistanceDefence,
                                        (byte)ShellArmorEffectType.MagicDefence,
                                        (byte)ShellArmorEffectType.ReducedFreeze,
                                        (byte)ShellArmorEffectType.ReducedParalysis,
                                        (byte)ShellArmorEffectType.ReducedAllStun,
                                        (byte)ShellArmorEffectType.ReducedAllBleedingType,
                                        (byte)ShellArmorEffectType.RecoveryHP,
                                        (byte)ShellArmorEffectType.RecoveryMP,
                                        (byte)ShellArmorEffectType.IncreasedFireResistence,
                                        (byte)ShellArmorEffectType.IncreasedWaterResistence,
                                        (byte)ShellArmorEffectType.IncreasedLightResistence,
                                        (byte)ShellArmorEffectType.IncreasedDarkResistence
                                    };
                                    maximum = new short[] { 160, 160, 160, 43, 35, 40, 40, 80, 80, 16, 16, 16, 16 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.ANormal, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.SNormal:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.PercentageTotalDamage,
                                    (byte)ShellWeaponEffectType.IncreasedElementalProperties,
                                    (byte)ShellWeaponEffectType.CriticalDamage,
                                    (byte)ShellWeaponEffectType.SLGlobal,
                                };
                                short[] maximum = { 24, 230, 100, 9 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.PercentageTotalDefence,
                                        (byte)ShellArmorEffectType.ReducedAllNegativeEffect,
                                        (byte)ShellArmorEffectType.IncreasedAllResistence,
                                        (byte)ShellArmorEffectType.RecoveryHPInDefence
                                    };
                                    maximum = new short[] { 20, 33, 22, 56 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.SNormal, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.CBonus:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.GainMoreGold,
                                    (byte)ShellWeaponEffectType.GainMoreXP
                                };
                                short[] maximum = { 7, 4 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.ReducedPrideLoss,
                                    };
                                    maximum = new short[] { 45 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.CBonus, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.BBonus:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.GainMoreGold,
                                    (byte)ShellWeaponEffectType.GainMoreXP
                                };
                                short[] maximum = { 13, 6 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.IncreasedRecoveryItemSpeed
                                    };
                                    maximum = new short[] { 21 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.BBonus, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.ABonus:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.GainMoreGold,
                                    (byte)ShellWeaponEffectType.GainMoreXP
                                };
                                short[] maximum = { 28, 12 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.IncreasedRecoveryItemSpeed
                                    };
                                    maximum = new short[] { 46 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.ABonus, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.SBonus:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.GainMoreGold,
                                    (byte)ShellWeaponEffectType.GainMoreXP
                                };
                                short[] maximum = { 40, 18 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.IncreasedRecoveryItemSpeed
                                    };
                                    maximum = new short[] { 55 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.SBonus, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.CPVP:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.PercentageDamageInPVP,
                                    (byte)ShellWeaponEffectType.ReducesPercentageEnemyDefenceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyMPInPVP
                                };
                                short[] maximum = { 8, 8, 12 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.PercentageAllPVPDefence,
                                        (byte)ShellArmorEffectType.CloseDefenceDodgeInPVP,
                                        (byte)ShellArmorEffectType.DistanceDefenceDodgeInPVP,
                                        (byte)ShellArmorEffectType.IgnoreMagicDamage
                                    };
                                    maximum = new short[] { 9, 4, 4, 4 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.CPVP, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.BPVP:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.PercentageDamageInPVP,
                                    (byte)ShellWeaponEffectType.ReducesPercentageEnemyDefenceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyMPInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyFireResistanceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyWaterResistanceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyLightResistanceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyDarkResistanceInPVP
                                };
                                short[] maximum = { 12, 12, 20, 6, 6, 6, 6 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.PercentageAllPVPDefence,
                                        (byte)ShellArmorEffectType.CloseDefenceDodgeInPVP,
                                        (byte)ShellArmorEffectType.DistanceDefenceDodgeInPVP,
                                        (byte)ShellArmorEffectType.IgnoreMagicDamage
                                    };
                                    maximum = new short[] { 11, 6, 6, 6 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.BPVP, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.APVP:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.PercentageDamageInPVP,
                                    (byte)ShellWeaponEffectType.ReducesPercentageEnemyDefenceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyMPInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyFireResistanceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyWaterResistanceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyLightResistanceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyDarkResistanceInPVP
                                };
                                short[] maximum = { 17, 17, 42, 15, 15, 15, 15 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.PercentageAllPVPDefence,
                                        (byte)ShellArmorEffectType.CloseDefenceDodgeInPVP,
                                        (byte)ShellArmorEffectType.DistanceDefenceDodgeInPVP,
                                        (byte)ShellArmorEffectType.IgnoreMagicDamage,
                                        (byte)ShellArmorEffectType.ProtectMPInPVP
                                    };
                                    maximum = new short[] { 20, 12, 12, 12, 0 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.APVP, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                        case ShellEffectLevelType.SPVP:
                            {
                                byte[] effects = {
                                    (byte)ShellWeaponEffectType.PercentageDamageInPVP,
                                    (byte)ShellWeaponEffectType.ReducesPercentageEnemyDefenceInPVP,
                                    (byte)ShellWeaponEffectType.ReducesEnemyAllResistancesInPVP
                                };
                                short[] maximum = { 35, 35, 17 };

                                if (!IsWeapon)
                                {
                                    effects = new byte[]
                                    {
                                        (byte)ShellArmorEffectType.PercentageAllPVPDefence,
                                        (byte)ShellArmorEffectType.DodgeAllAttacksInPVP
                                    };
                                    maximum = new short[] { 32, 16 };
                                }

                                int position = ServerManager.RandomNumber(0, effects.Length);
                                byte effect = effects[position];
                                short value = CalculateEffect(maximum[position]);

                                if (effectsList.Any(s => s.Effect == effect))
                                {
                                    continue;
                                }

                                effectsList.Add(new ShellEffectDTO { EffectLevel = ShellEffectLevelType.SPVP, Effect = effect, Value = value, EquipmentSerialId = EquipmentSerialId });
                                return;
                            }
                    }
                }
            }

            for (int i = 0; i < CNormCount; i++)
            {
                AddEffect(ShellEffectLevelType.CNormal);
            }

            for (int i = 0; i < BNormCount; i++)
            {
                AddEffect(ShellEffectLevelType.BNormal);
            }

            for (int i = 0; i < ANormCount; i++)
            {
                AddEffect(ShellEffectLevelType.ANormal);
            }

            for (int i = 0; i < SNormCount; i++)
            {
                AddEffect(ShellEffectLevelType.SNormal);
            }

            for (int i = 0; i < CBonusMax; i++)
            {
                AddEffect(ShellEffectLevelType.CBonus);
            }

            for (int i = 0; i < BBonusMax; i++)
            {
                AddEffect(ShellEffectLevelType.BBonus);
            }

            for (int i = 0; i < ABonusMax; i++)
            {
                AddEffect(ShellEffectLevelType.ABonus);
            }

            for (int i = 0; i < SBonusMax; i++)
            {
                AddEffect(ShellEffectLevelType.SBonus);
            }

            for (int i = 0; i < SPVPMax; i++)
            {
                AddEffect(ShellEffectLevelType.SPVP);
            }

            for (int i = 0; i < APVPMax; i++)
            {
                AddEffect(ShellEffectLevelType.APVP);
            }

            for (int i = 0; i < BPVPMax; i++)
            {
                AddEffect(ShellEffectLevelType.BPVP);
            }

            for (int i = 0; i < CPVPMax; i++)
            {
                AddEffect(ShellEffectLevelType.CPVP);
            }

            ShellEffects.Clear();
            DAOFactory.ShellEffectDAO.DeleteByEquipmentSerialId(this.EquipmentSerialId);
            ShellEffects.AddRange(effectsList);

            DAOFactory.ShellEffectDAO.InsertOrUpdateFromList(ShellEffects, EquipmentSerialId);
        }


        public void SetRarityPoint()
        {
            switch (Item.EquipmentSlot)
            {
                case EquipmentType.MainWeapon:
                case EquipmentType.SecondaryWeapon:
                    {
                        int point = CharacterHelper.RarityPoint(Rare, Item.IsHeroic ? (short)(95 + Item.LevelMinimum) : Item.LevelMinimum, false);
                        Concentrate = 0;
                        HitRate = 0;
                        DamageMinimum = 0;
                        DamageMaximum = 0;
                        if (Rare >= 0)
                        {
                            for (int i = 0; i < point; i++)
                            {
                                int rndn = ServerManager.RandomNumber(0, 3);
                                if (rndn == 0)
                                {
                                    Concentrate++;
                                    HitRate++;
                                }
                                else
                                {
                                    DamageMinimum++;
                                    DamageMaximum++;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i > Rare * 10; i--)
                            {
                                DamageMinimum--;
                                DamageMaximum--;
                            }
                        }
                    }
                    break;

                case EquipmentType.Armor:
                    {
                        int point = CharacterHelper.RarityPoint(Rare, Item.IsHeroic ? (short)(95 + Item.LevelMinimum) : Item.LevelMinimum, true);
                        DefenceDodge = 0;
                        DistanceDefenceDodge = 0;
                        DistanceDefence = 0;
                        MagicDefence = 0;
                        CloseDefence = 0;
                        double NewDistanceDefence = 0;
                        double NewMagicDefence = 0;
                        double NewCloseDefence = 0;
                        if (Rare >= 0)
                        {
                            for (int i = 0; i < point; i++)
                            {
                                int rndn = ServerManager.RandomNumber(0, 5);
                                if (rndn == 0)
                                {
                                    DefenceDodge++;
                                    DistanceDefenceDodge++;
                                }
                                else
                                {
                                    NewDistanceDefence = NewDistanceDefence + 0.9;
                                    NewMagicDefence = NewMagicDefence + 0.35;
                                    NewCloseDefence = NewCloseDefence + 0.95;
                                }
                            }
                            DistanceDefence = (short)NewDistanceDefence;
                            MagicDefence = (short)NewMagicDefence;
                            CloseDefence = (short)NewCloseDefence;
                        }
                        else
                        {
                            for (int i = 0; i > Rare * 10; i--)
                            {
                                DistanceDefence--;
                                MagicDefence--;
                                CloseDefence--;
                            }
                        }
                    }
                    break;
            }
        }

        public void Sum(ClientSession session, ItemInstance itemToSum)
        {
            if (!session.HasCurrentMapInstance)
            {
                return;
            }
            if (Upgrade < 6)
            {
                short[] upsuccess = { 100, 100, 85, 70, 50, 20 };
                int[] goldprice = { 1500, 3000, 6000, 12000, 24000, 48000 };
                short[] sand = { 5, 10, 15, 20, 25, 30 };
                const int sandVnum = 1027;
                if (Upgrade + itemToSum.Upgrade < 6 && ((itemToSum.Item.EquipmentSlot == EquipmentType.Gloves && Item.EquipmentSlot == EquipmentType.Gloves) || (Item.EquipmentSlot == EquipmentType.Boots && itemToSum.Item.EquipmentSlot == EquipmentType.Boots)))
                {
                    if (session.Character.Gold < goldprice[Upgrade])
                    {
                        return;
                    }
                    if (session.Character.Inventory.CountItem(sandVnum) < sand[Upgrade])
                    {
                        return;
                    }
                    session.Character.Inventory.RemoveItemAmount(sandVnum, (byte)sand[Upgrade]);
                    session.Character.Gold -= goldprice[Upgrade];

                    int rnd = ServerManager.RandomNumber();
                    if (rnd < upsuccess[Upgrade + itemToSum.Upgrade])
                    {
                        Logger.LogUserEvent("SUM_ITEM", session.GenerateIdentity(), $"[SumItem]ItemId {Id} ItemToSumId: {itemToSum.Id} Upgrade: {Upgrade} ItemToSumUpgrade: {itemToSum.Upgrade} Result: Success");

                        Upgrade += (byte)(itemToSum.Upgrade + 1);
                        DarkResistance += (short)(itemToSum.DarkResistance + itemToSum.Item.DarkResistance);
                        LightResistance += (short)(itemToSum.LightResistance + itemToSum.Item.LightResistance);
                        WaterResistance += (short)(itemToSum.WaterResistance + itemToSum.Item.WaterResistance);
                        FireResistance += (short)(itemToSum.FireResistance + itemToSum.Item.FireResistance);
                        session.Character.DeleteItemByItemInstanceId(itemToSum.Id);
                        session.SendPacket($"pdti 10 {ItemVNum} 1 27 {Upgrade} 0");
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("SUM_SUCCESS"), 0));
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("SUM_SUCCESS"), 12));
                        session.SendPacket(UserInterfaceHelper.GenerateGuri(19, 1, session.Character.CharacterId, 1324));
                        session.SendPacket(GenerateInventoryAdd());
                    }
                    else
                    {
                        Logger.LogUserEvent("SUM_ITEM", session.GenerateIdentity(), $"[SumItem]ItemId {Id} ItemToSumId: {itemToSum.Id} Upgrade: {Upgrade} ItemToSumUpgrade: {itemToSum.Upgrade} Result: Fail");

                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("SUM_FAILED"), 0));
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("SUM_FAILED"), 11));
                        session.SendPacket(UserInterfaceHelper.GenerateGuri(19, 1, session.Character.CharacterId, 1332));
                        session.Character.DeleteItemByItemInstanceId(itemToSum.Id);
                        session.Character.DeleteItemByItemInstanceId(Id);
                    }
                    session.CurrentMapInstance?.Broadcast(UserInterfaceHelper.GenerateGuri(6, 1, session.Character.CharacterId), session.Character.PositionX, session.Character.PositionY);
                    session.SendPacket(session.Character.GenerateGold());
                    session.SendPacket("shop_end 1");
                }
            }
        }

        public void UpgradeSpFun(ClientSession session, UpgradeProtection protect, int value)
        {
            if (session == null)
            {
                return;
            }
            if (Upgrade >= 19)
            {
                return;
            }

            short ScrollChicken = 5107;
            short ScrollPyjama = 5207;
            short ScrollPirate = 5519;

            if (!session.HasCurrentMapInstance)
            {
                return;
            }

            if (value == 1)
            {
                if (protect == UpgradeProtection.Event)
                {
                    if (session.Character.Inventory.CountItem(ScrollChicken) < 1)
                    {
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(ScrollChicken).Name, 1)), 10));
                        return;
                    }
                    session.Character.Inventory.RemoveItemAmount(ScrollChicken);
                    session.SendPacket(session.Character.Inventory.CountItem(ScrollChicken) < 1 ? "shop_end 2" : "shop_end 1");
                }
            }
            if (value == 3)
            {
                if (protect == UpgradeProtection.Event)
                {
                    if (session.Character.Inventory.CountItem(ScrollPirate) < 1)
                    {
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(ScrollPirate).Name, 1)), 10));
                        return;
                    }
                    session.Character.Inventory.RemoveItemAmount(ScrollPirate);
                    session.SendPacket(session.Character.Inventory.CountItem(ScrollPirate) < 1 ? "shop_end 2" : "shop_end 1");
                }
            }
            if (value == 2)
            {
                if (protect == UpgradeProtection.Event)
                {
                    if (session.Character.Inventory.CountItem(ScrollPyjama) < 1)
                    {
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(ScrollPyjama).Name, 1)), 10));
                        return;
                    }
                    session.Character.Inventory.RemoveItemAmount(ScrollPyjama);
                    session.SendPacket(session.Character.Inventory.CountItem(ScrollPyjama) < 1 ? "shop_end 2" : "shop_end 1");
                }
            }
            ItemInstance wearable = session.Character.Inventory.GetItemInstanceById(Id);
            ItemInstance inventory = session.Character.Inventory.GetItemInstanceById(Id);
            int rnd = ServerManager.RandomNumber();
            if (protect == UpgradeProtection.Protected)
            {
                session.CurrentMapInstance.Broadcast(session.Character.GenerateEff(3004), session.Character.MapX, session.Character.MapY);
            }
            session.CurrentMapInstance.Broadcast(session.Character.GenerateEff(3005), session.Character.MapX, session.Character.MapY);
            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_SUCCESS"), 12));
            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_SUCCESS"), 0));
            wearable.Upgrade++;
            if (wearable.Upgrade > 8)
            {
                session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name, itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
            }
            session.SendPacket(wearable.GenerateInventoryAdd());

            session.SendPacket(session.Character.GenerateGold());
            session.SendPacket(session.Character.GenerateEq());
            session.SendPacket("shop_end 1");
            session.SendPacket("wopen 20 0");
        }

        //Fix EQ Auto Upgrade
        public void UpgradeItem(ClientSession session, UpgradeMode mode, UpgradeProtection protection, bool isCommand = false)
        {
            int i;
            for (i = 1; i <= 99; i++)
            {
            if (!session.HasCurrentMapInstance)
            {
                return;
            }
            if (Upgrade < 10)
            {
                byte[] upfail;
                byte[] upfix;
                int[] goldprice;
                short[] cella;
                byte[] gem;

                if (Rare >= 8)
                {
                    if (ServerManager.Instance.Configuration.DobleEqUp == true)
                    {
                        upfix = ServerManager.Instance.RateItem().R8ItemUpgradeFixRate;
                        upfail = ServerManager.Instance.RateItem().R8ItemUpgradeFailRateEvent;
                    }
                    else
                    {
                        upfix = ServerManager.Instance.RateItem().R8ItemUpgradeFixRate;
                        upfail = ServerManager.Instance.RateItem().R8ItemUpgradeFailRateEvent;
                    }

                    goldprice = new[] { 5000, 15000, 30000, 100000, 300000, 800000, 1500000, 4000000, 7000000, 10000000 };
                    cella = new short[] { 40, 100, 160, 240, 320, 440, 560, 760, 960, 1200 };
                    gem = new byte[] { 2, 2, 4, 4, 6, 2, 2, 4, 4, 6 };
                }
                else
                {
                    if (ServerManager.Instance.Configuration.DobleEqUp)
                    {
                        upfix = ServerManager.Instance.RateItem().ItemUpgradeFixRate;
                        upfail = ServerManager.Instance.RateItem().ItemUpgradeFailRateEvent;
                    }
                    else
                    {
                        upfix = ServerManager.Instance.RateItem().ItemUpgradeFixRate;
                        upfail = ServerManager.Instance.RateItem().ItemUpgradeFailRate;
                    }

                    goldprice = new[] { 500, 1500, 3000, 10000, 30000, 80000, 150000, 400000, 700000, 1000000 };
                    cella = new short[] { 20, 50, 80, 120, 160, 220, 280, 380, 480, 600 };
                    gem = new byte[] { 1, 1, 2, 2, 3, 1, 1, 2, 2, 3 };
                }

                const short cellaVnum = 1014;
                const short gemVnum = 1015;
                const short gemFullVnum = 1016;
                const double reducedpricefactor = 0.5;
                const short normalScrollVnum = 1218;
                const short goldScrollVnum = 5369;

                if (IsFixed)
                {
                    session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("ITEM_IS_FIXED"), 10));
                    session.SendPacket("shop_end 1");
                    return;
                }
                switch (mode)
                {
                    case UpgradeMode.Free:
                        break;

                    case UpgradeMode.Reduced:
                        if (session.Character.Gold < (long)(goldprice[Upgrade] * reducedpricefactor))
                        {
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
                            return;
                        }
                        if (session.Character.Inventory.CountItem(cellaVnum) < cella[Upgrade] * reducedpricefactor)
                        {
                            session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(cellaVnum).Name, cella[Upgrade] * reducedpricefactor), 10));
                            return;
                        }
                        if (protection == UpgradeProtection.Protected && !isCommand && session.Character.Inventory.CountItem(goldScrollVnum) < 1)
                        {
                            session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(goldScrollVnum).Name, cella[Upgrade] * reducedpricefactor), 10));
                            return;
                        }
                        if (Upgrade < 5)
                        {
                            if (session.Character.Inventory.CountItem(gemVnum) < gem[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(gemVnum).Name, gem[Upgrade]), 10));
                                return;
                            }
                            session.Character.Inventory.RemoveItemAmount(gemVnum, gem[Upgrade]);
                        }
                        else
                        {
                            if (session.Character.Inventory.CountItem(gemFullVnum) < gem[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(gemFullVnum).Name, gem[Upgrade]), 10));
                                return;
                            }
                            session.Character.Inventory.RemoveItemAmount(gemFullVnum, gem[Upgrade]);
                        }
                        if (protection == UpgradeProtection.Protected && !isCommand)
                        {
                            session.Character.Inventory.RemoveItemAmount(goldScrollVnum);
                            session.SendPacket("shop_end 2");
                        }
                        session.Character.Gold -= (long)(goldprice[Upgrade] * reducedpricefactor);
                        session.Character.Inventory.RemoveItemAmount(cellaVnum, (int)(cella[Upgrade] * reducedpricefactor));
                        session.SendPacket(session.Character.GenerateGold());
                        break;

                    case UpgradeMode.Normal:
                        if (session.Character.Inventory.CountItem(cellaVnum) < cella[Upgrade])
                        {
                            return;
                        }
                        if (session.Character.Gold < goldprice[Upgrade])
                        {
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
                            return;
                        }
                        if (protection == UpgradeProtection.Protected && !isCommand && session.Character.Inventory.CountItem(normalScrollVnum) < 1)
                        {
                            session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(normalScrollVnum).Name, 1), 10));
                            return;
                        }
                        if (Upgrade < 5)
                        {
                            if (session.Character.Inventory.CountItem(gemVnum) < gem[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(gemVnum).Name, gem[Upgrade]), 10));
                                return;
                            }
                            session.Character.Inventory.RemoveItemAmount(gemVnum, gem[Upgrade]);
                        }
                        else
                        {
                            if (session.Character.Inventory.CountItem(gemFullVnum) < gem[Upgrade])
                            {
                                session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.GetItem(gemFullVnum).Name, gem[Upgrade]), 10));
                                return;
                            }
                            session.Character.Inventory.RemoveItemAmount(gemFullVnum, gem[Upgrade]);
                        }
                        if (protection == UpgradeProtection.Protected && !isCommand)
                        {
                            session.Character.Inventory.RemoveItemAmount(normalScrollVnum);
                            session.SendPacket("shop_end 2");
                        }
                        session.Character.Inventory.RemoveItemAmount(cellaVnum, cella[Upgrade]);
                        session.Character.Gold -= goldprice[Upgrade];
                        session.SendPacket(session.Character.GenerateGold());
                        break;
                }
                ItemInstance wearable = session.Character.Inventory.GetItemInstanceById(Id);
                int rnd = ServerManager.RandomNumber();
                if (Rare == 8)
                {
                    if (rnd < upfail[Upgrade])
                    {
                        Logger.LogUserEvent("UPGRADE_ITEM", session.GenerateIdentity(), $"[UpgradeItem]ItemType: {wearable.Item.ItemType} Protection: {protection.ToString()} IIId: {Id} Upgrade: {wearable.Upgrade} Result: Fail");

                        if (protection == UpgradeProtection.None)
                        {
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADE_FAILED"), 11));
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_FAILED"), 0));
                            session.Character.DeleteItemByItemInstanceId(Id);
                        }
                        else
                        {
                            session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3004), session.Character.PositionX, session.Character.PositionY);
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("SCROLL_PROTECT_USED"), 11));
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_FAILED_ITEM_SAVED"), 0));
                        }
                    }
                    else if (rnd < upfix[Upgrade])
                    {
                        Logger.LogUserEvent("UPGRADE_ITEM", session.GenerateIdentity(), $"[UpgradeItem]ItemType: {wearable.Item.ItemType} Protection: {protection.ToString()} IIId: {Id} Upgrade: {wearable.Upgrade} Result: Fixed");

                        session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3004), session.Character.PositionX, session.Character.PositionY);
                        wearable.IsFixed = true;
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADE_FIXED"), 11));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_FIXED"), 0));
                    }
                    else
                    {
                        Logger.LogUserEvent("UPGRADE_ITEM", session.GenerateIdentity(), $"[UpgradeItem]ItemType: {wearable.Item.ItemType} Protection: {protection.ToString()} IIId: {Id} Upgrade: {wearable.Upgrade} Result: Success");

                        session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3005), session.Character.PositionX, session.Character.PositionY);
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADE_SUCCESS"), 12));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_SUCCESS"), 0));
                        wearable.Upgrade++;
                        if (wearable.Upgrade > 4)
                        {
                            session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name, itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                        }
                        session.SendPacket(wearable.GenerateInventoryAdd());
                    }
                }
                else
                {
                    if (rnd < upfix[Upgrade])
                    {
                        Logger.LogUserEvent("UPGRADE_ITEM", session.GenerateIdentity(), $"[UpgradeItem]ItemType: {wearable.Item.ItemType} Protection: {protection.ToString()} IIId: {Id} Upgrade: {wearable.Upgrade} Result: Fixed");

                        session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3004), session.Character.PositionX, session.Character.PositionY);
                        wearable.IsFixed = true;
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADE_FIXED"), 11));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_FIXED"), 0));
                    }
                    else if (rnd < upfail[Upgrade] + upfix[Upgrade])
                    {
                        Logger.LogUserEvent("UPGRADE_ITEM", session.GenerateIdentity(), $"[UpgradeItem]ItemType: {wearable.Item.ItemType} Protection: {protection.ToString()} IIId: {Id} Upgrade: {wearable.Upgrade} Result: Fail");

                        if (protection == UpgradeProtection.None)
                        {
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADE_FAILED"), 11));
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_FAILED"), 0));
                            session.Character.DeleteItemByItemInstanceId(Id);
                        }
                        else
                        {
                            session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3004), session.Character.PositionX, session.Character.PositionY);
                            session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("SCROLL_PROTECT_USED"), 11));
                            session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_FAILED_ITEM_SAVED"), 0));
                        }
                    }
                    else
                    {
                        Logger.LogUserEvent("UPGRADE_ITEM", session.GenerateIdentity(), $"[UpgradeItem]ItemType: {wearable.Item.ItemType} Protection: {protection.ToString()} IIId: {Id} Upgrade: {wearable.Upgrade} Result: Success");

                        session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3005), session.Character.PositionX, session.Character.PositionY);
                        session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADE_SUCCESS"), 12));
                        session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADE_SUCCESS"), 0));
                        wearable.Upgrade++;
                        if (wearable.Upgrade > 4)
                        {
                            session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name, itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                        }
                        session.SendPacket(wearable.GenerateInventoryAdd());
                    }
                }
                session.SendPacket("shop_end 1");
            }

            }

        }

        public void ConvertToPartnerEquipment(ClientSession session)
        {
            const int sandVnum = 1027;
            long goldprice = 2000 + Item.LevelMinimum * 300;

            if (session.Character.Gold >= goldprice && session.Character.Inventory.CountItem(sandVnum) >= Item.LevelMinimum)
            {
                session.Character.Inventory.RemoveItemAmount(sandVnum, Item.LevelMinimum);
                session.Character.Gold -= goldprice;

                IsPartnerEquipment = true;
                ShellEffects.Clear();
                ShellRarity = null;
                DAOFactory.ShellEffectDAO.DeleteByEquipmentSerialId(EquipmentSerialId);
                BoundCharacterId = null;
                HoldingVNum = ItemVNum;

                switch (Item.EquipmentSlot)
                {
                    case EquipmentType.MainWeapon:
                    case EquipmentType.SecondaryWeapon:
                        switch (Item.Class)
                        {
                            case 2:
                                ItemVNum = 990;
                                break;
                            case 4:
                                ItemVNum = 991;
                                break;
                            case 8:
                                ItemVNum = 992;
                                break;
                        }
                        break;
                    case EquipmentType.Armor:
                        switch (Item.Class)
                        {
                            case 2:
                                ItemVNum = 997;
                                break;
                            case 4:
                                ItemVNum = 996;
                                break;
                            case 8:
                                ItemVNum = 995;
                                break;
                        }
                        break;
                }
                session.SendPacket(GenerateInventoryAdd());
                session.SendPacket("shop_end 1");
            }
        }

        #endregion
    }
}