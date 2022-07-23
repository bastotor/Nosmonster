using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Extension;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using OpenNos.Master.Library.Client;
using OpenNos.Master.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenNos.GameObject.Event.GAMES
{
    public class BattleROYA2
    {
        public const int MaxAllowedPlayers = 25;

        public const int MiniAllowedPlayers = 2;

        public const int ItemSaved = 99999;

        private static List<Group> _groups { get; set; }

        public static MapInstance Map { get; private set; }
        public static bool Spawn { get; private set; }

        public static void AddGroup(Group group)
        {
            _groups.Add(group);
        }

        public static Group GetGroupByClientSession(ClientSession session)
        {
            return _groups.FirstOrDefault(x => x.IsMemberOfGroup(session));
        }

        public static void MergeGroups(IEnumerable<Group> groups)
        {
            Group group = new Group
            {
                GroupType = GroupType.BattleRoyal,
            };
            AddGroup(group);
        }

        public static void RemoveGroup(Group group)
        {
            _groups.Remove(group);
        }

        public static bool SessionsHaveSameGroup(ClientSession session1, ClientSession session2)
        {
            return _groups != null && _groups.Any(x => x.IsMemberOfGroup(session1) && x.IsMemberOfGroup(session2));
        }


        public static void GenerateBattleRoyal()
        {
            Initialize();
            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(
                string.Format(Language.Instance.GetMessageFromKey("BATTLER_SECONDS"), 30), 1));
#pragma warning disable 4014
            DiscordWebhookHelper.DiscordEventT($"ServerEvent: Battle Royale will start in 30 seconds, are you ready to succeed together?");
            Thread.Sleep(20 * 1000);

            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(
                string.Format(Language.Instance.GetMessageFromKey("BATTLER_SECONDS"), 10), 1));
            Thread.Sleep(10 * 1000);

            ServerManager.Instance.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("BATTLE_STARTED"), 1));
            ServerManager.Instance.EventInWaiting = true;
            ServerManager.Instance.Sessions.Where(x => x.CurrentMapInstance.MapInstanceType == MapInstanceType.BaseMapInstance).ToList().ForEach(x => x.SendPacket($"qnaml 7 #guri^99999 {string.Format(Language.Instance.GetMessageFromKey("BATTLER_ASK"), 500)}"));
            Observable.Timer(TimeSpan.FromSeconds(30)).Subscribe(c =>
            {
                ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == false).ToList().ForEach(s => s.SendPacket("esf 0"));
                ServerManager.Instance.StartedEvents.Remove(EventType.BATTLEROYAL);
                ServerManager.Instance.EventInWaiting = false;
                if (Map.Sessions.Count() >= MiniAllowedPlayers)
                {
                    Map.Sessions.ToList().ForEach(x =>
                    {

                        if (x.Character.IsVehicled)
                        {
                            x.Character.RemoveVehicle();
                        }
                        ServerManager.Instance.TeleportOnRandomPlaceInMap(x, Map.MapInstanceId);
                        x.SendPacket("bsinfo 2 5 0 0");
                        x.VerifyStuff();
                        x.SaveLevel();
                        x.Character.Group?.LeaveGroup(x);
                    });
                    Thread.Sleep(10 * 100);
                    if (Map.Sessions.Count() >= MiniAllowedPlayers)
                    {
                        Map.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("BATTLER_FIGHT_WARN"), 0));
                        Observable.Timer(TimeSpan.FromSeconds(6)).Subscribe(s =>
                        {
                            Map.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("BATTLER_FIGHT_WARN"), 0));
                        });
                        Observable.Timer(TimeSpan.FromSeconds(13)).Subscribe(s =>
                        {
                            Map.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("BATTLER_FIGHT_WARN"), 0));
                        });
                        Observable.Timer(TimeSpan.FromSeconds(14)).Subscribe(s =>
                        {
                            Map.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("BATTLER_FIGHT_WARN"), 0));
                        });

                        Observable.Timer(TimeSpan.FromSeconds(15)).Subscribe(s =>
                        {
                            Map.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("BATTLER_FIGHT_START"), 0));
                            Map.IsPVP = true;
                            Spawn = true;
                            Map.Broadcast("bsinfo 1 5 600 60");
                            Observable.Timer(TimeSpan.FromMinutes(5)).Subscribe(e =>
                            {
                                Map.Sessions.ToList().ForEach(x =>
                                {
                                    x.VerifyStuff();
                                    x.BackupLevelNormal();
                                });
                                Thread.Sleep(1 * 1000);
                                End();
                            });

                        });
                        IDisposable obs = null;
                        obs = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(b =>
                        {
                            if (Map.Sessions.Count() > 1 && _groups.Count > 1)
                            {
                                return;
                            }

                            Map.Broadcast(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("BATTLER_WIN"), 0));
                            Map.Sessions.ToList().ForEach(x =>
                            {
                                Parallel.ForEach(x.CurrentMapInstance.Monsters.Where(s => s.ShouldRespawn != true), monster =>
                                {
                                    x.CurrentMapInstance.Broadcast(StaticPacketHelper.Out(UserType.Monster,
                                        monster.MapMonsterId));
                                    x.CurrentMapInstance.RemoveMonster(monster);
                                });
                                Parallel.ForEach(x.CurrentMapInstance.DroppedList.GetAllItems(), drop =>
                                {
                                    x.CurrentMapInstance.Broadcast(StaticPacketHelper.Out(UserType.Object, drop.TransportId));
                                    x.CurrentMapInstance.DroppedList.Remove(drop.TransportId);
                                });
                                CommunicationServiceClient.Instance.SendMessageToCharacter(new SCSCharacterMessage
                                {
                                    DestinationCharacterId = null,
                                    SourceCharacterId = x.Character.CharacterId,
                                    SourceWorldId = ServerManager.Instance.WorldId,
                                    Message = $"{x.Character.Name} viens de gagner la bataille royale",
                                    Type = MessageType.Shout
                                });
                                x.SendPacket("bsinfo 2 5 0 60");
                                x.Character.GetReputation(x.Character.Level * 100);
                                x.Character.GiftAdd(2361, 20);
                                if (x.Character.Dignity < 100)
                                {
                                    x.Character.Dignity = 100;
                                }
                                x.Character.Gold = x.Character.Gold > ServerManager.Instance.Configuration.MaxGold ? ServerManager.Instance.Configuration.MaxGold : x.Character.Gold;
                                x.SendPacket(x.Character.GenerateFd());
                                x.SendPacket(x.Character.GenerateGold());
                                x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("WIN_REPUT"), x.Character.Level * 10), 100));
                                x.SendPacket(x.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("DIGNITY_RESTORED"), x.Character.Level * 10), 10));
                                obs.Dispose();
                                Map.IsPVP = false;
                                Spawn = false;
                                x.VerifyStuff();
                                x.BackupLevelNormal();

                            });
                            EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(10), new EventContainer(Map, EventActionType.DISPOSEMAP, null));
                        });
                        int i = 0;

                        while (Map?.Sessions?.Any() == true)
                        {
                            runRound(i++, Map.Sessions.Count() * 2);
                            runPièce(i++, Map.Sessions.Count() * 2);
                        }
                    }
                    else
                    {
                        Map.Sessions.ToList().ForEach(x =>
                        {
                            x.VerifyStuff();
                            x.BackupLevelNormal();
                        });
                        ServerManager.Instance.StartedEvents.Remove(EventType.BATTLEROYAL);
                        EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(10), new EventContainer(Map, EventActionType.DISPOSEMAP, null));
                    }
                }
                else
                {
                    ServerManager.Instance.StartedEvents.Remove(EventType.BATTLEROYAL);
                    EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(10), new EventContainer(Map, EventActionType.DISPOSEMAP, null));
                }
            });
        }

        private static IEnumerable<Tuple<short, int, short, short>> generateDrop(Map map, short vnum, int amountofdrop, int amount)
        {
            List<Tuple<short, int, short, short>> dropParameters = new List<Tuple<short, int, short, short>>();
            for (int i = 0; i < amountofdrop; i++)
            {
                MapCell cell = map.GetRandomPosition();
                dropParameters.Add(new Tuple<short, int, short, short>(vnum, amount, cell.X, cell.Y));
            }
            return dropParameters;
        }
        private static void runRound(int number, int ez)
        {
            int amount = 120 + (60 * number);
            int i = 1;
            while (i != 0)
            {
                SpawnBox(1, ez);
                Thread.Sleep(50 * 100);// / amount);
                i--;
            }
            Thread.Sleep(50 * 100);
        }

        private static void runPièce(int number, int ez)
        {
            int amount = 120 + (60 * number);
            int i = 1;
            while (i != 0)
            {
                SpawnPièce(1, ez);
                Thread.Sleep(50 * 100);// / amount);
                i--;
            }
            Thread.Sleep(50 * 100);
        }

        private static void End()
        {
            Map.Sessions.ToList().ForEach(x =>
            {
                Map.Broadcast("bsinfo 2 5 0 60");
                Map.Broadcast("Personne n'a gagné le temps est fini.");
                Thread.Sleep(1 * 1000);
                ServerManager.Instance.StartedEvents.Remove(EventType.BATTLEROYAL);
                EventHelper.Instance.ScheduleEvent(TimeSpan.FromSeconds(10), new EventContainer(Map, EventActionType.DISPOSEMAP, null));
                x.Character.GiftAdd(2361, 3);
            });



        }

        private static void SpawnBox(int round, int Number)
        {
            if (Map != null)
            {
                if (Spawn == true)
                {
                    Map.DropItems(generateDrop(Map.Map, 1000, Number, 1).ToList());
                }
            }
        }

        private static void SpawnPièce(int round, int Number)
        {
            if (Map != null)
            {
                if (Spawn == true)
                {
                    Map.DropItems(generateDrop(Map.Map, 2361, Number, 1).ToList());

                }
            }
        }


        private static void Initialize()
        {
            short[] MapRandom = { 260 };
            ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == false).ToList().ForEach(s => s.SendPacket("esf 0"));
            Map = ServerManager.GenerateMapInstance(260, MapInstanceType.BattleRoyalInstance, new InstanceBag());
            _groups = new List<Group>();
        }
    }
}
