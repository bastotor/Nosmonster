using OpenNos.Core;
using OpenNos.Domain;

namespace NosTale.Packets.Packets.CommandPackets
{
    [PacketHeader("$Test", PassNonParseablePacket = true, Authority = AuthorityType.Administrator)]
    public class TestCommandPacket : PacketDefinition
    {
        #region
        #endregion
    }
}