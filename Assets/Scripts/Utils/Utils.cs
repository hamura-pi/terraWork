using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;
using Warsmiths.Common.Domain;
using Warsmiths.Common.Domain.Enums;
using Warsmiths.Common.Domain.Equipment;
using Random = System.Random;
// ReSharper disable InconsistentNaming
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.Utils
{
    public class EqDif
    {
        public float Anomality;
        public int Weight;

        public float AttackSpeed;
        public int AttackAngle;
        public int Durability;

        public float BaseDamage;
        public int Sharpering;

        public float MiliDamage;
        public float RangeDamage;
        public float MagicDamage;

        public int AttackArea;

        public int DamageRadius;
        public int Accuracy;
        public int DisruptionMod;
        public int CriticalMod;
        public int ShotsInLine;
        public int Holder;
        public int Strength;

        public int CriticalBlast;
        public int MaxTargets;
        //public int ChargeVelocity;
        public int Charger;

        public int FireShiled;
        public int IceShield;
        public int ElecticityShield;

        public int[] Chest;
        public int[] Back;
        public int[] LeftHand;
        public int[] RightHand;
        public int[] LeftLeg;
        public int[] RightLeg;

        public int OverallTriggers;
        public int OverallModules;
        public int OverallUpgades;
    }

    public static class Utils
    {
        public static T FromBson<T>(this byte[] data)
        {
            var stream = new MemoryStream(data);
            stream.Seek(0, SeekOrigin.Begin);
            var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Objects };
            serializer.Converters.Add(new WarsmithsClientConverter());
            var reader = new BsonReader(stream);
            return serializer.Deserialize<T>(reader);

        }

        public static byte[] ToBson(this object t)
        {
            var stream = new MemoryStream();

            var serializer = new JsonSerializer();
            var writer = new BsonWriter(stream);
            serializer.Serialize(writer, t);
            return stream.ToArray();
        }

        public static T FromJson<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string ToJson(this object t)
        {
            return JsonConvert.SerializeObject(t);
        }

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        private static EqDif FillEqDif(BaseWeapon e, Character c)
        {
            var result = new EqDif
            {
                Anomality = (float)Math.Round(e.GetTotalAnomality(c), 1),
                Weight = Mathf.CeilToInt(e.Weight),

                AttackSpeed = (float)Math.Round(e.AttackSpeed, 2),
                AttackArea = Mathf.CeilToInt(e.AttackArea),

                BaseDamage = (float)Math.Round(e.BaseDamage, 1),
                Sharpering = Mathf.CeilToInt(e.Sharpening),

                MiliDamage = (float)Math.Round(e.GetMeleeDamage(c), 1),
                RangeDamage = (float)Math.Round(e.GetRangeDamage(c), 1),
                MagicDamage = (float)Math.Round(e.GetMagicDamage(c), 1),

                DamageRadius = Mathf.CeilToInt(e.DamageRadius),
                Accuracy = Mathf.CeilToInt(e.Accuracy),
                DisruptionMod = Mathf.CeilToInt(e.DisruptionModifier),
                CriticalMod = Mathf.CeilToInt(e.CriticalModifier) * 100,
                ShotsInLine = Mathf.CeilToInt(e.ShotsInLine),
                Holder = Mathf.CeilToInt(e.Holder),
                Strength = Mathf.CeilToInt(e.Strength),

                CriticalBlast = Mathf.CeilToInt(e.CriticalBlast),
                MaxTargets = Mathf.CeilToInt(e.MaxTargets),
                //ChargeVelocity = Mathf.CeilToInt(e.ChargeVelocity), 
                Charger = Mathf.CeilToInt(e.Charger),

                Chest = new[] { 0, 0, 0 },
                Back = new[] { 0, 0, 0 },
                LeftHand = new[] { 0, 0, 0 },
                RightHand = new[] { 0, 0, 0 },
                LeftLeg = new[] { 0, 0, 0 },
                RightLeg = new[] { 0, 0, 0 },
            };
            return result;
        }

        private static EqDif FillEqDif(BaseArmor e)
        {
            var result = new EqDif
            {
                Anomality = (float)Math.Round(e.Anomality, 1),
                Weight = Mathf.CeilToInt(e.Weight),
                Durability = Mathf.CeilToInt(e.Durability),
                AttackAngle = Mathf.CeilToInt(e.AttackAngle),

                Sharpering = Mathf.CeilToInt(e.Sharpening),

                FireShiled = Mathf.CeilToInt(e.FireShield),
                IceShield = Mathf.CeilToInt(e.IceShield),
                ElecticityShield = Mathf.CeilToInt(e.ElectricityShield),

                Chest = new[] { 0, 0, 0 },
                Back = new[] { 0, 0, 0 }, 
                LeftHand = new[] { 0, 0, 0 }, 
                RightHand = new[] { 0, 0, 0 }, 
                LeftLeg = new[] { 0, 0, 0 }, 
                RightLeg = new[] { 0, 0, 0 }, 

                OverallTriggers = Mathf.CeilToInt(e.OverallTriggers),
                OverallModules = Mathf.CeilToInt(e.ModulesCount),
                OverallUpgades = Mathf.CeilToInt(e.OverallUpgrades),
            };

            var ap = e.ArmorParts.FirstOrDefault(x => x.ArmorPartType == ArmorPartTypes.Back);
            if (ap != null)
            {
                result.Back[0] = ap.Durability;
                result.Back[1] = ap.Casing;
                result.Back[2] = ap.MaxModulesCount;
            }

            ap = e.ArmorParts.FirstOrDefault(x => x.ArmorPartType == ArmorPartTypes.Chest);
            if (ap != null)
            {
                result.Chest[0] = ap.Durability;
                result.Chest[1] = ap.Casing;
                result.Chest[2] = ap.MaxModulesCount;
            }

            ap = e.ArmorParts.FirstOrDefault(x => x.ArmorPartType == ArmorPartTypes.LeftHand);
            if (ap != null)
            {
                result.LeftHand[0] = ap.Durability;
                result.LeftHand[1] = ap.Casing;
                result.LeftHand[2] = ap.MaxModulesCount;
            }

            ap = e.ArmorParts.FirstOrDefault(x => x.ArmorPartType == ArmorPartTypes.RightHand);
            if (ap != null)
            {
                result.RightHand[0] = ap.Durability;
                result.RightHand[1] = ap.Casing;
                result.RightHand[2] = ap.MaxModulesCount;
            }

            ap = e.ArmorParts.FirstOrDefault(x => x.ArmorPartType == ArmorPartTypes.LeftLeg);
            if (ap != null)
            {
                result.LeftLeg[0] = ap.Durability;
                result.LeftLeg[1] = ap.Casing;
                result.LeftLeg[2] = ap.MaxModulesCount;
            }

            ap = e.ArmorParts.FirstOrDefault(x => x.ArmorPartType == ArmorPartTypes.RightHand);
            if (ap != null)
            {
                result.RightHand[0] = ap.Durability;
                result.RightHand[1] = ap.Casing;
                result.RightHand[2] = ap.MaxModulesCount;
            }
            return result;
        }

        private static int EqDifPercent(float value1, float value2)
        {
            var min = Math.Min(value1, value2);
            var max = Math.Max(value1, value2);
            return Mathf.RoundToInt(Math.Abs(max) < 0.0000001f ? 0 : 100 - 100f * min / max);
        }

        public static List<EqDif> Compare(this BaseEquipment source, BaseEquipment dest, Character c)
        {
            var result = new List<EqDif>();

            EqDif src;
            EqDif dst;

            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (source is BaseArmor)
            {
                src = FillEqDif((BaseArmor)source);
                dst = FillEqDif((BaseArmor)dest);
            }
            else
            {
                src = FillEqDif((BaseWeapon)source, c);
                dst = FillEqDif((BaseWeapon)dest, c);
            }

            var diff = new EqDif
            {
                Anomality = EqDifPercent(src.Anomality, dst.Anomality),
                DisruptionMod = EqDifPercent(src.DisruptionMod, dst.DisruptionMod),
                Holder = EqDifPercent(src.Holder, dst.Holder),
                Durability = EqDifPercent(src.Durability, dst.Durability),
                BaseDamage = EqDifPercent(src.BaseDamage, dst.BaseDamage),
                Weight = EqDifPercent(src.Weight, dst.Weight),
                Accuracy = EqDifPercent(src.Accuracy, dst.Accuracy),
                AttackAngle = EqDifPercent(src.AttackAngle, dst.AttackAngle),
                AttackSpeed = EqDifPercent(src.AttackSpeed, dst.AttackSpeed),
                //ChargeVelocity = EqDifPercent(src.ChargeVelocity, dst.ChargeVelocity),
                Charger = EqDifPercent(src.Charger, dst.Charger),
                CriticalBlast = EqDifPercent(src.CriticalBlast, dst.CriticalBlast),
                CriticalMod = EqDifPercent(src.CriticalMod, dst.CriticalMod),
                DamageRadius = EqDifPercent(src.DamageRadius, dst.DamageRadius),
                ElecticityShield = EqDifPercent(src.ElecticityShield, dst.ElecticityShield),
                FireShiled = EqDifPercent(src.FireShiled, dst.FireShiled),
                IceShield = EqDifPercent(src.IceShield, dst.IceShield),
                MaxTargets = EqDifPercent(src.MaxTargets, dst.MaxTargets),
                MiliDamage = EqDifPercent(src.MiliDamage, dst.MiliDamage),
                RangeDamage = EqDifPercent(src.RangeDamage, dst.RangeDamage),
                MagicDamage = EqDifPercent(src.MagicDamage, dst.MagicDamage),
                OverallModules = EqDifPercent(src.OverallModules, dst.OverallModules),
                OverallTriggers = EqDifPercent(src.OverallTriggers, dst.OverallTriggers),
                OverallUpgades = EqDifPercent(src.OverallUpgades, dst.OverallUpgades),
                Sharpering = EqDifPercent(src.Sharpering, dst.Sharpering),
                ShotsInLine = EqDifPercent(src.ShotsInLine, dst.ShotsInLine),
                Strength = EqDifPercent(src.Strength, dst.Strength),
                Chest = new[] { EqDifPercent(src.Chest[0], dst.Chest[0]), 0, 0 },
                RightHand = new[] { EqDifPercent(src.RightHand[0], dst.RightHand[0]), 0, 0 },
                LeftLeg = new[] { EqDifPercent(src.LeftLeg[0], dst.LeftLeg[0]), 0, 0 },
                RightLeg = new[] { EqDifPercent(src.RightLeg[0], dst.RightLeg[0]), 0, 0 },
                Back = new[] { EqDifPercent(src.Back[0], dst.Back[0]), 0, 0 },
                LeftHand = new[] { EqDifPercent(src.LeftHand[0], dst.LeftHand[0]), 0, 0 }
            };

            result.Add(src);
            result.Add(dst);
            result.Add(diff);

            return result;
        }

        private static readonly Random Rng = new Random((int)DateTime.Now.Ticks);

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
        public static void Fade(this GameObject root, float from, float to, float time, float delay = 0, Action onFinished = null)
        {
            var ims = root.GetComponentsInChildren<Graphic>(true);
            for (var i = 0; i < ims.Length; i++)
            {
                var im = ims[i];
                im.color = new Color(im.color.r, im.color.g, im.color.b, from > -1 ? from : im.color.a);
            }

            Tweener tw = null;
            for (var i = 0; i < ims.Length; i++)
            {
                var im = ims[i];
                tw = im.DOFade(to, time);
                if (delay > 0) tw.SetDelay(delay);
            }
            if (tw != null)
            {
                tw.OnComplete(() =>
                {
                    if (onFinished != null) onFinished();
                });
            }
            else
            {
                if (onFinished != null) onFinished();
            }
        }

        public static void FadeSimple(this GameObject target, float from, float to, float time, float delay = 0, Action onFinished = null)
        {
            var ims = target.GetComponents<Graphic>();
            for (var i = 0; i < ims.Length; i++)
            {
                var im = ims[i];
                im.color = new Color(im.color.r, im.color.g, im.color.b, from);
            }

            Tweener tw = null;
            for (var i = 0; i < ims.Length; i++)
            {
                var im = ims[i];
                tw = im.DOFade(to, time);
                if (delay > 0) tw.SetDelay(delay);
            }
            if (tw != null)
            {
                tw.OnComplete(() =>
                {
                    if (onFinished != null) onFinished();
                });
            }
            else
            {
                if (onFinished != null) onFinished();
            }
        }

        public static float WeaponDamage(this BaseWeapon weapon, Character c)
        {
            return weapon.WeaponType == WeaponTypes.Melee
                ? weapon.GetMeleeDamage(c)
                : weapon.WeaponType == WeaponTypes.Ranged ? weapon.GetRangeDamage(c) : weapon.GetMagicDamage(c);
        }

        public static string StructureDescription(this StructureTypes a)
        {
            switch (a)
            {
                case StructureTypes.Metall:
                    return "Металл - Регенерация энергорезистов  на 30% быстрее, а энерго урона на 30% больше";
                case StructureTypes.Plastic:
                    return "Пластик - Прочность допеха меньше на 25%, а все физики получают +10%";
                case StructureTypes.Crystal:
                    return "Кристалл - Доспех поглащает  урон в размере 3% от своей прочности, любой урон который проходит поглощение, больше на 25%";
                case StructureTypes.Setaplastic:
                    return "Металлопластик - регенирует утраченную прочность на 1 процент в 2 раунда, но от победы в защите востанавливаеться нае 70 а 50% от урона оружия противника";
                case StructureTypes.Crystometall:
                    return "Металлокристал - воращает 20% принятого урона противнику в ближнем бою, урон от бафов ниже на 50%";
                case StructureTypes.CrystoPlastic:
                    return "Кристаллопластик- удары противника поадают в менне всего поврежеднную часть доспеха из возможных, но сопративление на 25 процентов менее эффективно";
                case StructureTypes.Dark:
                    return "Тень - негативные навыки противников не действуют на вас. Противники не видят вас в своей переферийной зоне. Сила бафа от дополнительной энергии в 2 раза больше. уклонение на урон 50%";
                case StructureTypes.Light:
                    return "Cияние - сила приемов эффективнее на 25%, нет утечки энергии, но имуны на метки не копяться";
                default:
                    return "";
            }
        }
    }
}
