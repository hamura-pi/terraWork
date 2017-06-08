using UnityEngine;
using Warsmiths.Common.Domain.Enums;

namespace Assets.Scripts
{
    public class ModelChanger : MonoBehaviour
    {

        public enum ArmorType
        {
            Light = 0,
            Medium = 1,
            Heavy = 2
        }
        public static ModelChanger I { get; private set; }

        public GameObject AdmarLight;
        public GameObject AdmarMedium;
        public GameObject AdmarHeavy;

        public GameObject OrtosLight;
        public GameObject OrtosMedium;
        public GameObject OrtosHeavy;

        public GameObject TerronLight;
        public GameObject TerronMedium;
        public GameObject TerronHeavy;

        public GameObject AnimitLight;
        public GameObject AnimitMedium;
        public GameObject AnimitHeavy;

        public void Start()
        {
            I = this;
        }

        public void OnDestroy()
        {
            I = null;
        }

        public void ChangeModel(RaceTypes race, ArmorType armor)
        {
            AdmarLight.SetActive(race == RaceTypes.Admar && armor == ArmorType.Light);
            AdmarMedium.SetActive(race == RaceTypes.Admar && armor == ArmorType.Medium);
            AdmarHeavy.SetActive(race == RaceTypes.Admar && armor == ArmorType.Heavy);

            OrtosLight.SetActive(race == RaceTypes.Orthoprax && armor == ArmorType.Light);
            OrtosMedium.SetActive(race == RaceTypes.Orthoprax && armor == ArmorType.Medium);
            OrtosHeavy.SetActive(race == RaceTypes.Orthoprax && armor == ArmorType.Heavy);

            TerronLight.SetActive(race == RaceTypes.Terron && armor == ArmorType.Light);
            TerronMedium.SetActive(race == RaceTypes.Terron && armor == ArmorType.Medium);
            TerronHeavy.SetActive(race == RaceTypes.Terron && armor == ArmorType.Heavy);

            AnimitLight.SetActive(race == RaceTypes.Animit && armor == ArmorType.Light);
            AnimitMedium.SetActive(race == RaceTypes.Animit && armor == ArmorType.Medium);
            AnimitHeavy.SetActive(race == RaceTypes.Animit && armor == ArmorType.Heavy);
        }
    }
}
