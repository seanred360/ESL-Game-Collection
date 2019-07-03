using UnityEngine;
using System.Collections;

namespace RPGCharacterAnims{
	
	public enum RPGCharacterState{
		Idle = 0,
		Move = 1,
		Jump = 2,
		DoubleJump = 3,
		Fall = 4,
		Swim = 5,
		Block = 6,
		ClimbLadder = 7,
		Roll = 8
	}

	public class RPGCharacterMovementController : SuperStateMachine{
		
		//Components.
		[HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
		private SuperCharacterController superCharacterController;
		private RPGCharacterController rpgCharacterController;
		private RPGCharacterInputController rpgCharacterInputController;
		private Rigidbody rb;
		private Animator animator;
		CapsuleCollider capCollider;
		public RPGCharacterState rpgCharacterState;

		[HideInInspector] public bool useMeshNav;
		[HideInInspector] public Vector3 lookDirection { get; private set; }
		[HideInInspector] public bool isKnockback;
		public float knockbackMultiplier = 1f;

		//Jumping.
		[HideInInspector] public bool canJump;
		[HideInInspector] public bool canDoubleJump = false;
		bool doublejumped = false;
		public float gravity = 25.0f;
		public float jumpAcceleration = 5.0f;
		public float jumpHeight = 3.0f;
		public float doubleJumpHeight = 4f;

		//Movement.
		[HideInInspector] public Vector3 currentVelocity;
		[HideInInspector] public bool isMoving = false;
		[HideInInspector] public bool canMove = true;
		[HideInInspector] public bool crouch;
		[HideInInspector] public bool isSprinting;
		public float movementAcceleration = 90.0f;
		public float walkSpeed = 4f;
		public float runSpeed = 6f;
		public float sprintSpeed = 12;
		float rotationSpeed = 40f;
		public float groundFriction = 50f;

		//Rolling.
		[HideInInspector] public bool isRolling = false;
		public float rollSpeed = 8;
		public float rollduration = 0.35f;
		private int rollNumber;

		//Air control.
		public float inAirSpeed = 6f;

		//Swimming.
		public float swimSpeed = 4f;
		public float waterFriction = 3f;

		void Awake(){
			superCharacterController = GetComponent<SuperCharacterController>();
			rpgCharacterController = GetComponent<RPGCharacterController>();
			rpgCharacterInputController = GetComponent<RPGCharacterInputController>();
			navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			animator = GetComponentInChildren<Animator>();
			capCollider = GetComponent<CapsuleCollider>();
			rb = GetComponent<Rigidbody>();
			if(rb != null){
				//Set restraints on startup if using Rigidbody.
				rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			}
			//Set currentState to idle on startup.
			currentState = RPGCharacterState.Idle;
			rpgCharacterState = RPGCharacterState.Idle;
		}

		/*void Update () {
		 * Update is normally run once on every frame update. We won't be using it in this case, since the SuperCharacterController component sends a callback Update called SuperUpdate. SuperUpdate is recieved by the SuperStateMachine, and then fires further callbacks depending on the state
		}*/

		//Put any code in here you want to run BEFORE the state's update function. This is run regardless of what state you're in.
		protected override void EarlyGlobalSuperUpdate(){
		}

		//Put any code in here you want to run AFTER the state's update function.  This is run regardless of what state you're in.
		protected override void LateGlobalSuperUpdate(){
			//Move the player by our velocity every frame.
			transform.position += currentVelocity * superCharacterController.deltaTime;
			//If using Navmesh nagivation, update values.
			if(useMeshNav && navMeshAgent != null){
				if(navMeshAgent.velocity.sqrMagnitude > 0){
					animator.SetBool("Moving", true);
					animator.SetFloat("Velocity Z", navMeshAgent.velocity.magnitude);
				}
				else{
					animator.SetFloat("Velocity Z", 0);
				}
			}
			//If alive and is moving, set animator.
			else if(!rpgCharacterController.isDead && canMove){
				if(currentVelocity.magnitude > 0 && rpgCharacterInputController.current.HasMoveInput()){
					isMoving = true;
					animator.SetBool("Moving", true);
					//Swimming.
					if(rpgCharacterState == RPGCharacterState.Swim){
						animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);
					}
					//Not swimming.
					else{
						animator.SetFloat("Velocity Z", currentVelocity.magnitude);
					}
				}
				else{
					isMoving = false;
					animator.SetBool("Moving", false);
				}
				//Sprinting.
				if(isSprinting){
					animator.SetBool("Sprint", true);
				}
				else{
					animator.SetBool("Sprint", false);
				}
			}
			//Aiming.
			if(rpgCharacterController.isAiming){
				Aiming();
			}
			else{
				if(!rpgCharacterController.isStrafing){
					if(rpgCharacterInputController.current.HasMoveInput() && canMove){
						RotateTowardsMovementDir();
					}
				}
				else{
					Strafing(rpgCharacterController.target.transform.position);
				}
			}
		}

		private bool AcquiringGround(){
			return superCharacterController.currentGround.IsGrounded(false, 0.01f);
		}

		public bool MaintainingGround(){
			return superCharacterController.currentGround.IsGrounded(true, 0.5f);
		}

		public void RotateGravity(Vector3 up){
			lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
		}

		/// <summary>
		/// Constructs a vector representing our movement local to our lookDirection, which is controlled by the camera.
		/// </summary>
		private Vector3 LocalMovement(){
	//		Vector3 right = Vector3.Cross(controller.up, lookDirection);
	//		Vector3 local = Vector3.zero;
	//		if(input.current.MoveInput.x != 0){
	//			local += right * input.current.MoveInput.x;
	//		}
	//		if(input.current.MoveInput.z != 0){
	//			local += lookDirection * input.current.MoveInput.z;
	//		}
			return rpgCharacterInputController.current.moveInput;
		}

		// Calculate the initial velocity of a jump based off gravity and desired maximum height attained
		private float CalculateJumpSpeed(float jumpHeight, float gravity){
			return Mathf.Sqrt(2 * jumpHeight * gravity);
		}

		//Below are the state functions. Each one is called based on the name of the state, so when currentState = Idle, we call Idle_EnterState. If currentState = Jump, we call Jump_SuperUpdate()
		void Idle_EnterState(){
			superCharacterController.EnableSlopeLimit();
			superCharacterController.EnableClamping();
			canJump = true;
			doublejumped = false;
			canDoubleJump = false;
			animator.SetInteger("Jumping", 0);
		}

		//Run every frame we are in the idle state.
		void Idle_SuperUpdate(){
			//If Jump.
			if(rpgCharacterInputController.allowedInput && rpgCharacterInputController.current.jumpInput){
				currentState = RPGCharacterState.Jump;
				rpgCharacterState = RPGCharacterState.Jump;
				return;
			}
			if(!MaintainingGround()){
				currentState = RPGCharacterState.Fall;
				rpgCharacterState = RPGCharacterState.Fall;
				return;
			}
			if(rpgCharacterInputController.current.HasMoveInput() && canMove){
				currentState = RPGCharacterState.Move;
				rpgCharacterState = RPGCharacterState.Move;
				return;
			}
			//Apply friction to slow to a halt.
			currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction * superCharacterController.deltaTime);
		}

		void Idle_ExitState(){
			//Run once when exit the idle state.
		}

		void Move_SuperUpdate(){
			//If Jump.
			if(rpgCharacterInputController.allowedInput && rpgCharacterInputController.current.jumpInput){
				currentState = RPGCharacterState.Jump;
				rpgCharacterState = RPGCharacterState.Jump;
				return;
			}
			if(!MaintainingGround()){
				currentState = RPGCharacterState.Fall;
				rpgCharacterState = RPGCharacterState.Fall;
				return;
			}
			//Set speed determined by movement type.
			if(rpgCharacterInputController.current.HasMoveInput() && canMove){
				//Keep strafing animations from playing.
				animator.SetFloat("Velocity X", 0F);
				//Injured limp.
				if(rpgCharacterController.injured){
					currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * 1.35f, movementAcceleration * superCharacterController.deltaTime);
					return;
				}
				//Sprinting.
				if(isSprinting){
					currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * sprintSpeed, movementAcceleration * superCharacterController.deltaTime);
					return;
				}
				//Strafing or Walking.
				if(rpgCharacterController.isStrafing){
					currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * walkSpeed, movementAcceleration * superCharacterController.deltaTime);
					if(rpgCharacterController.weapon != Weapon.RELAX){
						Strafing(rpgCharacterController.target.transform.position);
					}
					return;
				}
				//Run.
				currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * runSpeed, movementAcceleration * superCharacterController.deltaTime);
			}
			else{
				currentState = RPGCharacterState.Idle;
				rpgCharacterState = RPGCharacterState.Idle;
				return;
			}
		}

		void Jump_EnterState(){
			superCharacterController.DisableClamping();
			superCharacterController.DisableSlopeLimit();
			currentVelocity += superCharacterController.up * CalculateJumpSpeed(jumpHeight, gravity);
			//Set weaponstate to Unarmed if Relaxed.
			if(rpgCharacterController.weapon == Weapon.RELAX){
				rpgCharacterController.weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			canJump = false;
			animator.SetInteger("Jumping", 1);
			animator.SetTrigger("JumpTrigger");
		}

		void Jump_SuperUpdate(){
			Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(superCharacterController.up, currentVelocity);
			Vector3 verticalMoveDirection = currentVelocity - planarMoveDirection;
			if(Vector3.Angle(verticalMoveDirection, superCharacterController.up) > 90 && AcquiringGround()){
				currentVelocity = planarMoveDirection;
				currentState = RPGCharacterState.Idle;
				rpgCharacterState = RPGCharacterState.Idle;
				return;            
			}
			planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, LocalMovement() * inAirSpeed, jumpAcceleration * superCharacterController.deltaTime);
			verticalMoveDirection -= superCharacterController.up * gravity * superCharacterController.deltaTime;
			currentVelocity = planarMoveDirection + verticalMoveDirection;
			//Can double jump if starting to fall.
			if(currentVelocity.y < 0){
				DoubleJump();
			}
		}

		void DoubleJump_EnterState(){
			currentVelocity += superCharacterController.up * CalculateJumpSpeed(doubleJumpHeight, gravity);
			canDoubleJump = false;
			doublejumped = true;
			animator.SetInteger("Jumping", 3);
			animator.SetTrigger("JumpTrigger");
		}

		void DoubleJump_SuperUpdate(){
			Jump_SuperUpdate();
		}

		void DoubleJump(){
			if(!doublejumped){
				canDoubleJump = true;
			}
			if(rpgCharacterInputController.current.jumpInput && canDoubleJump && !doublejumped){
				currentState = RPGCharacterState.DoubleJump;
				rpgCharacterState = RPGCharacterState.DoubleJump;
			}
		}

		void Fall_EnterState(){
			if(!doublejumped){
				canDoubleJump = true;
			}
			superCharacterController.DisableClamping();
			superCharacterController.DisableSlopeLimit();
			canJump = false;
			animator.SetInteger("Jumping", 2);
			animator.SetTrigger("JumpTrigger");
		}

		void Fall_SuperUpdate(){
			if(AcquiringGround()){
				currentVelocity = Math3d.ProjectVectorOnPlane(superCharacterController.up, currentVelocity);
				currentState = RPGCharacterState.Idle;
				rpgCharacterState = RPGCharacterState.Idle;
				return;
			}
			DoubleJump();
			currentVelocity -= superCharacterController.up * gravity * superCharacterController.deltaTime;
		}

		void Swim_EnterState(){
			superCharacterController.DisableClamping();
			superCharacterController.DisableSlopeLimit();
			animator.SetBool("Strafing", false);
			rpgCharacterController.isStrafing = false;
			animator.SetBool("Aiming", false);
			rpgCharacterController.isAiming = false;
			rpgCharacterController.canAction = false;
			animator.SetTrigger("SwimTrigger");
			animator.SetBool("Swimming", true);
			StartCoroutine(rpgCharacterController.rpgCharacterWeaponController._HideAllWeapons(false, true));
			//Scale collider to match position of character.
			superCharacterController.radius = 1.5f;
			if(capCollider != null){
				capCollider.radius = 1.5f;
			}
		}

		void Swim_ExitState(){
			if(capCollider != null){
				capCollider.radius = 0.6f;
			}
			superCharacterController.radius = 0.6f;
			rpgCharacterController.canAction = true;
		}

		void Swim_SuperUpdate(){
			//Swim movement.
			currentVelocity = Vector3.MoveTowards(currentVelocity, LocalMovement() * swimSpeed, waterFriction * superCharacterController.deltaTime);
			//Apply friction to slow character to a halt on vertical axis.
			currentVelocity = Vector3.MoveTowards(currentVelocity, new Vector3(currentVelocity.x, 0, currentVelocity.z), (waterFriction * 5) * superCharacterController.deltaTime);
			if(rpgCharacterInputController.current.jumpInput && (rpgCharacterInputController.inputStrafe || rpgCharacterInputController.inputTargetBlock > 0.8f)){
				SwimDescent();
			}
			else if(rpgCharacterInputController.current.jumpInput){
				SwimAscend();
			}
		}

		public void SwimAscend(){
			currentVelocity += superCharacterController.up * CalculateJumpSpeed(1, gravity);
			animator.SetTrigger("JumpTrigger");
		}

		public void SwimDescent(){
			currentVelocity -= superCharacterController.up * CalculateJumpSpeed(1, gravity);
			animator.SetBool("Strafing", true);
			animator.SetTrigger("JumpTrigger");
		}

		void Roll_SuperUpdate(){
			if(rollNumber == 1){
				currentVelocity = Vector3.MoveTowards(currentVelocity, transform.forward * rollSpeed, groundFriction * superCharacterController.deltaTime);
			}
			if(rollNumber == 2){
				currentVelocity = Vector3.MoveTowards(currentVelocity, transform.right * rollSpeed, groundFriction * superCharacterController.deltaTime);
			}
			if(rollNumber == 3){
				currentVelocity = Vector3.MoveTowards(currentVelocity, -transform.forward * rollSpeed, groundFriction * superCharacterController.deltaTime);
			}
			if(rollNumber == 4){
				currentVelocity = Vector3.MoveTowards(currentVelocity, -transform.right * rollSpeed, groundFriction * superCharacterController.deltaTime);
			}
		}

		public void DirectionalRoll(){
			//Check which way the dash is pressed relative to the character facing.
			float angle = Vector3.Angle(rpgCharacterInputController.current.moveInput, -transform.forward);
			float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(rpgCharacterInputController.current.aimInput, transform.forward)));
			//Angle in [-179,180].
			float signed_angle = angle * sign;
			//Angle in 0-360.
			float angle360 = (signed_angle + 180) % 360;
			//Deternime the animation to play based on the angle.
			if(angle360 > 315 || angle360 < 45){
				StartCoroutine(_Roll(1));
			}
			if(angle360 > 45 && angle360 < 135){
				StartCoroutine(_Roll(2));
			}
			if(angle360 > 135 && angle360 < 225){
				StartCoroutine(_Roll(3));
			}
			if(angle360 > 225 && angle360 < 315){
				StartCoroutine(_Roll(4));
			}
		}

		/// <summary>
		/// Character Roll.
		/// </summary>
		/// <param name="1">Forward.</param>
		/// <param name="2">Right.</param>
		/// <param name="3">Backward.</param>
		/// <param name="4">Left.</param>
		public IEnumerator _Roll(int roll){
			rollNumber = roll;
			currentState = RPGCharacterState.Roll;
			rpgCharacterState = RPGCharacterState.Roll;
			if(rpgCharacterController.weapon == Weapon.RELAX){
				rpgCharacterController.weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			animator.SetInteger("Action", rollNumber);
			animator.SetTrigger("RollTrigger");
			isRolling = true;
			rpgCharacterController.canAction = false;
			yield return new WaitForSeconds(rollduration);
			isRolling = false;
			rpgCharacterController.canAction = true;
			currentState = RPGCharacterState.Idle;
			rpgCharacterState = RPGCharacterState.Idle;
		}

		void ClimbLadder_EnterState(){
			if(capCollider != null){
				capCollider.center = new Vector3(0, 0.75f, 0);
			}
		}
			
		void ClimbLadder_ExitState(){
			SwitchCollisionOn();
			if(capCollider != null){
				capCollider.center = new Vector3(0, 1.25f, 0);
			}
		}

		public void SwitchCollisionOff(){
			canMove = false;
			superCharacterController.enabled = false;
			animator.applyRootMotion = true;
			if(rb != null){
				rb.isKinematic = false;
			}
		}

		public void SwitchCollisionOn(){
			canMove = true;
			superCharacterController.enabled = true;
			animator.applyRootMotion = false;
			if(rb != null){
				rb.isKinematic = true;
			}
		}

		void RotateTowardsMovementDir(){
			if(rpgCharacterInputController.current.moveInput != Vector3.zero){
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rpgCharacterInputController.current.moveInput), Time.deltaTime * rotationSpeed);
			}
		}

		void RotateTowardsTarget(Vector3 targetPosition){
			Quaternion targetRotation = Quaternion.LookRotation(targetPosition - new Vector3(transform.position.x, 0, transform.position.z));
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
		}

		void Aiming(){
//			Debug.Log("Aiming");
			for(int i = 0; i < Input.GetJoystickNames().Length; i++){
				//If right joystick is moved, use that for facing.
				if(Mathf.Abs(rpgCharacterInputController.inputAimHorizontal) > 0.8 || Mathf.Abs(rpgCharacterInputController.inputAimVertical) < -0.8){
					Vector3 joyDirection = new Vector3(rpgCharacterInputController.inputAimHorizontal, 0, -rpgCharacterInputController.inputAimVertical);
					joyDirection = joyDirection.normalized;
					Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
					transform.rotation = joyRotation;
				}
			}
			//No joysticks, use mouse aim.
			if(Input.GetJoystickNames().Length == 0){
				Plane characterPlane = new Plane(Vector3.up, transform.position);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 mousePosition = new Vector3(0, 0, 0);
				float hitdist = 0.0f;
				if(characterPlane.Raycast(ray, out hitdist)){
					mousePosition = ray.GetPoint(hitdist);
				}
				mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
				RotateTowardsTarget(mousePosition);
			}
			//Update animator with local movement values.
			animator.SetFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
			animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);
		}

		//Update animator with local movement values.
		void Strafing(Vector3 targetPosition){
//			Debug.Log("Strafing");
			animator.SetFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
			animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);
			RotateTowardsTarget(targetPosition);
		}

		public IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount){
			isKnockback = true;
			StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
			yield return new WaitForSeconds(.1f);
			isKnockback = false;
		}

		IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount){
			while(isKnockback){
				rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
				yield return null;
			}
		}

		void OnTriggerEnter(Collider collide){
			//Entering a water volume.
			if(collide.gameObject.layer == 4){
				currentState = RPGCharacterState.Swim;
				rpgCharacterState = RPGCharacterState.Swim;
			}
			//Near a ladder.
			else if(collide.transform.parent != null){
				if(collide.transform.parent.name.Contains("Ladder")){
					rpgCharacterController.isNearLadder = true;
					rpgCharacterController.ladder = collide.gameObject;
				}
			}
			//Near a cliff.
			else if(collide.transform.name.Contains("Cliff")){
				rpgCharacterController.isNearCliff = true;
				rpgCharacterController.cliff = collide.gameObject;
			}
		}

		void OnTriggerExit(Collider collide){
			//Leaving a water volume.
			if(collide.gameObject.layer == 4){
				animator.SetBool("Swimming", false);
				currentState = RPGCharacterState.Jump;
				rpgCharacterState = RPGCharacterState.Jump;
			}
			//Leaving a ladder.
			else if(collide.transform.parent != null){
				if(collide.transform.parent.name.Contains("Ladder")){
					rpgCharacterController.isNearLadder = false;
					rpgCharacterController.ladder = null;
				}
			}
			//leaving a cliff.
			else if(collide.transform.name.Contains("Cliff")){
				rpgCharacterController.isNearCliff = false;
				rpgCharacterController.cliff = null;
			}
		}

		//Keep character from moving.
		public void LockMovement(){
			canMove = false;
			animator.SetBool("Moving", false);
			animator.applyRootMotion = true;
			currentVelocity = new Vector3(0, 0, 0);
		}

		public void UnlockMovement(){
			canMove = true;
			animator.applyRootMotion = false;
		}
	}
}