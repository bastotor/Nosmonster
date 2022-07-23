﻿using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.CommandPackets
{
    [PacketHeader("$Unlock", PassNonParseablePacket = true, Authority = AuthorityType.User)]
    public class UnlockPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public string lockcode { get; set; }

        public static string ReturnHelp()
        {
            return "$Unlock CODE";
        }

        #endregion
    }
}