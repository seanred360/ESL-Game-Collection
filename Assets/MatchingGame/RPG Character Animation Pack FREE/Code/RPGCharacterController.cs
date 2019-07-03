using UnityEngine;
using System.Collections;

namespace RPGCharacterAnims{

	[RequireComponent(typeof(RPGCharacterMovementController))]
	[RequireComponent(typeof(RPGCharacterWeaponController))]
	[RequireComponent(typeof(RPGCharacterInputController))]
	public class RPGCharacterController : MonoBehaviour{
		
		//Components.
		[HideInInspector]	public RPGCharacterMovementController rpgCharacterMovementController;
		[HideInInspector]	public RPGCharacterWeaponController rpgCharacterWeaponController;
		[HideInInspector]	public RPGCharacterInputController rpgCharacterInputController;
		[HideInInspector] public Animator animator;
		[HideInInspector] public IKHands ikHands;
		public Weapon weapon = Weapon.RELAX;
		public GameObject target;
		PerfectLookAt headLookController;
		
		//Strafing/action.
		[HideInInspector] public bool isDead = false;
		[HideInInspector] public bool isBlocking = false;
		[HideInInspector] public bool canAction = true;
		[HideInInspector] public bool isSitting = false;
		[HideInInspector] public bool isClimbing = false;
		[HideInInspector] public bool isNearLadder = false;
		[HideInInspector] public bool isNearCliff = false;
		[HideInInspector] public GameObject ladder;
		[HideInInspector] public GameObject cliff;
		[HideInInspector] public bool isCasting;
		[HideInInspector] public bool isAiming = false;
		[HideInInspector] public bool isStrafing = false;
		[HideInInspector] public bool injured;
		public bool hipShooting = false;
		public int specialAttack = 0;
		public float aimHorizontal;
		public float aimVertical;
		public float bowPull;
		public bool headLook = false;
		bool isHeadlook = false;
		public int numberOfConversationClips;
		int currentConversation;
		float idleTimer;
		float idleTrigger = 0f;
		
		public float animationSpeed = 1;
		
		#region Initialization
		
		void Awake(){
			rpgCharacterMovementController = GetComponent<RPGCharacterMovementController>();
			rpgCharacterWeaponController = GetComponent<RPGCharacterWeaponController>();
			rpgCharacterInputController = GetComponent<RPGCharacterInputController>();
			animator = GetComponentInChildren<Animator>();
			if(animator == null){
				Debug.LogError("ERROR: There is no animator for character.");
				Destroy(this);
			}
			//Find HeadLookController if applied.
			headLookController = GetComponent<PerfectLookAt>();
			ikHands = GetComponent<IKHands>();
			//Set for starting Relax state.
			weapon = Weapon.RELAX;
			animator.SetInteger("Weapon", -1);
			animator.SetInteger("WeaponSwitch", -1);
			StartCoroutine(_ResetIdleTimer());
			//Turn off headlook in editor.
			#if UNITY_EDITOR
			headLook = false;
			#endif
			isHeadlook = headLook;
		}

		void Start(){
			rpgCharacterMovementController.SwitchCollisionOn();
		}

		#endregion
		
		#region Updates

		void Update(){
			UpdateAnimationSpeed();
			if(rpgCharacterMovementController.rpgCharacterState != RPGCharacterState.Swim && rpgCharacterMovementController.MaintainingGround()){
				//Revive.
				if(rpgCharacterInputController.inputDeath){
					if(isDead){
						Revive();
					}
				}
				if(canAction){
					Blocking();
					if(!isBlocking){
						Strafing();
						RandomIdle();
						DirectionalAiming();
						Aiming();
						Rolling();
						//Hit.
						if(rpgCharacterInputController.inputLightHit){
							GetHit();
						}
						//Death.
						if(rpgCharacterInputController.inputDeath){
							if(!isDead){
								Death();
							}
							else{
								Revive();
							}
						}
						//Attacks.
						if(rpgCharacterInputController.inputAttackL){
							Attack(1);
						}
						if(rpgCharacterInputController.inputAttackR){
							Attack(2);
						}
						if(rpgCharacterInputController.inputLightHit){
							GetHit();
						}
						if(rpgCharacterInputController.inputCastL){
							AttackKick(1);
						}
						if(rpgCharacterInputController.inputCastL){
							StartCoroutine(_BlockBreak());
						}
						if(rpgCharacterInputController.inputCastR){
							AttackKick(2);
						}
						if(rpgCharacterInputController.inputCastR){
							StartCoroutine(_BlockBreak());
						}
						//Switch weapons.
						if(rpgCharacterWeaponController.isSwitchingFinished){
							if(rpgCharacterInputController.inputSwitchUpDown < -0.1f){  
								rpgCharacterWeaponController.SwitchWeaponTwoHand(0);
							}
							else if(rpgCharacterInputController.inputSwitchUpDown > 0.1f){
								rpgCharacterWeaponController.SwitchWeaponTwoHand(1);
							}
							if(rpgCharacterInputController.inputSwitchLeftRight < -0.1f){  
								rpgCharacterWeaponController.SwitchWeaponLeftRight(0);
							}
							else if(rpgCharacterInputController.inputSwitchLeftRight > 0.1f){  
								rpgCharacterWeaponController.SwitchWeaponLeftRight(1);
							}
							//Shield.
							if(rpgCharacterInputController.inputShield){
								StartCoroutine(rpgCharacterWeaponController._SwitchWeapon(7));
							}
							if(rpgCharacterInputController.inputRelax){
								StartCoroutine(rpgCharacterWeaponController._SwitchWeapon(-1));
							}
						}
						//Reset Switching.
						if(rpgCharacterInputController.inputSwitchLeftRight == 0 && rpgCharacterInputController.inputSwitchUpDown == 0){
							rpgCharacterWeaponController.isSwitchingFinished = true;
						}
						//Shooting / Navmesh.
						if(Input.GetMouseButtonDown(0)){
							if(rpgCharacterMovementController.useMeshNav){
								RaycastHit hit;
								if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
									rpgCharacterMovementController.navMeshAgent.destination = hit.point;
								}
							}
							else if((weapon == Weapon.TWOHANDBOW || weapon ==  Weapon.TWOHANDCROSSBOW || weapon ==  Weapon.RIFLE) && isAiming){
								animator.SetInteger("Action", 1);
								if(weapon == Weapon.RIFLE && hipShooting == true){
									animator.SetInteger("Action", 2);
								}
								animator.SetTrigger("AttackTrigger");
							}
						}
						//Reload.
						if(Input.GetMouseButtonDown(2)){
							animator.SetTrigger("ReloadTrigger");
						}
						//Climbing ladder.
						if(rpgCharacterMovementController.rpgCharacterState == RPGCharacterState.ClimbLadder && !isClimbing){
							if(rpgCharacterInputController.inputVertical > 0.1f){
								animator.SetInteger("Action", 1);
								animator.SetTrigger("ClimbLadderTrigger");
							}
							else if(rpgCharacterInputController.inputVertical < -0.1f){
								animator.SetInteger("Action", 2);
								animator.SetTrigger("ClimbLadderTrigger");
							}
						}
						if(rpgCharacterMovementController.rpgCharacterState == RPGCharacterState.ClimbLadder && isClimbing){
							if(rpgCharacterInputController.inputVertical == 0){
								isClimbing = false;
							}
						}
					}
				}
			}
			//Injury toggle.
			if(Input.GetKeyDown(KeyCode.I)){
				if(injured == false){
					injured = true;
					animator.SetBool("Injured", true);
				}
				else{
					injured = false;
					animator.SetBool("Injured", false);
				}
			}
			//Head look toggle.
			if(Input.GetKeyDown(KeyCode.L)){
				if(headLook == false){
					headLook = true;
					isHeadlook = true;
				}
				else{
					headLook = false;
					isHeadlook = false;
				}
			}
			//Slow time toggle.
			if(Input.GetKeyDown(KeyCode.T)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0.005f;
				}
			}
			//Pause toggle.
			if(Input.GetKeyDown(KeyCode.P)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0f;
				}
			}
			//Swim up/down toggle.
			if(rpgCharacterMovementController.rpgCharacterState == RPGCharacterState.Swim && (rpgCharacterInputController.inputStrafe || rpgCharacterInputController.inputStrafe || rpgCharacterInputController.inputTargetBlock > 0.1f)){
				animator.SetBool("Strafing", true);
			}
			else{
				animator.SetBool("Strafing", false);
			}
		}
		
		void LateUpdate(){
			//Headlook.
			if(headLookController != null){
				if(canAction && isHeadlook == true && !isAiming){
					headLookController.m_Weight += 0.03f;
				}
				else{
					headLookController.m_Weight -= 0.03f;
				}
				if(headLookController.m_Weight > 1){
					headLookController.m_Weight = 1;
				}
				else if(headLookController.m_Weight < 0){
					headLookController.m_Weight = 0;
				}
			}
		}

		void UpdateAnimationSpeed(){
			animator.SetFloat("AnimationSpeed", animationSpeed);
		}
		
		#endregion
		
		#region Aiming / Turning
		
		/// <summary>
		/// Direcitonal aiming used by 2Handed Bow and Rifle.
		/// </summary>
		void DirectionalAiming(){
			if(Input.GetKey(KeyCode.LeftArrow)){
				aimHorizontal -= 0.05f;
			}
			if(Input.GetKey(KeyCode.RightArrow)){
				aimHorizontal += 0.05f;
			}
			if(Input.GetKey(KeyCode.DownArrow)){
				aimVertical -= 0.05f;
			}
			if(Input.GetKey(KeyCode.UpArrow)){
				aimVertical += 0.05f;
			}
			if(aimHorizontal >= 1){
				aimHorizontal = 1;
			}
			if(aimHorizontal <= -1){
				aimHorizontal = -1;
			}
			if(aimVertical >= 1){
				aimVertical = 1;
			}
			if(aimVertical <= -1){
				aimVertical = -1;
			}
			if(Input.GetKey(KeyCode.B)){
				bowPull -= 0.05f;
			}
			if(Input.GetKey(KeyCode.N)){
				bowPull += 0.05f;
			}
			if(bowPull >= 1){
				bowPull = 1;
			}
			if(bowPull <= -1){
				bowPull = -1;
			}
			//Set the animator.
			animator.SetFloat("AimHorizontal", aimHorizontal);
			animator.SetFloat("AimVertical", aimVertical);
			animator.SetFloat("BowPull", bowPull);
		}
		
		//Turning.
		public IEnumerator _Turning(int direction){
			if(direction == 1){
				Lock(true, true, true, 0, 0.55f);
				animator.SetTrigger("TurnLeftTrigger");
			}
			if(direction == 2){
				Lock(true, true, true, 0, 0.55f);
				animator.SetTrigger("TurnRightTrigger");
			}
			yield return null;
		}
		
		#endregion
		
		#region Combat

		/// <summary>
		/// Dodge the specified direction.
		/// </summary>
		/// <param name="1">Left</param>
		/// <param name="2">Right</param>
		public IEnumerator _Dodge(int direction){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			animator.SetInteger("Action", direction);
			animator.SetTrigger("DodgeTrigger");
			Lock(true, true, true, 0, 0.55f);
			yield return null;
		}
		
		//0 = No side
		//1 = Left
		//2 = Right
		//3 = Dual
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
		public void Attack(int attackSide){
			int attackNumber = 0;
			if(canAction){
				//Ground attacks.
				if(rpgCharacterMovementController.MaintainingGround()){
					//Stationary attack.
					if(!rpgCharacterMovementController.isMoving){
						if(weapon == Weapon.RELAX){
							weapon = Weapon.UNARMED;
							animator.SetInteger("Weapon", 0);
						}
						//Armed or Unarmed.
						if(weapon == Weapon.UNARMED || weapon == Weapon.ARMED || weapon == Weapon.ARMEDSHIELD){
							int maxAttacks = 3;
							//Left attacks.
							if(attackSide == 1){
								animator.SetInteger("AttackSide", 1);
								//Left sword has 6 attacks.
								if(rpgCharacterWeaponController.leftWeapon == 8){
									attackNumber = Random.Range(1, 8);
								}
								//Left item has 4 attacks.
								else if(rpgCharacterWeaponController.leftWeapon == 14){
									attackNumber = Random.Range(1, 5);
								}
								else{
									attackNumber = Random.Range(1, maxAttacks + 1);
								}
							}
							//Right attacks.
							else if(attackSide == 2){
								animator.SetInteger("AttackSide", 2);
								//Right sword has 6 attacks.
								if(rpgCharacterWeaponController.rightWeapon == 9){
									attackNumber = Random.Range(8, 15);
								}
								//Right item has 4 attacks.
								else if(rpgCharacterWeaponController.rightWeapon == 15){
									attackNumber = Random.Range(5, 9);
								}
								//Right spear has 7 attacks.
								else if(rpgCharacterWeaponController.rightWeapon == 19){
									attackNumber = Random.Range(1, 8);
								}
								else{
									attackNumber = Random.Range(4, maxAttacks + 4);
								}
							}
							//Dual attacks.
							else if(attackSide == 3){
								attackNumber = Random.Range(1, maxAttacks + 1);
							}
							//Set the Locks.
							if(attackSide != 3){
								if(rpgCharacterWeaponController.leftWeapon == 8 || rpgCharacterWeaponController.leftWeapon == 10 || rpgCharacterWeaponController.leftWeapon == 16 
									|| rpgCharacterWeaponController.rightWeapon == 9 || rpgCharacterWeaponController.rightWeapon == 11 || rpgCharacterWeaponController.rightWeapon == 17){
									Lock(true, true, true, 0, 0.75f);
								}
								else{
									//Dagger and Item has longer attack time.
									Lock(true, true, true, 0, 1f);
								}
							}
							//Dual attacks.
							else{
								Lock(true, true, true, 0, 0.75f);
							}
						}
						//Shield or 2Handed Weapons.
						else if(weapon == Weapon.SHIELD){
							int maxAttacks = 1;
							attackNumber = Random.Range(1, maxAttacks);
							Lock(true, true, true, 0, 1.1f);
						}
						else if(weapon == Weapon.TWOHANDSPEAR){
							int maxAttacks = 10;
							attackNumber = Random.Range(1, maxAttacks);
							Lock(true, true, true, 0, 1.1f);
						}
						else if(weapon == Weapon.TWOHANDSWORD){
							int maxAttacks = 11;
							attackNumber = Random.Range(1, maxAttacks);
							Lock(true, true, true, 0, 1.1f);
						}
						else if(weapon == Weapon.RIFLE){
							int maxAttacks = 3;
							attackNumber = Random.Range(1, maxAttacks);
							Lock(true, true, true, 0, 1.1f);
						}
						else{
							int maxAttacks = 6;
							attackNumber = Random.Range(1, maxAttacks);
							if(weapon == Weapon.TWOHANDSWORD){
								Lock(true, true, true, 0, 0.85f);
							}
							else if(weapon == Weapon.TWOHANDAXE){
								Lock(true, true, true, 0, 1.5f);
							}
							else if(weapon == Weapon.STAFF){
								Lock(true, true, true, 0, 1f);
							}
							else{
								Lock(true, true, true, 0, 0.75f);
							}
						}
					}
					//Running attack.
					else{
						RunningAttack(attackSide);
						return;
					}
				}
				//Air attacks.
				else{
					AirAttack();
					return;
				}
				//Trigger the animation.
				animator.SetInteger("Action", attackNumber);
				if(attackSide == 3){
					animator.SetTrigger("AttackDualTrigger");
				}
				else{
					animator.SetTrigger("AttackTrigger");
				}
			}
		}

		void RunningAttack(int attackSide){
//			Debug.Log("RunningAttack");
			//If armed and moving, running attacks.
			if((weapon == Weapon.ARMED || weapon == Weapon.ARMEDSHIELD) && rpgCharacterInputController.current.HasMoveInput()){
				if(attackSide == 1 && rpgCharacterWeaponController.HasLeftWeapon()){
					animator.SetInteger("Action", 1);
					animator.SetTrigger("AttackTrigger");
				}
				if(attackSide == 2 && rpgCharacterWeaponController.HasRightWeapon()){
					animator.SetInteger("Action", 4);
					animator.SetTrigger("AttackTrigger");
				}
				if(attackSide == 3 && rpgCharacterWeaponController.HasDualWeapons()){
					animator.SetInteger("Action", 1);
					animator.SetTrigger("AttackDualTrigger");
				}
			}
		}

		void AirAttack(){
			animator.SetInteger("Action", 1);
			animator.SetTrigger("AttackTrigger");
			rpgCharacterInputController.current.jumpInput = true;
		}
		
		public void AttackKick(int kickSide){
			if(rpgCharacterMovementController.MaintainingGround()){
				if(weapon == Weapon.RELAX){
					weapon = Weapon.UNARMED;
					animator.SetInteger("Weapon", 0);
				}
				animator.SetInteger("Action", kickSide);
				animator.SetTrigger("AttackKickTrigger");
				Lock(true, true, true, 0, 0.9f);
			}
		}
		
		public void Special(int special){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			if(specialAttack == 0){
				specialAttack = special;
				animator.SetInteger("Action", special);
				animator.SetTrigger("SpecialAttackTrigger");
				Lock(true, true, true, 0, 0.5f);
			}
			else{
				animator.SetTrigger("SpecialEndTrigger");
				Lock(true, true, true, 0, 0.6f);
				UnLock(true, true);
				specialAttack = 0;
			}
		}
		
		/// <summary>
		/// Cast the specified attackSide and type. 
		///0 = No side
		///1 = Left
		///2 = Right
		///3 = Dual
		/// </summary>
		public void Cast(int attackSide, string type){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			//Cancel current casting.
			if(attackSide == 0){
				animator.SetTrigger("CastEndTrigger");
				isCasting = false;
				canAction = true;
				Lock(true, true, true, 0, 0.1f);
				return;
			}
			int maxAttacks = 3;
			//Set Left, Right, Dual for variable casts.
			if(attackSide == 4){
				if(rpgCharacterWeaponController.leftWeapon == 0 && rpgCharacterWeaponController.rightWeapon == 0){
					animator.SetInteger("LeftRight", 3);
				}
				else if(rpgCharacterWeaponController.leftWeapon == 0){
					animator.SetInteger("LeftRight", 1);
				}
				else if(rpgCharacterWeaponController.rightWeapon == 0){
					animator.SetInteger("LeftRight", 2);
				}
			}
			else{
				animator.SetInteger("LeftRight", attackSide);
			}
			//Cast Buffs, AOE, Summons.
			if(weapon == Weapon.UNARMED || weapon == Weapon.STAFF || weapon == Weapon.ARMED){
				//Buff1 = 1
				//Buff2 = 2
				//AOE1 = 3
				//AOE2 = 4
				//Summon1 = 5
				//Summon2 = 6
				maxAttacks = 2;
				int attackNumber = Random.Range(1, maxAttacks + 1);
				if(rpgCharacterMovementController.MaintainingGround()){
					if(type == "buff"){
						animator.SetInteger("Action", attackNumber);
					}
					else if(type == "AOE"){
						animator.SetInteger("Action", attackNumber + 2);
					}
					else if(type == "summon"){
						animator.SetInteger("Action", attackNumber + 4);
					}
				}
			}
			//Trigger Cast if character is grounded.
			if(rpgCharacterMovementController.MaintainingGround()){
				if(type == "attack"){
					animator.SetInteger("Action", Random.Range(1, maxAttacks + 1));
					animator.SetTrigger("AttackCastTrigger");
				}
				else{
					animator.SetTrigger("CastTrigger");
				}
				isCasting = true;
				canAction = false;
				Lock(true, true, false, 0, 0.8f);
			}
		}

		public void Blocking(){
			if(Input.GetAxisRaw("TargetBlock") < -0.8f){
				if(!isBlocking){
					animator.SetTrigger("BlockTrigger");
				}
				isBlocking = true;
				animator.SetBool("Blocking", true);
				rpgCharacterMovementController.canMove = false;
			}
			else{
				isBlocking = false;
				animator.SetBool("Blocking", false);
				rpgCharacterMovementController.canMove = true;
			}
		}

		void Strafing(){
			if(rpgCharacterInputController.inputStrafe || rpgCharacterInputController.inputTargetBlock > 0.8f && weapon != Weapon.RIFLE){
				if(weapon != Weapon.RELAX){
					animator.SetBool("Strafing", true);
					isStrafing = true;
				}
				if(rpgCharacterInputController.inputCastL){
					Cast(1, "attack");
				}
				if(rpgCharacterInputController.inputCastR){
					Cast(2, "attack");
				}
			}
			else{
				isStrafing = false;
				animator.SetBool("Strafing", false);
			}
		}

		void Aiming(){
			if(rpgCharacterInputController.current.HasAimInput() && (weapon == Weapon.RIFLE || weapon == Weapon.TWOHANDCROSSBOW || weapon == Weapon.TWOHANDBOW)){
				if(!isAiming){
					isAiming = true;
					animator.SetBool("Aiming", true);
				}
			}
			else{
				isAiming = false;
				animator.SetBool("Aiming", false);
			}
		}

		void Rolling(){
			if(!rpgCharacterMovementController.isRolling){
				if(rpgCharacterInputController.inputRoll){
					rpgCharacterMovementController.DirectionalRoll();
				}
			}
		}
		
		public void GetHit(){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			if(weapon != Weapon.RIFLE || weapon != Weapon.TWOHANDCROSSBOW){
				int hits = 5;
				if(isBlocking){
					hits = 2;
				}
				int hitNumber = Random.Range(1, hits + 1);
				animator.SetInteger("Action", hitNumber);
				animator.SetTrigger("GetHitTrigger");
				Lock(true, true, true, 0.1f, 0.4f);
				if(isBlocking){
					StartCoroutine(rpgCharacterMovementController._Knockback(-transform.forward, 3, 3));
					return;
				}
				//Apply directional knockback force.
				if(hitNumber <= 1){
					StartCoroutine(rpgCharacterMovementController._Knockback(-transform.forward, 8, 4));
				}
				else if(hitNumber == 2){
					StartCoroutine(rpgCharacterMovementController._Knockback(transform.forward, 8, 4));
				}
				else if(hitNumber == 3){
					StartCoroutine(rpgCharacterMovementController._Knockback(transform.right, 8, 4));
				}
				else if(hitNumber == 4){
					StartCoroutine(rpgCharacterMovementController._Knockback(-transform.right, 8, 4));
				}
			}
		}
		
		public void Death(){
			animator.SetTrigger("Death1Trigger");
			Lock(true, true, false, 0.1f, 0f);
			isDead = true;
		}
		
		public void Revive(){
			animator.SetTrigger("Revive1Trigger");
			Lock(true, true, true, 0f, 1f);
			isDead = false;
		}

		/// <summary>
		/// Climbing.
		/// </summary>
		/// <param name="Climb-Up">1</param>
		/// <param name="Climb-Down">2</param>
		/// <param name="Climb-Off-Top">3</param>
		/// <param name="Climb-Off-Bottom">4</param>
		/// <param name="Climb-On-Top">5</param>
		/// <param name="Climb-On-Bottom">6</param>
		public IEnumerator _ClimbLadder(){
			rpgCharacterMovementController.SwitchCollisionOff();
			//Get the direction of the ladder, and snap the character to the correct position and facing.
			Vector3 newVector = Vector3.Cross(ladder.transform.forward, ladder.transform.right);
			Vector3 newSpot = ladder.transform.position + (newVector.normalized * 0.71f);
			transform.position = new Vector3(newSpot.x, 0, newSpot.z);
			transform.rotation = Quaternion.Euler(transform.rotation.x, ladder.transform.rotation.eulerAngles.y, transform.rotation.z);
			isClimbing = true;
			animator.SetInteger("Action", 6);
			animator.SetTrigger("ClimbLadderTrigger");
			yield return new WaitForSeconds(1.2f);
			rpgCharacterMovementController.currentState = RPGCharacterState.ClimbLadder;
			rpgCharacterMovementController.rpgCharacterState = RPGCharacterState.ClimbLadder;
		}

		public void EndClimbingLadder(){
			rpgCharacterMovementController.currentState = RPGCharacterState.Idle;
			rpgCharacterMovementController.rpgCharacterState = RPGCharacterState.Idle;
			isClimbing = false;
		}
		
		#endregion

		#region Actions

		/* List of Actions for Animator triggers.
		 * 0: Sit
		 * 1: Laydown
		 * 2: Pickup
		 * 3: Activate
		 * 4: Drink1
		 * 5: Bow1
		 * 6: Bow2
		 * 7: No
		 * 8: Yes
		 * 9: Boost1
		 * */
		
		/// <summary>
		/// Keep character from doing actions.
		/// </summary>
		void LockAction(){
//			Debug.Log("LockAction");
			canAction = false;
			isHeadlook = false;
		}
		
		/// <summary>
		/// Let character move and act again.
		/// </summary>
		void UnLock(bool movement, bool actions){
			StartCoroutine(_ResetIdleTimer());
			if(movement){
				rpgCharacterMovementController.UnlockMovement();
			}
			if(actions){
				canAction = true;
				if(headLook){
					isHeadlook = true;
				}
			}
		}
	
		public void Sit(){
			canAction = false;
			isSitting = true;
			rpgCharacterMovementController.LockMovement();
			animator.SetInteger("Action", 0);
			animator.SetTrigger("ActionTrigger");
		}

		public void Sleep(){
			canAction = false;
			isSitting = true;
			rpgCharacterMovementController.LockMovement();
			animator.SetInteger("Action", 1);
			animator.SetTrigger("ActionTrigger");	
		}

		public void Stand(){
			isSitting = false;
			//Sitting.
			if(animator.GetInteger("Action") == 0){
				Lock(true, true, true, 0f, 1f);
			}
			//Laying down.
			else if(animator.GetInteger("Action") == 1){
				Lock(true, true, true, 0f, 2f);
			}
			animator.SetTrigger("ActionTrigger");
		}

		public void Pickup(){
			animator.SetInteger("Action", 2);
			animator.SetTrigger("ActionTrigger");
			Lock(true, true, true, 0, 1.4f);
		}
		
		public void Activate(){
			animator.SetInteger("Action", 3);
			animator.SetTrigger("ActionTrigger");
			Lock(true, true, true, 0, 1.2f);
		}

		public void Drink(){
			animator.SetInteger("Action", 4);
			animator.SetTrigger("ActionTrigger");
			Lock(true, true, true, 0, 1f);
		}

		public void Bow(){
			int numberOfBows = Random.Range(1, 3);
			animator.SetInteger("Action", numberOfBows + 4);
			animator.SetTrigger("ActionTrigger");
			Lock(true, true, true, 0, 3f);
		}

		public void No(){
			animator.SetInteger("Action", 7);
			animator.SetTrigger("ActionTrigger");
		}

		public void Yes(){
			animator.SetInteger("Action", 8);
			animator.SetTrigger("ActionTrigger");
		}

		public void Boost(){
			animator.SetInteger("Action", 9);
			animator.SetTrigger("ActionTrigger");
			Lock(true, true, true, 0, 1f);
		}

		#endregion
		
		#region Misc

		//Placeholder functions for Animation events.
		public void Hit(){
		}

		public void Shoot(){
		}

		public void FootR(){
		}

		public void FootL(){
		}

		public void Land(){
		}

		/// <summary>
		/// Plays random idle animation.  Currently only Alert1 animation.
		/// </summary>
		void RandomIdle(){
			if(!rpgCharacterMovementController.isMoving && weapon != Weapon.RELAX && !isAiming && !rpgCharacterWeaponController.isWeaponSwitching && rpgCharacterMovementController.canMove){
				idleTimer += 0.01f;
				if(idleTimer > idleTrigger){
					animator.SetInteger("Action", 1);
					animator.SetTrigger("IdleTrigger");
					StartCoroutine(_ResetIdleTimer());
					//TODO set anim times.
					Lock(true, true, true, 0, 1.25f);
				}
			}
		}

		IEnumerator _ResetIdleTimer(){
			idleTrigger = Random.Range(5f, 15f);
			idleTimer = 0;
			yield return new WaitForSeconds(1f);
			animator.ResetTrigger("IdleTrigger");
		}

		IEnumerator _GetCurrentAnimationLength(){
			yield return new WaitForEndOfFrame();
			float f = (float)animator.GetCurrentAnimatorClipInfo(0).Length;
			Debug.Log(f);
		}
			
		/// <summary>
		/// Lock character movement and/or action, on a delay for a set time.
		/// </summary>
		/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
		/// <param name="lockAction">If set to <c>true</c> lock action.</param>
		/// <param name="timed">If set to <c>true</c> timed.</param>
		/// <param name="delayTime">Delay time.</param>
		/// <param name="lockTime">Lock time.</param>
		public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime){
			StopCoroutine("_Lock");
     		StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
		}
		
		//Timed -1 = infinite, 0 = no, 1 = yes.
		public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime){
			if(delayTime > 0){
				yield return new WaitForSeconds(delayTime);
			}
			if(lockMovement){
				rpgCharacterMovementController.LockMovement();
			}
			if(lockAction){
				LockAction();
			}
			if(timed){
				if(lockTime > 0){
					yield return new WaitForSeconds(lockTime);
				}
				UnLock(lockMovement, lockAction);
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
				rpgCharacterWeaponController.leftWeapon = Lweapon;
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
				rpgCharacterWeaponController.rightWeapon = Rweapon;
				animator.SetInteger("RightWeapon", Rweapon);
			}
			//Set weapon side if applicable.
			if(weaponSide != -1){
				animator.SetInteger("LeftRight", weaponSide);
			}
			SetWeaponState(weapon);
		}
		
		public void SetWeaponState(int weaponNumber){
			if(weaponNumber == -1){
				weapon = Weapon.RELAX;
			}
			else if(weaponNumber == 0){
				weapon = Weapon.UNARMED;
			}
			else if(weaponNumber == 1){
				weapon = Weapon.TWOHANDSWORD;
			}
			else if(weaponNumber == 2){
				weapon = Weapon.TWOHANDSPEAR;
			}
			else if(weaponNumber == 3){
				weapon = Weapon.TWOHANDAXE;
			}
			else if(weaponNumber == 4){
				weapon = Weapon.TWOHANDBOW;
			}
			else if(weaponNumber == 5){
				weapon = Weapon.TWOHANDCROSSBOW;
			}
			else if(weaponNumber == 6){
				weapon = Weapon.STAFF;
			}
			else if(rpgCharacterWeaponController.Is1HandedWeapon(weaponNumber)){
				weapon = Weapon.ARMED;
				if(animator.GetInteger("LeftWeapon") == 7){
					if(animator.GetInteger("RightWeapon") != 0){
						weapon = Weapon.ARMEDSHIELD;
					}
					else{
						weapon = Weapon.SHIELD;
					}
				}
			}
			else if(weaponNumber == 18){
				weapon = Weapon.RIFLE;
			}
		}
		
		public IEnumerator _BlockBreak(){
			animator.applyRootMotion = true;
			animator.SetTrigger("BlockBreakTrigger");
			yield return new WaitForSeconds(1f);
			animator.applyRootMotion = false;
		}
		
		public void StartConversation(){
			currentConversation = Random.Range(1, numberOfConversationClips + 1);
			animator.SetInteger("Talking", Random.Range(1, currentConversation));
			StartCoroutine(_PlayConversationClip());
			canAction = false;
			rpgCharacterMovementController.LockMovement();
		}
		
		public void StopConversation(){
			currentConversation = 0;
			animator.SetInteger("Talking", 0);
			StopCoroutine("_PlayConversationClip");
		}
		
		/// <summary>
		/// Plays a random conversation animation.
		/// </summary>
		/// <returns>The conversation clip.</returns>
		IEnumerator _PlayConversationClip(){
			if(currentConversation != 0){
				yield return new WaitForSeconds(2f);
				animator.SetInteger("Talking", Random.Range(1, numberOfConversationClips + 1));
				StartCoroutine(_PlayConversationClip());
			}
			else{
				currentConversation = 0;
				animator.SetInteger("Talking", 0);
				canAction = true;
				rpgCharacterMovementController.LockMovement();
			}
		}
		
		public void AnimatorDebug(){
			Debug.Log("ANIMATOR SETTINGS---------------------------");
			Debug.Log("Moving: " + animator.GetBool("Moving"));
			Debug.Log("Strafing: " + animator.GetBool("Strafing"));
			Debug.Log("Aiming: " + animator.GetBool("Aiming"));
			Debug.Log("Stunned: " + animator.GetBool("Stunned"));
			Debug.Log("Shield: " + animator.GetBool("Shield"));
			Debug.Log("Swimming: " + animator.GetBool("Swimming"));
			Debug.Log("Blocking: " + animator.GetBool("Blocking"));
			Debug.Log("Injured: " + animator.GetBool("Injured"));
			Debug.Log("Weapon: " + animator.GetInteger("Weapon"));
			Debug.Log("WeaponSwitch: " + animator.GetInteger("WeaponSwitch"));
			Debug.Log("LeftRight: " + animator.GetInteger("LeftRight"));
			Debug.Log("LeftWeapon: " + animator.GetInteger("LeftWeapon"));
			Debug.Log("RightWeapon: " + animator.GetInteger("RightWeapon"));
			Debug.Log("AttackSide: " + animator.GetInteger("AttackSide"));
			Debug.Log("Jumping: " + animator.GetInteger("Jumping"));
			Debug.Log("Action: " + animator.GetInteger("Action"));
			Debug.Log("SheathLocation: " + animator.GetInteger("SheathLocation"));
			Debug.Log("Talking: " + animator.GetInteger("Talking"));
			Debug.Log("Velocity X: " + animator.GetFloat("Velocity X"));
			Debug.Log("Velocity Z: " + animator.GetFloat("Velocity Z"));
			Debug.Log("AimHorizontal: " + animator.GetFloat("AimHorizontal"));
			Debug.Log("AimVertical: " + animator.GetFloat("AimVertical"));
			Debug.Log("BowPull: " + animator.GetFloat("BowPull"));
			Debug.Log("Charge: " + animator.GetFloat("Charge"));
		}
		
		#endregion
		
	}
}