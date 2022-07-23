using OpenNos.Core;
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace OpenNos.GameObject.Event
{
    public class BattleRoyale
    {

        #region Methods

        public static void GenerateBattleRoyale(bool useTimer = true)
        {
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale will begin in 5 minutes", 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale will begin in 5 minutes", 1));
            Thread.Sleep(4 * 60 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale will begin in 1 minute", 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale will begin in 1 minute", 1));
            Thread.Sleep(30 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale will begin in 30 seconds", 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("La Battle Royale start after 30 seconds", 1));
            Thread.Sleep(20 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale will begin in 10 seconds", 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale will begin in 10 second", 1));
            Thread.Sleep(10 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale has started", 1));
            ServerManager.Instance.Broadcast($"qnaml 1 #guri^506 Participate in Battle Royale?");
            ServerManager.Instance.EventInWaiting = true;
            Thread.Sleep(10 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale has started", 1));
            ServerManager.Instance.Sessions.Where(s => s.Character != null && !s.Character.IsWaitingForEvent).ToList().ForEach(s => s.SendPacket("esf"));
            ServerManager.Instance.EventInWaiting = false;
            IEnumerable<ClientSession> sessions = ServerManager.Instance.Sessions.Where(s => s.Character != null && s.Character.IsWaitingForEvent && s.Character.MapId != 2106);
            List<Tuple<MapInstance, byte>> maps = new List<Tuple<MapInstance, byte>>();
            MapInstance map = null;
            int i = -1;
            int level = 0;
            byte instancelevel = 1;
            foreach (ClientSession s in sessions.OrderBy(s => s.Character?.Level))
            {
                i++;
                if (i == 0)
                {
                    map = ServerManager.GenerateMapInstance(2620, MapInstanceType.BattleRoyaleInstance, new InstanceBag());
                    maps.Add(new Tuple<MapInstance, byte>(map, instancelevel));
                }
                if (i == 9)
                {
                    i = -1;
                }
                if (map != null)
                {
                    ServerManager.Instance.TeleportOnRandomPlaceInMap(s, map.MapInstanceId);
                    s.Character.Buff.ClearAll();
                }
                else
                {
                    ServerManager.Instance.Broadcast($"msg 0 Error in Teleportation in Battle Royale");
                }
                level = s.Character.Level;
            }
            ServerManager.Instance.Sessions.Where(s => s.Character != null).ToList().ForEach(s => s.Character.IsWaitingForEvent = false);
            ServerManager.Instance.StartedEvents.Remove(EventType.INSTANTBATTLE);
            foreach (Tuple<MapInstance, byte> mapinstance in maps)
            {
                SurvivalTask task = new SurvivalTask();
                Observable.Timer(TimeSpan.FromMinutes(0)).Subscribe(X => SurvivalTask.Run(mapinstance));
            }
        }

        public class SurvivalTask
        {
            public static void Run(Tuple<MapInstance, byte> mapinstance)
            {
                if (!mapinstance.Item1.Sessions.Skip(1).Any())
                {
                    mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg("You won the Battle Royale", 1));
                    mapinstance.Item1.Sessions.ToList().ForEach(x =>
                    {
                        x.Character.Reputation += x.Character.Level * 20;
                        if (x.Character.Dignity < 100)
                        {
                            x.Character.Dignity = 100;
                        }
                        CharacterDTO ch = x.Character;
                        DAOFactory.CharacterDAO.InsertOrUpdate(ref ch);
                        x.Character.Gold += x.Character.Level * 12000;
                        x.Character.Gold = x.Character.Gold > ServerManager.Instance.Configuration.MaxGold ? ServerManager.Instance.Configuration.MaxGold : x.Character.Gold;
                        x.SendPacket(x.Character.GenerateFd());
                        x.SendPacket(x.Character.GenerateGold());
                        x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("WIN_MONEY"), x.Character.Level * 12000), 10));
                        x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("WIN_REPUT"), x.Character.Level * 20), 10));
                        x.SendPacket(x.Character.GenerateSay(string.Format(("Dignity restored"), 100), 10));
                    });
                    Thread.Sleep(5000);
                    mapinstance.Item1.Sessions.Where(s => s.Character != null).ToList().ForEach(s => ServerManager.Instance.ChangeMap(s.Character.CharacterId, s.Character.MapId, s.Character.MapX, s.Character.MapY));
                }
                Observable.Timer(TimeSpan.FromSeconds(15)).Subscribe(c =>
                {

                    mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg("The Battle will begin in 10 seconds", 0));
                    Thread.Sleep(10 * 1000);
                    mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg("The Battle will begin in 5 seconds", 0));
                    Thread.Sleep(5 * 1000);
                    mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg("Ready...", 0));
                    Thread.Sleep(1000);
                    mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg("Battle Royale has started!", 0));
                    mapinstance.Item1.IsPVP = true;
                    for (int timeI = 30; timeI >= 1; timeI--)
                    {
                        if (mapinstance.Item1.Sessions.Count() == 1)
                        {
                            mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg("You won the Battle Royale", 1));
                            mapinstance.Item1.Sessions.ToList().ForEach(x =>
                            {
                                x.Character.Reputation += x.Character.Level * 5;
                                if (x.Character.Dignity < 100)
                                {
                                    x.Character.Dignity = 100;
                                    x.SendPacket(x.Character.GenerateSay(string.Format(("Restored dignity"), 100), 10));
                                }

                                x.Character.Gold += x.Character.Level * 12000;
                                x.Character.Gold = x.Character.Gold > ServerManager.Instance.Configuration.MaxGold ? ServerManager.Instance.Configuration.MaxGold : x.Character.Gold;
                                x.Character.GiftAdd(5976, 1);
                                x.SendPacket(x.Character.GenerateFd());
                                x.SendPacket(x.Character.GenerateGold());
                                x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("WIN_MONEY"), x.Character.Level * 25000), 10));
                                x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("WIN_REPUT"), x.Character.Level * 20), 10));
                                EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(10), new EventContainer(mapinstance.Item1, EventActionType.DISPOSEMAP, null));
                            });
                        }
                        else
                        {
                            mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg("Remaining time : " + timeI + " minutes / or", 0));
                            Thread.Sleep(30 * 1000);
                        }
                    }
                    mapinstance.Item1.Broadcast(UserInterfaceHelper.GenerateMsg(("Battle Royale is over!"), 0));
                    mapinstance.Item1.IsPVP = false;
                    mapinstance.Item1.Sessions.ToList().ForEach(x =>
                    {
                        x.Character.Reputation += x.Character.Level * 30;
                        if (x.Character.Dignity < 50)
                        {
                            x.Character.Dignity = 50;
                        }

                        x.Character.Gold += x.Character.Level * 12000;
                        x.Character.Gold = x.Character.Gold > ServerManager.Instance.Configuration.MaxGold ? ServerManager.Instance.Configuration.MaxGold : x.Character.Gold;
                        x.SendPacket(x.Character.GenerateFd());
                        x.SendPacket(x.Character.GenerateGold());
                        x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("WIN_MONEY"), x.Character.Level * 12000), 10));
                        x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("WIN_REPUT"), x.Character.Level * 10), 10));
                        x.SendPacket(x.Character.GenerateSay(string.Format(("Dignity restored")), 10));
                    });
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(10), new EventContainer(mapinstance.Item1, EventActionType.DISPOSEMAP, null));
                });
            }
        }

        #endregion
    }
}