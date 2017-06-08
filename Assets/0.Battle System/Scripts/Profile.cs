using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour {

	public float damage;	//Урон
	public float distance;	//Дистанция
	public float distanceNear;	//Ближняя дистанция
	public float angle;	//Угол атаки
	public int attack;	//Атака
	public int protect;	//Защита
	public int zalp;		//Залп (дальний бой)
	public int oboima;		//Обойма (дальний бой)
	public float radius;	//Радиус разброса выстрела (дальний бой)

	public float crit;			// Крит сила
	public int critPercent;	// Процент крита
	public float reversal;		// Реверсал сила
	public int reversalPercent;	// Процент реверсала
	public float max;			// Максимальное восстановление за раунд
	public float speed;		// Скорость атаки
	public float multiplier;		// Значение прироста к энергиям

	public PlayerAvatar player;

}
