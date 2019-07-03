using UnityEngine;
using System.Collections;

namespace RPGCharacterAnims{
	
	public enum Weapon{
		UNARMED = 0,
		TWOHANDSWORD = 1,
		TWOHANDSPEAR = 2,
		TWOHANDAXE = 3,
		TWOHANDBOW = 4,
		TWOHANDCROSSBOW = 5,
		STAFF = 6,
		ARMED = 7,
		RELAX = 8,
		RIFLE = 9,
		SHIELD = 11,
		ARMEDSHIELD = 12
	}
		
	public class RPGCharacterWeaponController : MonoBehaviour{
		RPGCharacterController rpgCharacterController;
		private Animator animator;
		
		//Weapon Parameters.
		[HideInInspector] public int rightWeapon = 0;
		[HideInInspector] public int leftWeapon = 0;
		[HideInInspector] public bool isSwitchingFinished = true;
		[HideInInspector] public bool isWeaponSwitching = false;
		[HideInInspector] public bool instantWeaponSwitch;
		[HideInInspector] public bool dualSwitch;
		
		//Weapon Models.
		public GameObject twoHandAxe;
		public GameObject twoHandSword;
		public GameObject twoHandSpear;
		public GameObject twoHandBow;
		public GameObject twoHandCrossbow;
		public GameObject staff;
		public GameObject swordL;
		public GameObject swordR;
		public GameObject maceL;
		public GameObject maceR;
		public GameObject daggerL;
		public GameObject daggerR;
		public GameObject itemL;
		public GameObject itemR;
		public GameObject shield;
		public GameObject pistolL;
		public GameObject pistolR;
		public GameObject rifle;
		public GameObject spear;

		void Awake(){
			rpgCharacterController = GetComponent<RPGCharacterController>();
			//Find the Animator component.
			animator = GetComponentInChildren<Animator>();
			StartCoroutine(_HideAllWeapons(false, false));
		}

		//weaponNumber -1 = Relax
		//weaponNumber 0 = Unarmed
		//weaponNumber 1 = 2H Sword
		//weaponNumber 2 = 2H Spear
		//weaponNumber 3 = 2H Axe
		//weaponNumber 4 = 2H Bow
		//weaponNumber 5 = 2H Crowwbow
		//weaponNumber 6 = 2H Staff
		//weaponNumber 7 = Shield
		//weaponNumber 8 = L Sword
		//weaponNumber 9 = R Sword
		//weaponNumber 10 = L Mace
		//weaponNumber 11 = R Mace
		//weaponNumber 12 = L Dagger
		//weaponNumber 13 = R Dagger
		//weaponNumber 14 = L Item
		//weaponNumber 15 = R Item
		//weaponNumber 16 = L Pistol
		//weaponNumber 17 = R Pistol
		//weaponNumber 18 = Rifle
		//weaponNumber 19 == Right Spear
		public IEnumerator _SwitchWeapon(int weaponNumber){
//			Debug.Log("Switch Weapon: " + weaponNumber);
			if(instantWeaponSwitch){
				StartCoroutine(_InstantWeaponSwitch(weaponNumber));
				yield break;
			}
			//If is Unarmed/Relax.
			if(IsNoWeapon(animator.GetInteger("Weapon"))){
				//Switch to Relax.
				if(weaponNumber == -1){
					StartCoroutine(_SheathWeapon(0, -1));
				}
				//Switch to Unarmed.
				else{
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
			}
			//Character has 2handed weapon.
			else if(Is2HandedWeapon(animator.GetInteger("Weapon"))){
				StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
				yield return new WaitForSeconds(1.2f);
				//Switching to weapon.
				if(weaponNumber > 0){
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
			}
			//Character has 1handed weapon(s).
			else if(Is1HandedWeapon(animator.GetInteger("Weapon"))){
				//Dual switching with dual wielding.
				if(dualSwitch && leftWeapon != 0 && rightWeapon != 0){
					StartCoroutine(_DualSheath(animator.GetInteger("Weapon"), weaponNumber));
					yield return new WaitForSeconds(1f);
					StartCoroutine(_UnSheathWeapon(weaponNumber));
					yield break;
				}
				//Character is switching to 2handed weapon or Unarmed or Relax, put put away all weapons.
				if(Is2HandedWeapon(weaponNumber) || IsNoWeapon(weaponNumber)){
					//Left hand has a weapon.
					if(leftWeapon != 0){
						StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					//Right hand has a weapon.
					if(rightWeapon != 0){
						StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					if(weaponNumber > 0){
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
				//Switching left weapon, put away left weapon if equipped.
				else if(IsLeftWeapon(weaponNumber)){
					if(leftWeapon > 0){
						StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
				//Switching right weapon, put away right weapon if equipped
				else if(IsRightWeapon(weaponNumber)){
					if(leftWeapon > 0 && dualSwitch){
						StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
						StartCoroutine(_DualUnSheath(weaponNumber));
						yield break;
					}
					if(rightWeapon > 0){
						StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
			}
			yield return null;
		}


		public IEnumerator _UnSheathWeapon(int weaponNumber){
//			Debug.Log("UnsheathWeapon: " + weaponNumber);
			isWeaponSwitching = true;
			//Use Dual switch.
			if(dualSwitch){
				StartCoroutine(_DualUnSheath(weaponNumber));
				yield break;
			}
			//Switching to Unarmed from Relax.
			if(weaponNumber == 0){
				DoWeaponSwitch(-1, -1, -1, 0, false);
				yield return new WaitForSeconds(0.75f);
				SetAnimator(0, -2, 0, 0, 0);
			}
			//Switching to 2handed weapon.
			else if(Is2HandedWeapon(weaponNumber)){
				//Switching from 2handed weapon.
				if(Is2HandedWeapon(animator.GetInteger("Weapon"))){
					DoWeaponSwitch(0, weaponNumber, weaponNumber, -1, false);
					yield return new WaitForSeconds(0.75f);
					SetAnimator(weaponNumber, -2, animator.GetInteger("Weapon"), -1, -1);
				}
				else{
					DoWeaponSwitch(animator.GetInteger("Weapon"), weaponNumber, weaponNumber, -1, false);
					yield return new WaitForSeconds(0.75f);
					SetAnimator(weaponNumber, -2, weaponNumber, -1, -1);
				}
			}
			//Switching to 1handed weapons.
			else{
				//If switching from Unarmed or Relax.
				if(IsNoWeapon(animator.GetInteger("Weapon"))){
					animator.SetInteger("WeaponSwitch", 7);
				}
				//Left hand weapons.
				if(weaponNumber == 7 || weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16){
					//If not switching Shield.
					if(weaponNumber == 7){
						animator.SetBool("Shield", true);
					}
					DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 1, false);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(7, 7, weaponNumber, -1, 1);
				}
				//Right hand weapons.
				else if(weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19){
					animator.SetBool("Shield", false);
					DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 2, false);
					yield return new WaitForSeconds(0.5f);
					if(leftWeapon == 7){
						animator.SetBool("Shield", true);
					}
					SetAnimator(7, 7, -1, weaponNumber, 2);
				}
			}
//			rpgCharacterController.SetWeaponState(weaponNumber);
			yield return null;
		}

		public IEnumerator _SheathWeapon(int weaponNumber, int weaponTo){
//			Debug.Log("Sheath Weapon: " + weaponNumber + " - Weapon To: " + weaponTo);
			//Reset for animation events.
			isWeaponSwitching = true;
			//Use Dual switch.
			if(dualSwitch){
				StartCoroutine(_DualSheath(weaponNumber, weaponTo));
				yield break;
			}
			//Set LeftRight hand for 1handed switching.
			if(IsLeftWeapon(weaponNumber)){
				animator.SetInteger("LeftRight", 1);
			}
			else if(IsRightWeapon(weaponNumber)){
				animator.SetInteger("LeftRight", 2);
			}
			//Switching to Unarmed or Relaxed.
			if(weaponTo < 1){
				//Have at least 1 weapon.
				if(rightWeapon != 0 || leftWeapon != 0){
					//Sheath 1handed weapon.
					if(Is1HandedWeapon(weaponNumber)){
						//If sheathing both weapons, go to Armed first.
						if(rightWeapon != 0 && leftWeapon != 0){
							DoWeaponSwitch(7, weaponNumber, 7, -1, true);
						}
						else{
							DoWeaponSwitch(weaponTo, weaponNumber, 7, -1, true);
						}
						yield return new WaitForSeconds(0.5f);
						if(IsLeftWeapon(weaponNumber)){
							animator.SetInteger("LeftWeapon", 0);
							SetAnimator(weaponTo, -2, 0, -1, -1);
						}
						else if(IsRightWeapon(weaponNumber)){
							animator.SetInteger("RightWeapon", 0);
							SetAnimator(weaponTo, -2, -1, 0, -1);
						}
						animator.SetBool("Shield", false);
					}
					//Sheath 2handed weapon.
					else if(Is2HandedWeapon(weaponNumber)){
						DoWeaponSwitch(weaponTo, weaponNumber, animator.GetInteger("Weapon"), -1, true);
						yield return new WaitForSeconds(0.5f);
						SetAnimator(weaponTo, -2, 0, 0, -1);
					}
				}
				//Unarmed, switching to Relax.
				else if(rightWeapon == 0 && leftWeapon == 0){
					DoWeaponSwitch(weaponTo, weaponNumber, animator.GetInteger("Weapon"), 0, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponTo, -2, 0, 0, -1);
				}
			}
			//Switching to 2handed weapon.
			else if(Is2HandedWeapon(weaponTo)){
				//Switching from 1handed weapons.
				if(animator.GetInteger("Weapon") == 7){
					//Dual weilding, switch to Armed if first switch.
					if(leftWeapon != 0 && rightWeapon != 0){
						DoWeaponSwitch(7, weaponNumber, 7, -1, true);
						if(IsLeftWeapon(weaponNumber)){
							SetAnimator(7, -2, 0, -1, -1);
						}
						else if(IsRightWeapon(weaponNumber)){
							SetAnimator(7, -2, -1, 0, -1);
						}
					}
					else{
						DoWeaponSwitch(0, weaponNumber, 7, -1, true);
						yield return new WaitForSeconds(0.5f);
						SetAnimator(0, -2, 0, 0, -1);
					}
				}
				//Switching from 2handed weapons.
				else{
					DoWeaponSwitch(0, weaponNumber, animator.GetInteger("Weapon"), -1, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponNumber, -2, weaponNumber, 0, -1);
				}
			}
			//Switching to 1handed weapons.
			else{
				//Switching from 2handed weapons, go to Unarmed before next switch.
				if(Is2HandedWeapon(animator.GetInteger("Weapon"))){
					DoWeaponSwitch(0, weaponNumber, animator.GetInteger("Weapon"), 0, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(0, -2, 0, 0, 0);
				}
				//Switching from 1handed weapon(s), go to Armed before next switch.
				else if(Is1HandedWeapon(animator.GetInteger("Weapon"))){
					if(IsRightWeapon(weaponNumber)){
						animator.SetBool("Shield", false);
					}
					DoWeaponSwitch(7, weaponNumber, 7, -1, true);
					yield return new WaitForSeconds(0.1f);
					if(weaponNumber == 7){
						animator.SetBool("Shield", false);
					}
					if(IsLeftWeapon(weaponNumber)){
						SetAnimator(7, 7, 0, -1, 0);
					}
					else{
						SetAnimator(7, 7, -1, 0, 0);
					}
				}
			}
			rpgCharacterController.SetWeaponState(weaponTo);
			yield return null;
		}

		IEnumerator _DualUnSheath(int weaponNumber){
			//Debug.Log("_DualUnSheath: " + weaponNumber);
			//Switching to Unarmed.
			if(weaponNumber == 0){
				DoWeaponSwitch(-1, -1, -1, -1, false);
				yield return new WaitForSeconds(0.5f);
				SetAnimator(0, -1, 0, 0, 0);
			}
			//Switching to 1handed weapons.
			else if(Is1HandedWeapon(weaponNumber)){
				//Only if both hands are empty.
				if(leftWeapon == 0 && rightWeapon == 0){
					//Switching to Shield.
					if(weaponNumber == 7){
						animator.SetBool("Shield", true);
						DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 1, false);
						yield return new WaitForSeconds(0.5f);
						SetAnimator(7, -2, 7, 0, 1);
						yield break;
					}
					DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 3, false);
					//Set alternate weapon for Left.
					if(IsRightWeapon(weaponNumber)){
						rightWeapon = weaponNumber;
						animator.SetInteger("RightWeapon", weaponNumber);
						leftWeapon = weaponNumber - 1;
						animator.SetInteger("LeftWeapon", weaponNumber - 1);
					}
					//Set alternate weapon for Right.
					else if(IsLeftWeapon(weaponNumber)){
						leftWeapon = weaponNumber;
						animator.SetInteger("LeftWeapon", weaponNumber);
						rightWeapon = weaponNumber + 1;
						animator.SetInteger("RightWeapon", weaponNumber + 1);
					}
					yield return new WaitForSeconds(0.5f);
					SetAnimator(7, -2, -1, -1, 3);
				}
				//Only 1 1handed weapon.
				else{
					DoWeaponSwitch(7, weaponNumber, 7, 1, false);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(7, -2, 0, 0, 1);
				}
			}
			else if(Is2HandedWeapon(weaponNumber)){
				DoWeaponSwitch(0, weaponNumber, weaponNumber, -1, false);
				yield return new WaitForSeconds(0.5f);
				SetAnimator(weaponNumber, -1, weaponNumber, 0, 0);
			}
			yield return null;
		}

		IEnumerator _DualSheath(int weaponNumber, int weaponTo){
			//Debug.Log("_DualSheath: " + weaponNumber + " -  Weapon To: " + weaponTo);
			//If switching to Relax from Unarmed.
			if(weaponNumber == 0 && weaponTo == -1){
				DoWeaponSwitch(-1, -1, 0, -1, true);
				yield return new WaitForSeconds(0.5f);
				SetAnimator(-1, -1, 0, 0, 0);
			}
			//Sheath 2handed weapon.
			else if(Is2HandedWeapon(weaponNumber)){
				//Switching to Relax.
				if(weaponTo == -1){
					DoWeaponSwitch(weaponTo, weaponNumber, weaponNumber, 1, true);
				}
				else{
					DoWeaponSwitch(0, weaponNumber, weaponNumber, 1, true);
				}
				yield return new WaitForSeconds(0.5f);
				SetAnimator(weaponTo, -1, 0, 0, 0);
			}
			//Sheath 1handed weapon(s).
			else if(Is1HandedWeapon(weaponNumber)){
				//If has 2 1handed weapons.
				if(leftWeapon != 0 && rightWeapon != 0){
					//If swtiching to 2handed weapon, goto Unarmed.
					if(Is2HandedWeapon(weaponTo)){
						DoWeaponSwitch(0, weaponNumber, 7, 3, true);
						yield return new WaitForSeconds(0.5f);
						StartCoroutine(_HideAllWeapons(false, false));
						SetAnimator(0, -2, 0, 0, 0);
					}
					//Switching to other 1handed weapons.
					else if(Is1HandedWeapon(weaponTo)){
						DoWeaponSwitch(7, weaponNumber, 7, 3, true);
						yield return new WaitForSeconds(0.5f);
						StartCoroutine(_HideAllWeapons(false, false));
						SetAnimator(7, -2, 0, 0, 0);
					}
					//Switching to Unarmed/Relax.
					else if(IsNoWeapon(weaponTo)){
						DoWeaponSwitch(weaponTo, weaponNumber, 7, 3, true);
						yield return new WaitForSeconds(0.5f);
						StartCoroutine(_HideAllWeapons(false, false));
						SetAnimator(weaponTo, -2, 0, 0, 0);
					}
				}
				//Has 1 1handed weapon.
				else{
					DoWeaponSwitch(7, weaponNumber, 7, 3, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponTo, -2, 0, 0, 0);
				}
			}
			yield return null;
		}

		IEnumerator _InstantWeaponSwitch(int weaponNumber){
			animator.SetInteger("Weapon", -2);
			yield return new WaitForEndOfFrame();
			animator.SetTrigger("InstantSwitchTrigger");
			if(Is1HandedWeapon(weaponNumber)){
				animator.SetInteger("Weapon", 7);
			}
			else{
				animator.SetInteger("Weapon", weaponNumber);
			}
			StartCoroutine(_HideAllWeapons(false, false));
			StartCoroutine(_WeaponVisibility(weaponNumber, true, false));
			rpgCharacterController.SetWeaponState(weaponNumber);
		}

		void DoWeaponSwitch(int weaponSwitch, int weaponVisibility, int weaponNumber, int leftRight, bool sheath){
//			Debug.Log("DoWeaponSwitch: " + weaponSwitch + " WeaponNumber: " + weaponNumber + " Sheath: " + sheath);
			//Go to Null state and wait for animator.
			animator.SetInteger("Weapon", -2);
			while(animator.isActiveAndEnabled && animator.GetInteger("Weapon") != -2){
			}
			//Lock character for switch unless has moving sheath/unsheath anims.
			if(weaponSwitch < 1){
				if(Is2HandedWeapon(weaponNumber)){
					rpgCharacterController.Lock(true, true, true, 0f, 1f);
				}
			}
			else if(Is1HandedWeapon(weaponSwitch)){
				rpgCharacterController.Lock(true, true, true, 0f, 1f);
			}
			//Set weaponSwitch if applicable.
			if(weaponSwitch != -2){
				animator.SetInteger("WeaponSwitch", weaponSwitch);
			}
			animator.SetInteger("Weapon", weaponNumber);
			//Set leftRight if applicable.
			if(leftRight != -1){
				animator.SetInteger("LeftRight", leftRight);
			}
			//Set animator trigger.
			if(sheath){
				animator.SetTrigger("WeaponSheathTrigger");
				if(dualSwitch){
					StartCoroutine(_WeaponVisibility(weaponVisibility, false, true));

				}
				else{
					StartCoroutine(_WeaponVisibility(weaponVisibility, false, false));
				}
				//If using IKHands, trigger IK blend.
				if(rpgCharacterController.ikHands != null){
					StartCoroutine(rpgCharacterController.ikHands._BlendIK(false, 0f, 0.2f, weaponVisibility));
				}
			}
			else{
				animator.SetTrigger("WeaponUnsheathTrigger");
				if(dualSwitch){
					StartCoroutine(_WeaponVisibility(weaponVisibility, true, true));
				}
				else{
					StartCoroutine(_WeaponVisibility(weaponVisibility, true, false));
				}
				//If using IKHands, trigger IK blend.
				if(rpgCharacterController.ikHands != null){
					StartCoroutine(rpgCharacterController.ikHands._BlendIK(true, 0.5f, 1, weaponVisibility));
				}
			}
		}

		/// <summary>
		/// Controller weapon switching.
		/// </summary>
		public void SwitchWeaponTwoHand(int upDown){
			if(instantWeaponSwitch){
				StartCoroutine(_HideAllWeapons(false, false));
			}
			isSwitchingFinished = false;
			int weaponSwitch = (int)rpgCharacterController.weapon;
			if(upDown == 0){
				weaponSwitch--;
				if(weaponSwitch < 1 || weaponSwitch == 18 || weaponSwitch == 20){
					StartCoroutine(_SwitchWeapon(6));
				}
				else{
					StartCoroutine(_SwitchWeapon(weaponSwitch));
				}
			}
			if(upDown == 1){
				weaponSwitch++;
				if(weaponSwitch > 6 && weaponSwitch < 18){
					StartCoroutine(_SwitchWeapon(1));
				}
				else{
					StartCoroutine(_SwitchWeapon(weaponSwitch));
				}
			}
		}

		/// <summary>
		/// Controller weapon switching.
		/// </summary>
		public void SwitchWeaponLeftRight(int leftRight){
			if(instantWeaponSwitch){
				StartCoroutine(_HideAllWeapons(false, false));
			}
			int weaponSwitch = 0;
			isSwitchingFinished = false;
			if(leftRight == 0){
				weaponSwitch = leftWeapon;
				if(weaponSwitch < 16 && weaponSwitch != 0 && leftWeapon != 7){
					weaponSwitch += 2;
				}
				else{
					weaponSwitch = 8;
				}
			}
			if(leftRight == 1){
				weaponSwitch = rightWeapon;
				if(weaponSwitch < 17 && weaponSwitch != 0){
					weaponSwitch += 2;
				}
				else{
					weaponSwitch = 9;
				}
			}
			StartCoroutine(_SwitchWeapon(weaponSwitch));
		}

		public void WeaponSwitch(){
			if(isWeaponSwitching){
				isWeaponSwitching = false;
			}
		}

		public void SetSheathLocation(int location){
			animator.SetInteger("SheathLocation", location);
		}

		public IEnumerator _HideAllWeapons(bool timed, bool resetToUnarmed){
			if(timed){
				while(!isWeaponSwitching && instantWeaponSwitch){
					yield return null;
				}
			}
			//Reset to Unarmed.
			if(resetToUnarmed){
				animator.SetInteger("Weapon", 0);
				rpgCharacterController.weapon = Weapon.UNARMED;
				StartCoroutine(rpgCharacterController.rpgCharacterWeaponController._WeaponVisibility(rpgCharacterController.rpgCharacterWeaponController.leftWeapon, false, true));
				animator.SetInteger("RightWeapon", 0);
				animator.SetInteger("LeftWeapon", 0);
				animator.SetInteger("LeftRight", 0);
			}
			if(twoHandAxe != null){
				twoHandAxe.SetActive(false);
			}
			if(twoHandBow != null){
				twoHandBow.SetActive(false);
			}
			if(twoHandCrossbow != null){
				twoHandCrossbow.SetActive(false);
			}
			if(twoHandSpear != null){
				twoHandSpear.SetActive(false);
			}
			if(twoHandSword != null){
				twoHandSword.SetActive(false);
			}
			if(staff != null){
				staff.SetActive(false);
			}
			if(swordL != null){
				swordL.SetActive(false);
			}
			if(swordR != null){
				swordR.SetActive(false);
			}
			if(maceL != null){
				maceL.SetActive(false);
			}
			if(maceR != null){
				maceR.SetActive(false);
			}
			if(daggerL != null){
				daggerL.SetActive(false);
			}
			if(daggerR != null){
				daggerR.SetActive(false);
			}
			if(itemL != null){
				itemL.SetActive(false);
			}
			if(itemR != null){
				itemR.SetActive(false);
			}
			if(shield != null){
				shield.SetActive(false);
			}
			if(pistolL != null){
				pistolL.SetActive(false);
			}
			if(pistolR != null){
				pistolR.SetActive(false);
			}
			if(rifle != null){
				rifle.SetActive(false);
			}
			if(spear != null){
				spear.SetActive(false);
			}
		}
	
		/// <summary>
		/// Sets the animator state.
		/// </summary>
		/// <param name="weapon">Weapon.</param>
		/// <param name="weaponSwitch">Weapon switch.</param>
		/// <param name="Lweapon">Lweapon.</param>
		/// <param name="Rweapon">Rweapon.</param>
		/// <param name="weaponSide">Weapon side.</param>
		void SetAnimator(int weapon, int weaponSwitch, int Lweapon, int Rweapon, int weaponSide){
			Debug.Log("SETANIMATOR: Weapon:" + weapon + " Weaponswitch:" + weaponSwitch + " Lweapon:" + Lweapon + " Rweapon:" + Rweapon + " Weaponside:" + weaponSide);
			//Set Weapon if applicable.
			if(weapon != -2){
				animator.SetInteger("Weapon", weapon);
			}
			//Set WeaponSwitch if applicable.
			if(weaponSwitch != -2){
				animator.SetInteger("WeaponSwitch", weaponSwitch);
			}
			//Set left weapon if applicable.
			if(Lweapon != -1){
				leftWeapon = Lweapon;
				animator.SetInteger("LeftWeapon", Lweapon);
				//Set Shield.
				if(Lweapon == 7){
					animator.SetBool("Shield", true);
				}
				else{
					animator.SetBool("Shield", false);
				}
			}
			//Set right weapon if applicable.
			if(Rweapon != -1){
				rightWeapon = Rweapon;
				animator.SetInteger("RightWeapon", Rweapon);
			}
			//Set weapon side if applicable.
			if(weaponSide != -1){
				animator.SetInteger("LeftRight", weaponSide);
			}
			rpgCharacterController.SetWeaponState(weapon);
		}

		public IEnumerator _WeaponVisibility(int weaponNumber, bool visible, bool dual){
//			Debug.Log("WeaponVisiblity: " + weaponNumber + "  Visible: " + visible + "  dual: " + dual);
			while(isWeaponSwitching){
				yield return null;
			}
			if(weaponNumber == 1){
				twoHandSword.SetActive(visible);
			}
			else if(weaponNumber == 2){
				twoHandSpear.SetActive(visible);
			}
			else if(weaponNumber == 3){
				twoHandAxe.SetActive(visible);
			}
			else if(weaponNumber == 4){
				twoHandBow.SetActive(visible);
			}
			else if(weaponNumber == 5){
				twoHandCrossbow.SetActive(visible);
			}
			else if(weaponNumber == 6){
				staff.SetActive(visible);
			}
			else if(weaponNumber == 7){
				shield.SetActive(visible);
			}
			else if(weaponNumber == 8){
				swordL.SetActive(visible);
				if(dual){
					swordR.SetActive(visible);
				}
			}
			else if(weaponNumber == 9){
				swordR.SetActive(visible);
				if(dual){
					swordL.SetActive(visible);
				}
			}
			else if(weaponNumber == 10){
				maceL.SetActive(visible);
				if(dual){
					maceR.SetActive(visible);
				}
			}
			else if(weaponNumber == 11){
				maceR.SetActive(visible);
				if(dual){
					maceL.SetActive(visible);
				}
			}
			else if(weaponNumber == 12){
				daggerL.SetActive(visible);
				if(dual){
					daggerR.SetActive(visible);
				}
			}
			else if(weaponNumber == 13){
				daggerR.SetActive(visible);
				if(dual){
					daggerL.SetActive(visible);
				}
			}
			else if(weaponNumber == 14){
				itemL.SetActive(visible);
				if(dual){
					itemR.SetActive(visible);
				}
			}
			else if(weaponNumber == 15){
				itemR.SetActive(visible);
				if(dual){
					itemL.SetActive(visible);
				}
			}
			else if(weaponNumber == 16){
				pistolL.SetActive(visible);
				if(dual){
					pistolR.SetActive(visible);
				}
			}
			else if(weaponNumber == 17){
				pistolR.SetActive(visible);
				if(dual){
					pistolL.SetActive(visible);
				}
			}
			else if(weaponNumber == 18){
				rifle.SetActive(visible);
			}
			else if(weaponNumber == 19){
				spear.SetActive(visible);
			}
			yield return null;
		}
		
		public bool IsNoWeapon(int weaponNumber){
			if(weaponNumber < 1){
				return true;
			}
			else{
				return false;
			}
		}
		
		public bool IsLeftWeapon(int weaponNumber){
			if((weaponNumber == 7 || weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16)){
				return true;
			}
			else{
				return false;
			}
		}

		public bool HasLeftWeapon(){
			if(IsLeftWeapon(animator.GetInteger("LeftWeapon"))){
				return true;
			}
			else{
				return false;
			}
		}
		
		public bool IsRightWeapon(int weaponNumber){
			if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19)){
				return true;
			}
			else{
				return false;
			}
		}

		public bool HasRightWeapon(){
			if(IsRightWeapon(animator.GetInteger("RightWeapon"))){
				return true;
			}
			else{
				return false;
			}
		}

		public bool HasDualWeapons(){
			if(HasRightWeapon() && HasLeftWeapon()){
				return true;
			}
			else{
				return false;
			}
		}
		
		public bool Is2HandedWeapon(int weaponNumber){
			if((weaponNumber > 0 && weaponNumber < 7) || weaponNumber == 18 || weaponNumber == 20){
				return true;
			}
			else{
				return false;
			}
		}
		
		public bool Is1HandedWeapon(int weaponNumber){
			if((weaponNumber > 6 && weaponNumber < 18) || weaponNumber == 19){
				return true;
			}
			else{
				return false;
			}
		}
	}
}