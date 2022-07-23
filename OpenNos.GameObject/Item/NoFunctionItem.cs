/*
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
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using System;
using System.Linq;

namespace OpenNos.GameObject
{
    public class NoFunctionItem : Item
    {
        #region Instantiation

        public NoFunctionItem(ItemDTO item) : base(item)
        {
        }

        #endregion

        #region Methods

        public override void Use(ClientSession session, ref ItemInstance inv, byte Option = 0, string[] packetsplit = null)
        {
            if (session.Character.IsVehicled)
            {
                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("CANT_DO_VEHICLED"), 10));
                return;
            }

            if (session.CurrentMapInstance.MapInstanceType == Domain.MapInstanceType.TalentArenaMapInstance)
            {
                return;
            }

            switch (Effect)
            {
                case 10:
                    {
                        switch (EffectValue)
                        {
                            case 1:
                                if (session.Character.Inventory.CountItem(1036) < 1 || session.Character.Inventory.CountItem(1013) < 1)
                                {
                                    return;
                                }
                                session.Character.Inventory.RemoveItemAmount(1036);
                                session.Character.Inventory.RemoveItemAmount(1013);
                                if (ServerManager.RandomNumber() < 25)
                                {
                                    switch (ServerManager.RandomNumber(0, 2))
                                    {
                                        case 0:
                                            session.Character.GiftAdd(1015, 1);
                                            break;
                                        case 1:
                                            session.Character.GiftAdd(1016, 1);
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                if (session.Character.Inventory.CountItem(1038) < 1 || session.Character.Inventory.CountItem(1013) < 1)
                                {
                                    return;
                                }
                                session.Character.Inventory.RemoveItemAmount(1038);
                                session.Character.Inventory.RemoveItemAmount(1013);
                                if (ServerManager.RandomNumber() < 25)
                                {
                                    switch (ServerManager.RandomNumber(0, 4))
                                    {
                                        case 0:
                                            session.Character.GiftAdd(1031, 1);
                                            break;
                                        case 1:
                                            session.Character.GiftAdd(1032, 1);
                                            break;
                                        case 2:
                                            session.Character.GiftAdd(1033, 1);
                                            break;
                                        case 3:
                                            session.Character.GiftAdd(1034, 1);
                                            break;
                                    }
                                }
                                break;


                            case 3:
                                if (session.Character.Inventory.CountItem(1037) < 1 || session.Character.Inventory.CountItem(1013) < 1)
                                {
                                    return;
                                }
                                session.Character.Inventory.RemoveItemAmount(1037);
                                session.Character.Inventory.RemoveItemAmount(1013);
                                if (ServerManager.RandomNumber() < 25)
                                {
                                    switch (ServerManager.RandomNumber(0, 17))
                                    {
                                        case 0:
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                            session.Character.GiftAdd(1017, 1);
                                            break;
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                            session.Character.GiftAdd(1018, 1);
                                            break;
                                        case 9:
                                        case 10:
                                        case 11:
                                            session.Character.GiftAdd(1019, 1);
                                            break;
                                        case 12:
                                        case 13:
                                            session.Character.GiftAdd(1020, 1);
                                            break;
                                        case 14:
                                            session.Character.GiftAdd(1021, 1);
                                            break;
                                        case 15:
                                            session.Character.GiftAdd(1022, 1);
                                            break;
                                        case 16:
                                            session.Character.GiftAdd(1023, 1);
                                            break;
                                    }
                                }
                                break;
                        }

                        session.Character.GiftAdd(1014, (byte)ServerManager.RandomNumber(5, 11));
                    }
                    break;
                  
                     //RESIZE 10=>1
                case 25099:
                    {
                            session.Character.Size = 1;
                        session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());

                    }
                    break;

                    //RESIZE 1=>10  //Chaos
                case 25100:
                    {
                            session.Character.Size = 10;
                        session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());

                    }
                    break;

                case 2600:
                    {

                        ItemInstance specialistInstance1 = session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
                        if (specialistInstance1 != null)
                        {

                            short[] upsuccess = { 50, 40, 30, 20, 10 };

                            int[] goldprice = { 5000, 10000, 20000, 50000, 100000 };
                            byte[] stoneprice = { 1, 2, 3, 4, 5 };
                            short stonevnum;
                            byte upmode = 1;
                            short SpDamage = 0;
                            short SpDefence = 0;
                            short SpElement = 0;
                            short SpHP = 0;
                            short SpFire = 0;
                            short SpWater = 0;
                            short SpLight = 0;
                            short SpDark = 0;
                            short Fallimenti = 0;
                            short Successi = 0;

                            switch (specialistInstance1.Item.Morph)
                            {
                                case 2:
                                case 6:
                                case 9:
                                case 12:
                                case 1403:
                                    stonevnum = 2514;
                                    break;

                                case 3:
                                case 4:
                                case 14:
                                    stonevnum = 2515;
                                    break;

                                case 5:
                                case 11:
                                case 15:
                                    stonevnum = 2516;
                                    break;

                                case 10:
                                case 13:
                                case 7:
                                    stonevnum = 2517;
                                    break;

                                case 17:
                                case 18:
                                case 19:
                                    stonevnum = 2518;
                                    break;

                                case 20:
                                case 21:
                                case 22:
                                    stonevnum = 2519;
                                    break;

                                case 23:
                                case 24:
                                case 25:
                                    stonevnum = 2520;
                                    break;

                                case 26:
                                case 27:
                                case 28:
                                    stonevnum = 2521;
                                    break;

                                default:
                                    return;
                            }

                            while (session.Character.Inventory.CountItem(stonevnum) > 0)
                            {
                                if (specialistInstance1.SpStoneUpgrade > 99)
                                {
                                    break;
                                }
                                if (specialistInstance1.SpStoneUpgrade > 80)
                                {
                                    upmode = 5;
                                }
                                else if (specialistInstance1.SpStoneUpgrade > 60)
                                {
                                    upmode = 4;
                                }
                                else if (specialistInstance1.SpStoneUpgrade > 40)
                                {
                                    upmode = 3;
                                }
                                else if (specialistInstance1.SpStoneUpgrade > 20)
                                {
                                    upmode = 2;
                                }

                                if (session.Character.Gold < goldprice[upmode - 1])
                                {
                                    break;
                                }
                                if (session.Character.Inventory.CountItem(stonevnum) < stoneprice[upmode - 1])
                                {
                                    break;
                                }
                                int rnd9 = ServerManager.RandomNumber();
                                if (rnd9 < upsuccess[upmode - 1])
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
                                    if (type < 3)
                                    {
                                        specialistInstance1.SpDamage += count;
                                        SpDamage += count;
                                    }
                                    else if (type < 6)
                                    {
                                        specialistInstance1.SpDefence += count;
                                        SpDefence += count;
                                    }
                                    else if (type < 9)
                                    {
                                        specialistInstance1.SpElement += count;
                                        SpElement += count;
                                    }
                                    else if (type < 12)
                                    {
                                        specialistInstance1.SpHP += count;
                                        SpHP += count;
                                    }
                                    else if (type == 12)
                                    {
                                        specialistInstance1.SpFire += count;
                                        SpFire += count;
                                    }
                                    else if (type == 13)
                                    {
                                        specialistInstance1.SpWater += count;
                                        SpWater += count;
                                    }
                                    else if (type == 14)
                                    {
                                        specialistInstance1.SpLight += count;
                                        SpLight += count;
                                    }
                                    else if (type == 15)
                                    {
                                        specialistInstance1.SpDark += count;
                                        SpDark += count;
                                    }
                                    specialistInstance1.SpStoneUpgrade++;
                                    Successi++;
                                }
                                else
                                {
                                    Fallimenti++;
                                }
                                session.SendPacket(specialistInstance1.GenerateInventoryAdd());
                                session.Character.Gold -= goldprice[upmode - 1];
                                session.SendPacket(session.Character.GenerateGold());
                                session.Character.Inventory.RemoveItemAmount(stonevnum, stoneprice[upmode - 1]);
                            }
                            if (Successi > 0 || Fallimenti > 0)
                            {
                                session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3005), session.Character.MapX, session.Character.MapY);
                                session.SendPacket(session.Character.GenerateSay("---------------Perfection Status---------------", 11));
                                session.SendPacket(session.Character.GenerateSay("Succès: " + Successi, 11));
                                session.SendPacket(session.Character.GenerateSay("Fails: " + Fallimenti, 11));
                                session.SendPacket(session.Character.GenerateSay("Attaque: " + SpDamage, 11));
                                session.SendPacket(session.Character.GenerateSay("Défense: " + SpDefence, 11));
                                session.SendPacket(session.Character.GenerateSay("HP: " + SpHP, 11));
                                session.SendPacket(session.Character.GenerateSay("Element " + SpElement, 11));
                                session.SendPacket(session.Character.GenerateSay("Feu: " + SpFire, 11));
                                session.SendPacket(session.Character.GenerateSay("Eau: " + SpWater, 11));
                                session.SendPacket(session.Character.GenerateSay("Lumière: " + SpLight, 11));
                                session.SendPacket(session.Character.GenerateSay("Obscure: " + SpDark, 11));
                                session.SendPacket(session.Character.GenerateSay("-----------------------------------------------", 11));
                            }
                            else
                            {
                                session.SendPacket(session.Character.GenerateSay("NOT_ENOUGH_ITEM", 10));
                            }
                        }
                        else
                        {
                            session.SendPacket(session.Character.GenerateSay("WEAR_SP", 10));
                        }
                    }
                    break;
                default:
                    Logger.Warn(string.Format(Language.Instance.GetMessageFromKey("NO_HANDLER_ITEM"), GetType(), VNum, Effect, EffectValue));
                    break;
            }
        }

        #endregion
    }
}