using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Server;
using Newtonsoft.Json;
using UnityEngine;
using Warsmiths.Common.Domain;
using Warsmiths.Common.Domain.Craft.Quest;
using Warsmiths.Common.Domain.Enums;

namespace Assets.Scripts.Common
{
	public abstract class CraftQuestGoals{};

	public class CraftQuestGoalCollectStat : CraftQuestGoals
	{
		public StatTypes Stat;
		public int Amout;
	}

    public delegate void VoidDelegate();
    public delegate void VoidDelegate<T>(T a);
    public delegate void VoidDelegate<T,U>(T a,U b);

    public class s_Base : MonoBehaviour
    {
        public static s_Base I;
        public Action<string> OnReceptDataLoadCompleted;

        public List<BaseReciept> ReceptsList;
        public List<BasePerk> PerkList;
		public TextAsset ReceptData;

        public List<string> GameHints = new List<string>();
    
        public List<HeroTypes> HeroesList;
        public List<Sprite> RatityList;

		public Dictionary<string, CraftQuestGoals> QuestsGoals = new Dictionary<string, CraftQuestGoals>{
			{"Quest4", new CraftQuestGoalCollectStat{ Stat = StatTypes.Durability, Amout = 111}},
			{"Quest5", new CraftQuestGoalCollectStat{ Stat = StatTypes.Durability, Amout = 142}},
		    {"Quest6", new CraftQuestGoalCollectStat{ Stat = StatTypes.Durability, Amout = 155}}
		};


        public class InterfaceHero
        {
            public string Name;
            public RaceTypes RealRace = RaceTypes.Admar;
            public HeroTypes Hero = HeroTypes.Aila;
            public Sprite Icon;
            public Sprite Board;
            public string Race;
            public string Armor;
            public string Tehnology;
            public string Ultimate;
            public string About;
            public string AboutFull;
        }

        public class InterfaceClass
        {
            public string Name;
            public ClassTypes Class = ClassTypes.Alchemist;
            public Sprite Icon;
            public string Role;
            public string Characteristic;
            public string Energy;
            public string Weapon;
            public string About;
            public string AboutFull;
        }

        public class InterfaceGensis
        {
            public string Name;
            public Sprite Icon;
            public StartBonusTypes Genesis = StartBonusTypes.Capsule;
            public string About;
        }
        public class InterfaceTrigger
        {
            public string Name;
            public Sprite Icon;
            public TriggerTypes GriTriggerTypes;
            public string About;
        }

        public string saveid;
        public void Awake()
        {
            I = this;
        }

    
        public static IEnumerator DeleyDelegate(Action ac,float time)
        {
            yield return new WaitForSeconds(time);
            ac.Invoke();
        }

        public void Start()
        {

            if (DoNotDestroyedObjects.I != null)
                DoNotDestroyedObjects.I.Add(gameObject);

			ReceptsList = new List<BaseReciept>();
            PerkList = new List<BasePerk>();
            OnReceptDataLoadCompleted += LoadedData;
            if (ServerController.I != null)
            {
                Init();

                // Game Hints
                LoadGameHints();


                PlayerModel.I.OnUpdateInventory += OnUpdateInventory;
               

            }
            else
            {
                //var domcong = new DomainConfiguration();
                Init();
            }
        }

        private void OnUpdateInventory()
        {
            //
        }

        private void OnServerConfigUpdate()
        {
            //
        }

        public void Update()
        {
            /* if (Input.GetKeyUp(KeyCode.Q))
             {
                 PreCraftController.I.Recept = new BaseQuest() { _id = 1234.ToString() };
                 ServerController.Game.SendSaveReciept(PreCraftController.I.Recept);
             }

             if (Input.GetKeyUp(KeyCode.W))
             {
                 ServerController.Game.SendGetReciept(saveid);
             }
             if (Input.GetKeyUp(KeyCode.U))
             {
                 foreach (var rec in PlayerModel.I.Data.SavedReciepts)
                 {
                     print(rec);
                 }
             }*/
 
        }

        public void Init()
        {
            //   StartCoroutine(DataLoader.LoadStreamingAsset(LoadedData, "Data/Craft/Reciepts/Admar/Reciepts.txt"));
            //    LoadData();

            ServerController.Game.OnGetRecieptResult = (a) =>
            {
                print(a._id + " resoult ");
                saveid = a._id;
            };

            
            ServerController.Game.OnRecieptCreated = (a) =>
            {
                print(a +":" + a._id + " Created ");
                saveid = a._id;
            };
            LoadStsreamAsset("Data/Craft/Reciepts/Admar/Reciepts.txt");
            PerkList.Add(new BasePerk { Name = "Add",Disctiption = "Добавление - Призовите элемент на любую свободную клетку",Number = "A1" });
            PerkList.Add(new BasePerk { Name = "Move", Disctiption = "Сдвиг – Вы можете передвинуть спящий элемент за один молоток",Number = "B1"});
            PerkList.Add(new BasePerk { Name = "Chronograph", Disctiption = "Хронограф – 3 одинаковых по цвету элементов в ряд дают молоток", Number = "C1",  });
            PerkList.Add(new BasePerkStat { Name = "Chip", Number = "D1" });
            PerkList.Add(new BasePerk { Name = "BoostsSlots", Number = "E1" });
            PerkList.Add(new BasePerk { Name = "ForceActivation", Disctiption = "Принудительная активация – Израсходовав 5 молтков, специализированные клетки", Number = "I1" });
            PerkList.Add(new BasePerk { Name = "Lattice", Number = "T1" });
            PerkList.Add(new BasePerk { Name = "BuildZone", Number = "Q1" });
            PerkList.Add(new BasePerk { Name = "AliancePower", Number = "H1" });
            PerkList.Add(new BasePerkStat { Name = "Power1_3", Number = "J1" });
            PerkList.Add(new BasePerkStat { Name = "Power13_15", Number = "J2" });

        
        }

        public void LoadGameHints()
        {
            var hintsTxt = Resources.Load<TextAsset>("Base/Hints/GameHints");
            var lines = hintsTxt.text.Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
                if (lines[i] != "") GameHints.Add(lines[i]);
            }
        }

        public void LoadStsreamAsset(string assetName)
        {
            //var file = Resources.Load<TextAsset>("Reciepts");


            if (OnReceptDataLoadCompleted != null)
            {
                OnReceptDataLoadCompleted(ReceptData.text);
            }
            /*
#if !UNITY_ANDROID || UNITY_EDITOR
            var data = System.IO.File.ReadAllText(file);

            if (OnReceptDataLoadCompleted != null)
            {
                OnReceptDataLoadCompleted(data);
            }
#else
            StartCoroutine(LoadReceptData(file));
#endif*/
        }
        void LoadedData(string a)
        {

            ReceptsList = JsonConvert.DeserializeObject<List<BaseReciept>>(a, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });

            ReceptsList = ReceptsList.OrderByDescending(x => x.LevelRequire).ToList();
            ReceptsList.Reverse();
        }
        public void LoadData()
        {
            StartCoroutine(DataLoader.LoadStreamingAsset(LoadedData, "Data/Craft/Reciepts/Admar/Reciepts.txt"));

        }

        private IEnumerator LoadReceptData(string file)
        {
            if (!file.Contains("://"))
            {
                file = "file://" + file;
            }

            var www = new WWW(file);
            yield return www;
            if (OnReceptDataLoadCompleted != null)
            {
                OnReceptDataLoadCompleted(www.text);
            }
        }
    }
}