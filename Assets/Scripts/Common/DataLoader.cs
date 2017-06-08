using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warsmiths.Common.Domain.Enums;
using System.Linq;
using Assets.Scripts.DataClasses;
using Assets.Scripts.Utils;
using Warsmiths.Common.Domain;
using Warsmiths.Common.Domain.Equipment;
using Object = UnityEngine.Object;


namespace Assets.Scripts.Common
{
    public class DataLoader
    {

        // Super mega script
        public class ClassesStatClass
        {
            public List<GenesisInf> Data;

            public ClassesStatClass()
            {
                Data = new List<GenesisInf>();
            }
        }
        public static List<Sprite> LoadSpritePack(string sprpackname)
        {
            return Resources.LoadAll<Sprite>(sprpackname).ToList();
        }

        public static T LoadData<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
        public static List<T> LoadAllData<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path).ToList();
        }

        public static ArmorTypes ConverArmorType(BaseArmor arm)
        {
            return arm.ArmorType;
        }

        public static Sprite LoadRaceArmor(ArmorTypes atype, int state) // light mid heavy
        {
            // print("Base/Armor/" + PlayerModel.I.Character.RaceType + "/" + atype + "/" + state);
            return LoadData<Sprite>("Base/Armor/" + PlayerModel.I.Character.RaceType + "/" + atype + "/" + state);
        }

        public static Sprite LoadRaceArmor(BaseArmor arm, int state) // light mid heavy
        {
            return LoadRaceArmor(ConverArmorType(arm), state);

        }
        public static Sprite LoadElement(BaseElement atype) // Elements..... Fixed by p.iluhin
        {
            return LoadData<Sprite>("Elements/" + atype.Type + "" + atype.ColorType);
        }
        public static Sprite LoadBackground()
        {
            var a = PlayerModel.I.Character.Equipment.Armor;
            if (a == null) return null;
            var aName = a.ArmorType.ToString();
            var s = "Base/Race/Backgrounds/" + PlayerModel.I.Character.RaceType + "_" + aName;
            return LoadData<Sprite>(s);
        }


        public static Dictionary<ClassTypes, ClassInf> ClassInfo;
        public static Dictionary<ClassTypes, Sprite> ClassIcon;
        public static Dictionary<HeroTypes, HeroInf> HeroInfo;
        public static Dictionary<HeroTypes, Sprite> HeroIcon;
        public static Dictionary<StartBonusTypes, Sprite> GenesisIcon;
        public static Dictionary<StartBonusTypes, GenesisInf> GenesisInfo;


        public static Dictionary<string, GenesisInf> ClassesStats;

        // Use this for initialization
        static DataLoader()
        {
            ClassInfo = new Dictionary<ClassTypes, ClassInf>();
            HeroInfo = new Dictionary<HeroTypes, HeroInf>();
            HeroIcon = new Dictionary<HeroTypes, Sprite>();
            ClassIcon = new Dictionary<ClassTypes, Sprite>();
            GenesisIcon = new Dictionary<StartBonusTypes, Sprite>();
            GenesisInfo = new Dictionary<StartBonusTypes, GenesisInf>();
            ClassesStats = new Dictionary<string, GenesisInf>();
        }

		public static ItemInf GetPerkInfo(string iteminf)
        {
                 
			var perkinfo = Resources.Load("Base/Perks/Info/" + iteminf) as TextAsset;
             if(perkinfo == null) return null;

            // ReSharper disable once PossibleNullReferenceException
            var s = perkinfo.text;
            s = s.Replace("<br>", "\n");
			var sls = s.FromJson<ItemInf>();

            return sls;
        }

        public static GenesisInf GetClassStatInfo(string stat)
        {
            GenesisInf classInfo;

            if (stat == "") return new GenesisInf { About = "", Name = "" };

            if (ClassesStats.TryGetValue(stat, out classInfo)) return classInfo;

            var classFullInfoText2 = Resources.Load("Base/Class/ClassInfo/ClassDescs") as TextAsset;

            // ReSharper disable once PossibleNullReferenceException
            var s = classFullInfoText2.text;
            s = s.Replace("<br>", "\n");
            var sls = s.FromJson<ClassesStatClass>();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < sls.Data.Count; i++)
            {
                ClassesStats.Add(sls.Data[i].Name, sls.Data[i]);
            }
            classInfo = ClassesStats[stat];

            return classInfo;
		}   

		public static Sprite GetItemImage(IEntity item)
		{
			
			return null;
		}
		public static ItemInf GetItemInfo(IEntity item){
			
			TextAsset itemText = null;

			

			if (itemText != null) {
				// ReSharper disable once PossibleNullReferenceException
				var s = itemText.text;
				s = s.Replace ("<br>", "\n");
				return s.FromJson<ItemInf> ();
			}
			return new ItemInf { Name = "Not found", About = "Not found"};

		}

	
		public static ItemInf GetTriggerInfo(TriggerTypes trigger)
        {

			ItemInf triggerInfo;

            var classFullInfoText2 = Resources.Load("Base/Triggers/" + trigger) as TextAsset;

            // ReSharper disable once PossibleNullReferenceException
            var s = classFullInfoText2.text;
            s = s.Replace("<br>", "\n");

			triggerInfo = s.FromJson<ItemInf>();

            return triggerInfo;
        }

        public static Sprite LoadWeaponSprite(BaseWeapon w)
        {
            var color = "";
            if (w.EnergyDamageIce > 0 || w.EnergyDamageFire > 0 || w.EnergyDamageElectro > 0)
            {
                color = "." + (w.EnergyDamageIce > 0 ? "b" : "") + (w.EnergyDamageFire > 0 ? "b" : "") +
                        (w.EnergyDamageElectro > 0 ? "b" : "");
            }
            if (string.IsNullOrEmpty(w.Sprite))
            {
                Debug.LogWarning("Sprite for weapon" + w._id + " not found.");
                return null;
            }

            var weaponName = w.Sprite;
            var spriteName = "Base/";
            if (w.WeaponSize != WeaponSizeTypes.Shield)
            {
                spriteName += "Weapons/" + weaponName + "/" + weaponName + "." + ((int) w.Rarety + 1) + color;
            }
            else
            {
                spriteName += "Shields/" + weaponName;
            }

            var s = Resources.Load<Sprite>(spriteName);
            if (s == null)
            {
                Debug.LogWarning("Sprite for " + w.Name + " not found. Expected path: " + spriteName);
            }
            return s;

        }
        public static s_Base.InterfaceClass GetClassInfo(ClassTypes _class)
        {
            ClassInf classInfo;
            var classs = new s_Base.InterfaceClass();
            if (!ClassInfo.TryGetValue(_class, out classInfo))
            {
                var classInfoText = Resources.Load("Base/Class/ClassInfo/" + _class) as TextAsset;


                if (classInfoText != null)
                {
                    var a = classInfoText.ToString();
                    classInfo = JsonUtility.FromJson<ClassInf>(a);
                }
                ClassInfo.Add(_class, classInfo);
            }

            Sprite classIcon;
            if (!ClassIcon.TryGetValue(_class, out classIcon))
            {
                classIcon = Resources.Load<Sprite>("Base/Class/ClassIcon/" + _class);
                ClassIcon.Add(_class, classIcon);
            }

            if (classInfo != null)
            {
                classs.Name = classInfo.Name;
                classs.Role = classInfo.Role;
                classs.Characteristic = classInfo.Characteristic;
                classs.Energy = classInfo.Energy;
                classs.Weapon = classInfo.Weapon;
                classs.Icon = classIcon;
                classs.Class = _class;
                classs.About = classInfo.About;
            }

            return classs;
        }
        public static s_Base.InterfaceGensis GetGenesisInfo(StartBonusTypes startBonus)
        {
            GenesisInf genesisInfo;

            var genesis = new s_Base.InterfaceGensis();

            if (!GenesisInfo.TryGetValue(startBonus, out genesisInfo))
            {
                var classInfoText = Resources.Load("Base/Genesis/GenesisInfo/" + startBonus) as TextAsset;
                if (classInfoText != null)
                {
                    var a = classInfoText.ToString();
                    genesisInfo = JsonUtility.FromJson<GenesisInf>(a);
                }
                GenesisInfo.Add(startBonus, genesisInfo);
            }


            Sprite genesisIcon;
            if (!GenesisIcon.TryGetValue(startBonus, out genesisIcon))
            {
                genesisIcon = Resources.Load<Sprite>("Base/Genesis/GenesisIcon/" + startBonus);
                GenesisIcon.Add(startBonus, genesisIcon);
                //print("get " + classIcon);
            }

            genesis.Genesis = startBonus;
            genesis.Icon = genesisIcon;
            if (genesisInfo != null)
            {
                genesis.Name = genesisInfo.Name;
                genesis.About = genesisInfo.About;
            }

            return genesis;
        }

        public static IEnumerator LoadStreamingAsset(Action<string> a, string path, Action b = null)
        {

            var www = new WWW("file://" + System.IO.Path.Combine(Application.streamingAssetsPath, path));

            while (!www.isDone)
            {

                yield return null;
            }

            if (!string.IsNullOrEmpty(www.error))
            {

                Debug.Log(www.error);
                yield break;
            }
            else
            {
                a(www.text);
            }

            yield return 0;
        }


        public static s_Base.InterfaceHero GetHeroInterface(HeroTypes heroType)
        {
            var hero = new s_Base.InterfaceHero();
            HeroInf heroInfo;
            if (HeroInfo.TryGetValue(heroType, out heroInfo))
            {
            }
            else
            {
                var heroInfoText = Resources.Load("Base/Hero/HeroInfo/" + heroType) as TextAsset;
                var heroInfoText2 = Resources.Load("Base/Hero/HeroInfo/" + heroType + "Disc") as TextAsset;

                if (heroInfoText != null) heroInfo = JsonUtility.FromJson<HeroInf>(heroInfoText.ToString());
                // print(_hero);
                if (heroInfoText2 != null) hero.AboutFull = heroInfoText2.text;
                //  print(heroInfo);
            }

            Sprite heroIcon;

            Sprite heroBoard;
            if (HeroIcon.TryGetValue(heroType, out heroIcon))
            {
            }
            else
                heroIcon = Resources.Load<Sprite>("Base/Hero/HeroIcon/" + heroType);
            if (HeroIcon.TryGetValue(heroType, out heroBoard))
            {
            }
            else
                heroBoard = Resources.Load<Sprite>("Base/Hero/HeroBoard/" + heroType);

            if (heroInfo != null)
            {
                hero.Name = heroInfo.Name;
                hero.Race = heroInfo.Race;
                hero.Armor = heroInfo.Armor;
                hero.Tehnology = heroInfo.Tehnology;
                hero.Ultimate = heroInfo.Ultimate;
                hero.Icon = heroIcon;
                hero.Board = heroBoard;
                hero.About = heroInfo.About;
                hero.Hero = heroType;
                switch (heroInfo.Race)
                {
                    case "Animit":
                        hero.RealRace = RaceTypes.Animit;
                        break;
                    case "Orthoprax":
                        hero.RealRace = RaceTypes.Orthoprax;
                        break;
                    case "Admar":
                        hero.RealRace = RaceTypes.Admar;
                        break;
                    case "Terron":
                        hero.RealRace = RaceTypes.Terron;
                        break;
                }
            }

            return hero;
        }
    }
}
