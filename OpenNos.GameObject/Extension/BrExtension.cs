using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.GameObject.Extension
{
    public static class BrExtension
    {
		public static void SaveLevel(this ClientSession Session)
		{
			int rnd = ServerManager.RandomNumber(0, 3);
			Session.Character.LevelSaved = Session.Character.Level;
			Session.Character.HeroLevelSaved = Session.Character.HeroLevel;
			Session.Character.JobLevelSaved = Session.Character.JobLevel;
			Session.Character.PrestigeSaved = Session.Character.Prestige;
			Session.Character.ReputationSaved = Session.Character.Reputation;
			if (Session.Character.Class == ClassType.Adventurer)
			{
				switch (rnd)
				{
					case 0:
						Session.Character.ChangeClass(ClassType.Archer, false);
						break;
					case 1:
						Session.Character.ChangeClass(ClassType.Swordsman, false);
						break;
					case 2:
						Session.Character.ChangeClass(ClassType.Magician, false);
						break;
					default:
						Session.Character.ChangeClass(ClassType.Archer, false);
						break;
				}
				Session.Character.IsAdventurerAfterBattle = true;
			}
			Session.Character.IsBattleRoyalLevel = true;
			Session.Character.Prestige = 0;
			Session.Character.Level = 99;
			Session.Character.JobLevel = 80;
			Session.Character.HeroLevel = 50;
			Session.Character.Reputation = 500000;
			Session.Character.Hp = (int)Session.Character.HPLoad();
			Session.Character.Mp = (int)Session.Character.MPLoad();
			Session.SendPacket(Session.Character.GenerateStat());
			Session.SendPackets(Session.Character.GenerateStatChar());
			Session.SendPacket(Session.Character.GenerateLev());
			Session.SendPacket(Session.Character.GenerateFd());
			Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateIn(true), ReceiverType.AllExceptMe);
			Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateGidx(), ReceiverType.AllExceptMe);

		}

		private static void RemoveSP(this ClientSession session, short itemVNum)
		{
			if (session?.HasSession == true && !session.Character.IsVehicled)
			{
				List<BuffType> bufftodisable = new List<BuffType> { BuffType.Bad, BuffType.Good, BuffType.Neutral };
				session.Character.DisableBuffs(BuffType.All);
				session.Character.UseSp = false;
				session.Character.LoadSpeed();
				session.SendPacket(session.Character.GenerateCond());
				session.SendPacket(session.Character.GenerateLev());
				session.Character.SpCooldown = 30;
				if (session.Character.SkillsSp != null)
				{
					foreach (CharacterSkill ski in session.Character.SkillsSp.Where(s => !s.CanBeUsed()))
					{
						short time = ski.Skill.Cooldown;
						double temp = (ski.LastUse - DateTime.Now).TotalMilliseconds + (time * 100);
						temp /= 1000;
						session.Character.SpCooldown = temp > session.Character.SpCooldown ? (int)temp : session.Character.SpCooldown;
					}
				}
				session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("STAY_TIME"), session.Character.SpCooldown), 11));
				session.SendPacket($"sd {session.Character.SpCooldown}");
				session.CurrentMapInstance?.Broadcast(session.Character.GenerateCMode());
				session.CurrentMapInstance?.Broadcast(UserInterfaceHelper.GenerateGuri(6, 1, session.Character.CharacterId), session.Character.PositionX, session.Character.PositionY);

				// ms_c
				session.SendPacket(session.Character.GenerateSki());
				session.SendPackets(session.Character.GenerateQuicklist());
				session.SendPacket(session.Character.GenerateStat());
				session.SendPackets(session.Character.GenerateStatChar());

				Observable.Timer(TimeSpan.FromMilliseconds(session.Character.SpCooldown * 1000)).Subscribe(o =>
				{
					session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("TRANSFORM_DISAPPEAR"), 11));
					session.SendPacket("sd 0");
				});
			}
		}

		public static void VerifyStuff(this ClientSession Session)
		{
			if (Session.Character.UseSp)
			{
				Session.Character.LastSp = (DateTime.Now - Process.GetCurrentProcess().StartTime.AddSeconds(-50)).TotalSeconds;
				ItemInstance specialist = Session.Character.Inventory.LoadBySlotAndType((byte)EquipmentType.Sp, InventoryType.Wear);
				if (specialist != null)
				{
					Session.RemoveSP(specialist.ItemVNum);
				}
			}
			Session.Character.Inventory.RemoveStuff(Session, 0, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 1, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 2, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 3, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 4, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 5, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 6, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 7, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 8, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 9, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 10, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 11, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 12, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 13, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 14, true, true);
			Session.Character.Inventory.RemoveStuff(Session, 15, true, true);
		}

		public static void BackupLevelNormal(this ClientSession Session)
		{
			if (!Session.Character.IsBattleRoyalLevel)
			{
				return;
			}

			if (Session.Character.IsAdventurerAfterBattle == true)
			{
				Session.Character.ChangeClass(ClassType.Adventurer, false);
				Session.Character.IsAdventurerAfterBattle = false;
			}
			Session.Character.IsBattleRoyalLevel = false;
			Session.Character.Level = Session.Character.LevelSaved;
			Session.Character.HeroLevel = Session.Character.HeroLevelSaved;
			Session.Character.JobLevel = Session.Character.JobLevelSaved;
			Session.Character.Prestige = Session.Character.PrestigeSaved;
			Session.Character.Reputation = Session.Character.ReputationSaved;
			Session.Character.Hp = (int)Session.Character.HPLoad();
			Session.Character.Mp = (int)Session.Character.MPLoad();
			Session.SendPacket(Session.Character.GenerateStat());
			Session.SendPackets(Session.Character.GenerateStatChar());
			Session.SendPacket(Session.Character.GenerateLev());
			Session.SendPacket(Session.Character.GenerateFd());
			Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateIn(), ReceiverType.AllExceptMe);
			Session.CurrentMapInstance?.Broadcast(Session, Session.Character.GenerateGidx(), ReceiverType.AllExceptMe);
			Parallel.ForEach(Session.Character.Inventory.Where(s => s.IsBattleRoyal == true),
									inv =>
									{
										Session.Character.Inventory.DeleteById(inv.Id);
										Session.SendPacket(UserInterfaceHelper.Instance.GenerateInventoryRemove(inv.Type, inv.Slot));
									});
		}
	}
}
