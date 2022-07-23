using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using OpenNos.Master.Library.Client;
using OpenNos.Master.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenNos.GameObject.Event.GAMES
{

    public static class WorldRad
    {
        #region Properties

        public static int AngelDamage { get; set; }

        public static MapInstance WorldMapinstance { get; set; }

        public static int DemonDamage { get; set; }

        public static bool IsLocked { get; set; }

        public static bool IsRunning { get; set; }

        public static int RemainingTime { get; set; }

   

        public static MapInstance UnknownLandMapInstance { get; set; }
        public static MapInstance HatusLandMapInstance { get; internal set; }

        #endregion

        #region Methods

        public static void Run()
        {
            WolrdBoss raidThread = new WolrdBoss();
            Observable.Timer(TimeSpan.FromMinutes(0)).Subscribe(X => raidThread.Run());
        }

        #endregion
    }

    public class WolrdBoss
    {

        public static ClientSession Session { get; }
        #region Methods

        public void Run()
        {
            CommunicationServiceClient.Instance.SendMessageToCharacter(new SCSCharacterMessage
            {
                DestinationCharacterId = null,
                SourceCharacterId = 0,
                SourceWorldId = ServerManager.Instance.WorldId,
                Message = $"Chaos summoned a boss that appeared in the spawn",
                Type = MessageType.Shout
            });

            WorldRad.RemainingTime = 2400;
            const int interval = 1;

            WorldRad.WorldMapinstance = ServerManager.GenerateMapInstance(140, MapInstanceType.WorldBossInstance, new InstanceBag());
            WorldRad.UnknownLandMapInstance = ServerManager.GetMapInstance(ServerManager.GetBaseMapInstanceIdByMapId(2700));


            // RETOUR
            WorldRad.WorldMapinstance.CreatePortal(new Portal
            {
                SourceMapInstanceId = WorldRad.WorldMapinstance.MapInstanceId,
                SourceX =5 ,
                SourceY = 45,
                DestinationMapId = 2700,
                DestinationX = 57,
                DestinationY = 81,
                DestinationMapInstanceId = WorldRad.UnknownLandMapInstance.MapInstanceId,
                Type = -1
            });


            // ALLER
            WorldRad.UnknownLandMapInstance.CreatePortal(new Portal
            {
                SourceMapId = 2700,
                SourceX = 86,
                SourceY = 31,
                DestinationMapId = 140,
                DestinationX = 44,
                DestinationY = 4,
                DestinationMapInstanceId = WorldRad.WorldMapinstance.MapInstanceId,
                Type = -1
            });

            List<EventContainer> onDeathEvents = new List<EventContainer>
            {
               new EventContainer(WorldRad.WorldMapinstance, EventActionType.SCRIPTEND, (byte)1)
            };

            #region Fafnir

            MapMonster FafnirMonster = new MapMonster
            {
                MonsterVNum = 2619,
                MapY = 31,
                MapX = 18,
                MapId = WorldRad.WorldMapinstance.Map.MapId,
                Position = 5,
                IsMoving = true,
                MapMonsterId = WorldRad.WorldMapinstance.GetNextMonsterId(),
                ShouldRespawn = false
            };
            FafnirMonster.Initialize(WorldRad.WorldMapinstance);
            WorldRad.WorldMapinstance.AddMonster(FafnirMonster);
            MapMonster Fafnir = WorldRad.WorldMapinstance.Monsters.Find(s => s.Monster.NpcMonsterVNum == 2619);
            if (Fafnir != null)
            {
                Fafnir.BattleEntity.OnDeathEvents = onDeathEvents;
                Fafnir.IsBoss = true;
            }
            #endregion

            Observable.Timer(TimeSpan.FromMinutes(15)).Subscribe(X => LockRaid());
            Observable.Timer(TimeSpan.FromMinutes(60)).Subscribe(X => EndRaid());


        }

        private void EndRaid()
        {
            ServerManager.Shout(Language.Instance.GetMessageFromKey("WORDLBOSS_END"), true);

            foreach (ClientSession sess in WorldRad.WorldMapinstance.Sessions.ToList())
            {
                ServerManager.Instance.ChangeMapInstance(sess.Character.CharacterId, WorldRad.UnknownLandMapInstance.MapInstanceId, sess.Character.MapX, sess.Character.MapY);
                Thread.Sleep(100);
            }
            EventHelper.Instance.RunEvent(new EventContainer(WorldRad.WorldMapinstance, EventActionType.DISPOSEMAP, null));
            WorldRad.IsRunning = false;
            WorldRad.AngelDamage = 0;
            WorldRad.DemonDamage = 0;
            ServerManager.Instance.StartedEvents.Remove(EventType.WORLDBOSS);
            WorldRad.IsLocked = true;

        }
        private void LockRaid()
        {
            foreach (Portal p in WorldRad.UnknownLandMapInstance.Portals.Where(s => s.DestinationMapInstanceId == WorldRad.WorldMapinstance.MapInstanceId).ToList())
            {
                WorldRad.UnknownLandMapInstance.Portals.Remove(p);
                WorldRad.UnknownLandMapInstance.Broadcast(p.GenerateGp());
            }
            ServerManager.Shout(Language.Instance.GetMessageFromKey("WORLDBOSS_LOCKED"), true);
            WorldRad.IsLocked = true;
        }

        #endregion
    }
}