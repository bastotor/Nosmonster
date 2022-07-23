﻿using NosTale.Packets;
using OpenNos.Core;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Extension;
using OpenNos.GameObject.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Handler.World.Family
{
    public class FamilySkillPacketHandler : IPacketHandler
    {
        private ClientSession Session { get; set; }

        public FamilySkillPacketHandler(ClientSession session) => Session = session;

        public void UseFamilySkill(FwsPacket fwsPacket)
        {
            if (Session.Character.Family != null && Session.Character.FamilyCharacter != null && Session.Character.Family.FamilySkillMissions.FirstOrDefault(s => s.ItemVNum == fwsPacket.ItemVNum && s.CurrentValue == 1) is FamilySkillMission fsm && fsm != null)
            {
                if (Session.Character.FamilyCharacter.Authority == FamilyAuthority.Familydeputy || Session.Character.FamilyCharacter.Authority == FamilyAuthority.Head)
                {
                    switch (fwsPacket.ItemVNum)
                    {
                        case 9600:
                            ServerManager.Instance.Configuration.FamilyExpBuff = true;
                            ServerManager.Instance.Configuration.TimeExpBuff = DateTime.Now.AddMinutes(60);
                            Observable.Timer(TimeSpan.FromMinutes(60)).Subscribe(x => { ServerManager.Instance.Configuration.FamilyExpBuff = false; });

                            foreach (ClientSession s in ServerManager.Instance.Sessions)
                            {
                                s.Character.AddStaticBuff(new StaticBuffDTO
                                {
                                    CardId = 360,
                                    CharacterId = s.Character.CharacterId,
                                    RemainingTime = (int)(ServerManager.Instance.Configuration.TimeExpBuff - DateTime.Now).TotalSeconds
                                });
                            }
                            ServerManager.Shout($"Family {Session.Character.Family.Name} used Exp Buff at Channel {ServerManager.Instance.ChannelId}");
                            break;
                        case 9601:
                            ServerManager.Instance.Configuration.FamilyGoldBuff = true;
                            ServerManager.Instance.Configuration.TimeGoldBuff = DateTime.Now.AddMinutes(60);
                            Observable.Timer(TimeSpan.FromMinutes(60)).Subscribe(x => { ServerManager.Instance.Configuration.FamilyExpBuff = false; });
                            foreach (ClientSession s in ServerManager.Instance.Sessions)
                            {
                                s.Character.AddStaticBuff(new StaticBuffDTO
                                {
                                    CardId = 361,
                                    CharacterId = s.Character.CharacterId,
                                    RemainingTime = (int)(ServerManager.Instance.Configuration.TimeGoldBuff - DateTime.Now).TotalSeconds
                                });
                            }
                            ServerManager.Shout($"Family {Session.Character.Family.Name} used Gold Buff at Channel {ServerManager.Instance.ChannelId}");
                            break;
                        case 9602:
                            if (ServerManager.Instance.ChannelId != 51) return;
                            if (Session.Character.Family.FamilyFaction != (byte)FactionType.Angel) return;
                            if (ServerManager.Instance.Act4AngelStat.Mode != 0) return;
                            if (ServerManager.Instance.Act4DemonStat.Mode != 0) return;
                            ServerManager.Instance.Act4AngelStat.Percentage += 2000;
                            break;
                        case 9603:
                            if (ServerManager.Instance.ChannelId != 51) return;
                            if (Session.Character.Family.FamilyFaction != (byte)FactionType.Demon) return;
                            if (ServerManager.Instance.Act4AngelStat.Mode != 0) return;
                            if (ServerManager.Instance.Act4DemonStat.Mode != 0) return;
                            ServerManager.Instance.Act4DemonStat.Percentage += 2000;
                            break;

                        default:
                            return;
                    }
                    Session.Character.Family.InsertFamilyLog(FamilyLogType.SkillUse, characterName: Session.Character.Name, itemVNum: (fwsPacket.ItemVNum - 600));
                    fsm.CurrentValue = 0;
                    Session.Character.Family.SaveMission(fsm);
                }
            }
        }
    }
}
