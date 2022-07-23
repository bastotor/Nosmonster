﻿using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.CommandPackets
{
    [PacketHeader("$GiveMall", PassNonParseablePacket = true, Authority = AuthorityType.Administrator)]
    public class GiveMallPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public short Amount { get; set; }

        [PacketIndex(1)]
        public string CharacterName { get; set; }

        public static string ReturnHelp() => "$GiveMall <Amount> <Nickname>";

        #endregion
    }
}