﻿using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.CommandPackets
{
    [PacketHeader("$ChangeLock", PassNonParseablePacket = true, Authority = AuthorityType.User)]
    public class ChangeLockPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public string oldlock { get; set; }

        [PacketIndex(1)]
        public string newlock { get; set; }

        public static string ReturnHelp()
        {
            return "$ChangeLock ACTUALCODE NEWCODE";
        }

        #endregion
    }
}