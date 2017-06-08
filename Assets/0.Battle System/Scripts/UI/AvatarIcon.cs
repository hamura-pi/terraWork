using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Avatar{
	public string Name;
	public AvatarPart part;
}

public class AvatarIcon : MonoBehaviour {

	public PlayerAvatar player;
	public Avatar[] parts;

	void Start(){
		float totalHP = 0;
		foreach (Avatar a in parts)
			totalHP += a.part.hp;

		player.armor.SetMaximum ((int)totalHP);
	}

	public float ShowDamage (string bodyPart, float damage, PlayerAvatar enemy) {
		if (bodyPart == "Front")
			return parts[0].part.ShowDamage (damage, enemy);
		else if (bodyPart == "Back")			
			return parts[1].part.ShowDamage (damage, enemy);
		else if (bodyPart == "LeftArm")			
			return parts[2].part.ShowDamage (damage, enemy);
		else if (bodyPart == "RightArm")			
			return parts[3].part.ShowDamage (damage, enemy);
		else if (bodyPart == "LeftLeg")			
			return parts[4].part.ShowDamage (damage, enemy);
		else if (bodyPart == "RightLeg")	
			return parts[5].part.ShowDamage (damage, enemy);
		return damage;
	}

	public void Recover(float amount){
		List<int> damaged = new List<int> (){1,2,3,4,5,6};	//Массив с номером части
		List<int> tmp = new List<int> ();
		int r = 0;
//		int error = 0;										// для отладки
		while (true) {
			r = Random.Range (0, damaged.Count);			//Берем случайную часть доспеха
			damaged [r] = 0;								//Исключаем ее из повторного поиска
			if (parts[r].part.Damaged ()) {					//Если у доспеха есть урон, 
				if (parts[r].part.Recover (amount) == 0)	//Восстанавлиаем и смотрим на остаток от восстановления,
					return;									//Если остаток есть переносим его на следующую часть доспеха
			}

			foreach (int i in damaged) {					//Заполняем новый массив исключая текущее значение рандома
				if (i != 0)
					tmp.Add (i);
				if (tmp.Count == 0)							//Если массив пустой - выход
					return;
			}

			damaged = new List<int>(tmp);
			tmp.Clear ();

//			error++;
//			if (error > 7) {
//				Debug.LogError ("Overflow");
//				return;
//			}
		}
	}

	public void Reset(){
		parts[0].part.Reset ();
		parts[1].part.Reset ();
		parts[2].part.Reset ();
		parts[3].part.Reset ();
		parts[4].part.Reset ();
		parts[5].part.Reset ();
	}
}
