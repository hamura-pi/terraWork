using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleType{
	None, Near, Far, Gun, Magic, Sword
};

public enum EnergyType{
	Fire, Ice, Electricity
};

public class GameLogic : MonoBehaviour {
	
	public static GameLogic instance;

	public Text Info1;
	public Text Info2;
	public Text line1;
	public Text line2;
	[HideInInspector]
	public bool endRound;
	[HideInInspector]
	public float playerDOF;
	private float counter;
	private float timePrevValue;

	private bool startFade;
	private Color red = new Color (1, 0.18f, 0.18f);
	private Color blue = new Color (0.38f, 0.67f, 1);
	private Color endColor = new Color(0.5f, 0.5f, 0.5f, 0);

	void Awake(){
		instance = this;
		GameObject go = GameObject.FindGameObjectWithTag ("Player");
		PlayerAvatar pa = go.GetComponent<PlayerAvatar> ();
		playerDOF = pa.battleGun.distance;
	}

	void Update(){
		if (!startFade)	return;
		counter += Time.timeScale/100f;
		if (counter >= 1) startFade = false;
		line1.color = Color.Lerp (red, endColor, counter);
		line2.color = Color.Lerp (blue, endColor, counter);
	}

	public void Popravka(PlayerAvatar attackPlayer, PlayerAvatar protectPlayer, string bodyPart){		
//		string s1, s2;
//		s1 = attackPlayer.name + " - ATAKA";
//		s2 = protectPlayer.name + " - УРОН";
		//Находим диапазон рандома
		int randomRange = (attackPlayer.profile.attack + protectPlayer.profile.protect)/2;
		//Модификатор атаки
		int mod = Mathf.Abs (attackPlayer.profile.attack - protectPlayer.profile.protect);
		//Узнаем максимальную атаку и находим игрока с этой атакой
		int max = Mathf.Max (attackPlayer.profile.attack, protectPlayer.profile.protect);
		int mod1 = 0;
		int mod2 = 0;
		if (max == attackPlayer.profile.attack)
			mod1 = mod;
		if (max == protectPlayer.profile.protect)
			mod2 = mod;
		//Кидаем кости и прибавляем модификаторы
		int attackDice = attackPlayer.DropDice (randomRange) + mod1;
		int protectDice = protectPlayer.DropDice (randomRange) + mod2;

		if (attackDice > protectDice) {
			attackPlayer.PowerUp (true, false);
			attackPlayer.AddFury ();
			protectPlayer.Damage (bodyPart, attackPlayer);
		} else if (attackDice < protectDice) {
			//2 игрок сумел защититься
			attackPlayer.Attack (false);
			protectPlayer.PowerUp (false, true);
			protectPlayer.Guard (attackPlayer.profile.multiplier);
			protectPlayer.Recover (attackPlayer.profile.damage);
//			if (protectPlayer == player1)
//				s1 = "ЗАЩИТА";
//			else
//				s2 = "ЗАЩИТА";
		} else {
			if (Random.value > 0.5f) {
				attackPlayer.PowerUp (true, false);
				attackPlayer.AddFury ();
				protectPlayer.Damage (bodyPart, attackPlayer);
			} else {
				//2 игрок сумел защититься
				attackPlayer.Attack (false);
				protectPlayer.PowerUp (false, true);
				protectPlayer.Guard (attackPlayer.profile.multiplier);
				protectPlayer.Recover (attackPlayer.profile.damage);
//				if (protectPlayer == player1)
//					s1 = "ЗАЩИТА";
//				else
//					s2 = "ЗАЩИТА";
			}
		}
		SetLine1Text("АТАКА: <b>" + attackDice.ToString() + "</b>" );
		SetLine2Text("ЗАЩИТА: <b>" + protectDice.ToString() + "</b>" );
//		Info1.text = s1 + "                                             " + s2;
	}

	public void SliderTime(float value){
		Time.timeScale = value;
	}

	public void Pause(bool state){
		startFade = !state;
		if (state) {
			timePrevValue = Time.timeScale;
			Time.timeScale = 0;
		}else{
			Time.timeScale = timePrevValue;
		}
	}

	public void ButtonReset ()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	void SetLine1Text(string t){
		line1.text = t;
		line1.color = new Color (1, 0.18f, 0.18f);
	}

	void SetLine2Text(string t){
		line2.text = t;
		line2.color = new Color (0.38f, 0.67f, 1);
		counter = 0;
		startFade = true;
	}

	public void EndRound(PlayerAvatar initiator, PlayerAvatar opponent){
		if (endRound)
			return;
		endRound = true;

		string bodyPart = initiator.GetEnemySide (opponent.transform);
		if (opponent.fullEnergy && opponent.inView) {
			opponent.SetEnergy (false);
			Info2.text = "<color=cyan>Формула</color>";
			Formula (initiator, opponent, bodyPart);
		} else {
			Info2.text = "<color=yellow>Поправка</color>";
			Popravka (initiator, opponent, bodyPart);
		}
	}

	void Formula(PlayerAvatar player1, PlayerAvatar player2, string bodyPart){
		PlayerAvatar attackWinner = GetAttackWinner (player1, player2);
		PlayerAvatar protectWinner = GetProtectWinner (player1, player2);
		if (attackWinner.IsAttack ()) {
			SetLine1Text ("");
			SetLine2Text ("");
			return;
		}
		//Вариант А:
		if (attackWinner == player1 && protectWinner == player1) {
			player1.PowerUp (true, true);
			player1.AddFury ();
			player1.Recover (player2.profile.damage);
			player2.Damage (bodyPart, player1);
			player2.SetEnergy(false);
			Info1.text = "АТАКА                                                УРОН";
			return;
		}
		//		return;
		if (attackWinner == player2 && protectWinner == player2) {
			player2.PowerUp (true, true);
			player2.AddFury ();
			player2.Recover (player1.profile.damage);
			player1.Damage (bodyPart, player2);
			player1.SetEnergy(false);
			Info1.text = "УРОН                                                АТАКА";
			return;
		}
		//Вариант Б:
		if (attackWinner == player1 && protectWinner == player2) {
			if (player2.protectPriority == player1.attackPriority) {
				if (Random.value > 0.5f)
					player2.SetProtectPriority (1000);
				else
					player2.SetProtectPriority (0);	
			}
			if (player2.protectPriority > player1.attackPriority) {
				player1.Attack (false);
				player1.AddFury ();
				player2.PowerUp (false, true);
				player2.Guard (player1.profile.multiplier);
				player2.Recover (player1.profile.damage, true);
				Info1.text = "АТАКА                                             ЗАЩИТА";
			} else {
				player1.AddFury ();
				player1.PowerUp (true, false);
				player2.Damage (bodyPart, player1);
				player2.AddMoral (player1.profile.multiplier);
				Info1.text = "АТАКА                                               УРОН";
			}
			player2.SetEnergy(false);
			return;
		}
		if (attackWinner == player2 && protectWinner == player1) {
			//			print (player1.protectPriority + " " + player2.attackPriority);
			if (player1.protectPriority == player2.attackPriority) {
				if (Random.value > 0.5f)
					player1.SetProtectPriority (1000);
				else
					player1.SetProtectPriority (0);	
			}
			if (player1.protectPriority > player2.attackPriority) {
				player2.Attack (false);
				player2.AddFury ();
				player1.PowerUp (false, true);
				player1.Guard (player2.profile.multiplier);
				player1.Recover (player2.profile.damage, true);
				Info1.text = "ЗАЩИТА                                               АТАКА";
			} else {
				player2.AddFury ();
				player2.PowerUp (true, false);
				player1.Damage (bodyPart, player2);
				player1.AddMoral (player2.profile.multiplier);
				Info1.text = "УРОН                                                 АТАКА";
			}				
			player2.SetEnergy(false);
			return;
		}
		Debug.LogError ("Ошибка формулы");
	}

	PlayerAvatar GetAttackWinner(PlayerAvatar player1, PlayerAvatar player2){
		int p1 = player1.profile.attack;
		int p2 = player2.profile.attack;
		//Находим диапазон рандома 
		int randomRange = (p1 + p2)/2;
		//Модификатор атаки
		int mod = p1 > p2 ? p1 - p2 : p2 - p1;
		//Узнаем максимальную атаку и находим игрока с этой атакой
		int max = Mathf.Max (p1, p2);
		int mod1 = 0;
		int mod2 = 0;
		if (max == p1)
			mod1 = mod;
		if (max == p2)
			mod2 = mod;
		//Кидаем кости и прибавляем модификаторы
		int p1Dice = player1.DropDice (randomRange) + mod1;
		int p2Dice = player2.DropDice (randomRange) + mod2;
		int priority = p1Dice > p2Dice ? p1Dice - p2Dice : p2Dice - p1Dice;
		if (p1Dice > p2Dice) {
			player1.SetAttackPriority (priority);
			SetLine1Text("АТАКА (P1): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + " (" + priority + ")</b>" );
			return player1;
		} else if (p1Dice < p2Dice) {
			player2.SetAttackPriority (priority);
			SetLine1Text("АТАКА (P2): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + " (" + priority + ")</b>");
			return player2;
		} else {
			if (Random.value > 0.5f) {
				player1.SetAttackPriority (priority);
				SetLine1Text("АТАКА (P1): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + "</b> (Random)");
				return player1;
			} else {
				player2.SetAttackPriority (priority);
				SetLine1Text("АТАКА (P2): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + "</b> (Random)");
				return player2;
			}
		}
	}

	PlayerAvatar GetProtectWinner(PlayerAvatar player1, PlayerAvatar player2){
		int p1 = player1.profile.protect;
		int p2 = player2.profile.protect;
		//Находим диапазон рандома 
		int randomRange = (p1 + p2)/2;
		//Модификатор атаки
		int mod = p1 > p2 ? p1 - p2 : p2 - p1;
		//Узнаем максимальную атаку и находим игрока с этой атакой
		int max = Mathf.Max (p1, p2);
		int mod1 = 0;
		int mod2 = 0;
		if (max == p1)
			mod1 = mod;
		if (max == p2)
			mod2 = mod;
		//Кидаем кости и прибавляем модификаторы
		int p1Dice = player1.DropDice (randomRange) + mod1;
		int p2Dice = player2.DropDice (randomRange) + mod2;
		//		SetLine2Text("ЗАЩИТА: <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + "</b>");
		int priority = p1Dice > p2Dice ? p1Dice - p2Dice : p2Dice - p1Dice;
		if (p1Dice > p2Dice) {
			player1.SetProtectPriority(priority);
			SetLine2Text("ЗАЩИТА (P1): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + " (" + priority + ")</b>");
			return player1;
		} else 
			if (p1Dice < p2Dice) {
				player2.SetProtectPriority (priority);
				SetLine2Text("ЗАЩИТА (P2): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + " (" + priority + ")</b>");
				return player2;
			} else {
				if (Random.value > 0.5f) {
					player1.SetProtectPriority (priority);
					SetLine2Text("ЗАЩИТА (P1): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + "</b> (Random)");
					return player1;
				} else {
					player2.SetProtectPriority (priority);
					SetLine2Text("ЗАЩИТА (P2): <b>" + p1Dice.ToString() + "</b> vs <b>" + p2Dice.ToString() + "</b> (Random)");
					return player2;
				}
			}
	}
}
