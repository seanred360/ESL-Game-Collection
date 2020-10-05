using UnityEngine;
using System.Collections;

namespace RPGCharacterAnims{

	public class IKHands : MonoBehaviour{
		private Animator animator;
		private RPGCharacterWeaponController rpgCharacterWeaponController;
		public Transform leftHandObj;
		public Transform attachLeft;
		[Range(0, 1)] public float leftHandPositionWeight;
		[Range(0, 1)] public float leftHandRotationWeight;
		Transform blendToTransform;
		
		void Awake() {
			animator = GetComponent<Animator>();
			rpgCharacterWeaponController = GetComponent<RPGCharacterWeaponController>();
		}
		
		void OnAnimatorIK(int layerIndex){
			if(leftHandObj != null){
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPositionWeight);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);
				animator.SetIKPosition(AvatarIKGoal.LeftHand, attachLeft.position);                    
				animator.SetIKRotation(AvatarIKGoal.LeftHand, attachLeft.rotation);
			}
		}
			
		public IEnumerator _BlendIK(bool blendOn, float delay, float timeToBlend, int weapon){
			//If not using 2 handed weapon, quit function. (or 2hand axe/bow)
			if((weapon > 0 && weapon < 3) || weapon == 18 || weapon == 5 || weapon == 6 || weapon == 20){
				GetCurrentWeaponAttachPoint(weapon);
			}
			else{
				yield break;
			}
			yield return new WaitForSeconds(delay);
			float t = 0f;
			float blendTo = 0;
			float blendFrom = 0;
			if(blendOn){
				blendTo = 1;
			}
			else{
				blendFrom = 1;
			}
			while(t < 1){
				t += Time.deltaTime / timeToBlend;
				attachLeft = blendToTransform;
				leftHandPositionWeight = Mathf.Lerp(blendFrom, blendTo, t);
				leftHandRotationWeight = Mathf.Lerp(blendFrom, blendTo, t);
				yield return null;
			}
			yield break;
		}
		void GetCurrentWeaponAttachPoint(int weapon){
			if(weapon == 1){
				blendToTransform = rpgCharacterWeaponController.twoHandSword.transform.GetChild(0).transform;
			}
			else if(weapon == 2){
				blendToTransform = rpgCharacterWeaponController.twoHandSpear.transform.GetChild(0).transform;
			}
			else if(weapon == 3){
				blendToTransform = rpgCharacterWeaponController.twoHandAxe.transform.GetChild(0).transform;
			}
			else if(weapon == 5){
				blendToTransform = rpgCharacterWeaponController.twoHandCrossbow.transform.GetChild(0).transform;
			}
			else if(weapon == 18){
				blendToTransform = rpgCharacterWeaponController.rifle.transform.GetChild(0).transform;
			}
		}
	}
}