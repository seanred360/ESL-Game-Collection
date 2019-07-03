/* For documentation please refer to this address:
http://peyman-mass.blogspot.com/2017/12/using-multiple-bones-to-look-at-target.html */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FwdDirection
{
	X_AXIS = 0,
	Y_AXIS = 1,
	Z_AXIS = 2,
	MINUS_X_AXIS = 3,
	MINUS_Y_AXIS = 4,
	MINUS_Z_AXIS = 5
}

[System.Serializable]
public class PerfectLookAtData
{
	private Quaternion m_DefaultRotation;
	public Transform m_Bone;
	public float m_RotationLimit = 90.0f;
	public float m_RotateAroundUpVectorWeight = 0.0f;
	public FwdDirection m_ForwardAxis;
	public FwdDirection m_ParentBoneForwardAxis;
	public bool m_ResetToDefaultRotation = false;
	public PerfecLookAtLinkedBones[] m_LinkedBones;

	public void SetDefaultRotation( Quaternion rot )
	{
		m_DefaultRotation = rot;
	}

	public Quaternion GetDefaultRotation()
	{
		return m_DefaultRotation;
	}

	public void CheckJointRotation()
	{
		if( m_RotationLimit < Mathf.Epsilon )
		{
			Debug.LogWarning( "Joint limit is zero or negative. No rotation will take effect" );
		}
	}
}

[System.Serializable]
public class PerfecLookAtLinkedBones
{
	private Quaternion m_DefaultRotation;
	private Quaternion m_LastFrameRotation;
	public Transform m_Bone;
	public bool m_ResetToDefaultRotation = false;

	public void SetDefaultRotation( Quaternion rot )
	{
		m_DefaultRotation = rot;
	}

	public Quaternion GetDefaultRotation()
	{
		return m_DefaultRotation;
	}

	public void SetLastFrameRotation( Quaternion rotation )
	{
		m_LastFrameRotation = rotation;
	}

	public Quaternion GetLastFrameRotation()
	{
		return m_LastFrameRotation;
	}
}

public class PerfectLookAt : MonoBehaviour {

    public GameObject m_TargetObject;
    private Vector3 m_UpVector = Vector3.up;
	[HideInInspector]
    public float m_Weight = 1.0f;
    public float m_LookAtBlendSpeed = 5.0f;
    private float m_BlendSpeed = 0.0f;
    public bool m_DrawDebugLookAtLines = false;
    private bool m_IsValid = true;
    public PerfectLookAtData[] m_LookAtBones;
    private Quaternion[] m_BlendedRotations;
    private Quaternion[] m_LastFrameRotations;


    /*************************************************************************/
    public float GetLookAtWeight()
    {
        return m_Weight;
    }

    /*************************************************************************/
    public void SetLookAtWeight( float weight )
    {
        m_Weight = Mathf.Clamp( weight, 0.0f, 1.0f );
        m_BlendSpeed = 0.0f;
    }

    /*************************************************************************/
    public void EnablePerfectLookat( float time, bool cancelCurrentTransition = true )
    {
        if ( Mathf.Abs( m_Weight - 1.0f ) > Mathf.Epsilon )
        {
            if ( Mathf.Abs( m_BlendSpeed ) < Mathf.Epsilon || cancelCurrentTransition )
            {
                m_BlendSpeed = ( 1.0f - m_Weight ) / time;
            }
        }
        else
        {
            m_BlendSpeed = 0.0f;
        }
    }

    /*************************************************************************/
    public void DisablePerfectLookat( float time, bool cancelCurrentTransition = true )
    {
        if ( Mathf.Abs( m_Weight ) > Mathf.Epsilon )
        {
            if ( Mathf.Abs( m_BlendSpeed ) < Mathf.Epsilon || cancelCurrentTransition )
            {
                m_BlendSpeed = -m_Weight / time;
            }
        }
        else
        {
            m_BlendSpeed = 0.0f;
        }
    }

    /*************************************************************************/
    public bool IsInTransition()
    {
        if( Mathf.Abs( m_BlendSpeed ) > Mathf.Epsilon )
        {
            return true;
        }

        return false;
    }

    /*************************************************************************/
    public float GetTimeToFinishTransition()
    {
        float ret = 0.0f;
        if ( Mathf.Abs( m_BlendSpeed ) > 10.0f * Mathf.Epsilon ) // multiplied by 10 to avoid numerical errors on division
        {
            if( m_BlendSpeed > 0.0f )// On enable with positive speed
            {
                ret = ( 1.0f - m_Weight ) / m_BlendSpeed;
            }
            else// On disable with negative speed.
            {
                ret = m_Weight / m_BlendSpeed;
            }
        }

        return ret;
    }

    /*************************************************************************/
    private Quaternion GetWorldLookAtRotation( Vector3 targetVector, Vector3 fwdVectorInWorldSpace )
    {
        Quaternion worldLookAtRotation;
        Vector3 rotationAxis;
        float angle = Vector3.Angle( fwdVectorInWorldSpace, targetVector ) * Mathf.Deg2Rad;

        if ( Mathf.Abs( angle - Mathf.PI ) < Mathf.Epsilon || angle <= Mathf.Epsilon )
        {
            rotationAxis = Vector3.up;
        }
        else
        {
            rotationAxis = Vector3.Cross( fwdVectorInWorldSpace, targetVector );
        }

        worldLookAtRotation = QuaternionFromAngleAxis( ref rotationAxis, angle );
        

        return worldLookAtRotation;
    }

    /*************************************************************************/
    private Quaternion PerfectLookAtSlerp( Quaternion a, Quaternion b, float weight )
    {
        if( Mathf.Abs( weight - 1.0f ) < Mathf.Epsilon )
        {
            return b;
        }

        if( weight < Mathf.Epsilon )
        {
            return a;
        }

        return Quaternion.Slerp( a, b, weight );
    }

    /*************************************************************************/
    private Quaternion QuaternionFromAngleAxis( ref Vector3 rotationAxis, float angleRad )
    {
        float halfAngleRad = angleRad * 0.5f;
        rotationAxis = rotationAxis.normalized * Mathf.Sin( halfAngleRad );
        float quatW = Mathf.Cos( halfAngleRad );
        return new Quaternion( rotationAxis.x, rotationAxis.y, rotationAxis.z, quatW );
    }

    /*************************************************************************/
    private void CheckForForceDefaultRotation()
    {
        for ( int i = 0; i < m_LookAtBones.Length; i++ )
        {
            PerfectLookAtData lookAtData = m_LookAtBones[ i ];
            
            if ( lookAtData.m_ResetToDefaultRotation )
            {
                lookAtData.m_Bone.transform.localRotation = lookAtData.GetDefaultRotation();
            }

            for ( int j = 0; j < lookAtData.m_LinkedBones.Length; j++ )
            {
                PerfecLookAtLinkedBones linkedBoneData = lookAtData.m_LinkedBones[j];
                if ( linkedBoneData.m_ResetToDefaultRotation )
                {
                    linkedBoneData.m_Bone.transform.localRotation = linkedBoneData.GetDefaultRotation();
                }
            }
        }
    }

    /*************************************************************************/
    private bool CheckValidity()
    {
        bool ret = true;

        for ( int i = 0; i < m_LookAtBones.Length; i++ )
        {
            PerfectLookAtData lookAtData = m_LookAtBones[ i ];
            if ( lookAtData.m_Bone == null )
            {
                ret = false;
                break;
            }

            for ( int j = 0; j < lookAtData.m_LinkedBones.Length; j++ )
            {
                if ( lookAtData.m_LinkedBones[ j ].m_Bone == null )
                {
                    ret = false;
                    break;
                }
            }
        }

        return ret;
    }

    /*************************************************************************/
    private float GetAngleFromQuaternionDeg( ref Quaternion inputQuat )
    {
        float ret = Mathf.Acos( inputQuat.w ) * Mathf.Rad2Deg * 2.0f;
        return ret;
    }

    /*************************************************************************/
    private float GetAngleFromQuaternionDeg( Quaternion inputQuat )
    {
        float ret = Mathf.Acos( inputQuat.w ) * Mathf.Rad2Deg * 2.0f;
        return ret;
    }

    /*************************************************************************/
    private float GetAngleFromQuaternionRad( Quaternion inputQuat )
    {
        float ret = Mathf.Acos( inputQuat.w ) * 2.0f;
        return ret;
    }

    /*************************************************************************/
    private float GetAngleFromQuaternionRad( ref Quaternion inputQuat )
    {
        float ret = Mathf.Acos( inputQuat.w ) * 2.0f;
        return ret;
    }

    /*************************************************************************/
    private Vector3 GetForwardVector( ref Transform inputTr, FwdDirection inputAxis )
    {
        switch ( inputAxis )
        {
            case FwdDirection.X_AXIS:
                return inputTr.right;

            case FwdDirection.Y_AXIS:
                return inputTr.up;

            case FwdDirection.Z_AXIS:
                return inputTr.forward;

            case FwdDirection.MINUS_X_AXIS:
                return -inputTr.right;

            case FwdDirection.MINUS_Y_AXIS:
                return -inputTr.up;

            case FwdDirection.MINUS_Z_AXIS:
                return -inputTr.forward;

            default:
                return inputTr.forward;
        }
            
    }

    /*************************************************************************/
    private Vector3 BlendTowardTargetVector( ref Transform inputTr, FwdDirection inputAxis )
    {
        switch ( inputAxis )
        {
            case FwdDirection.X_AXIS:
                return inputTr.right;

            case FwdDirection.Y_AXIS:
                return inputTr.up;

            case FwdDirection.Z_AXIS:
                return inputTr.forward;

            case FwdDirection.MINUS_X_AXIS:
                return -inputTr.right;

            case FwdDirection.MINUS_Y_AXIS:
                return -inputTr.up;

            case FwdDirection.MINUS_Z_AXIS:
                return -inputTr.forward;

            default:
                return inputTr.forward;
        }

    }


    /*************************************************************************/
    void Start ()
    {
        m_IsValid = CheckValidity();
        m_BlendedRotations = new Quaternion[ m_LookAtBones.Length ];
        m_LastFrameRotations = new Quaternion[ m_LookAtBones.Length ];

        for ( int i = 0; i < m_LookAtBones.Length; i++ )
        {
            PerfectLookAtData lookAtBoneData = m_LookAtBones[ i ];
            PerfecLookAtLinkedBones[] currentLinkedBones = lookAtBoneData.m_LinkedBones;
            lookAtBoneData.SetDefaultRotation( lookAtBoneData.m_Bone.localRotation );
            m_LastFrameRotations[ i ] = lookAtBoneData.m_Bone.localRotation;

            lookAtBoneData.CheckJointRotation();

            for ( int j = 0; j < currentLinkedBones.Length; j++ )
            {
                currentLinkedBones[ j ].SetDefaultRotation( currentLinkedBones[ j ].m_Bone.localRotation );
                currentLinkedBones[ j ].SetLastFrameRotation( currentLinkedBones[ j ].m_Bone.rotation );
            }
        }
    }

    /*************************************************************************/
    void Update()
    {
        if ( m_LookAtBones.Length > 0 )
        {
            if ( m_DrawDebugLookAtLines )
            {
                Vector3 destination = m_TargetObject.transform.position - m_LookAtBones[ 0 ].m_Bone.position;
                Debug.DrawLine( m_LookAtBones[ 0 ].m_Bone.position, m_LookAtBones[ 0 ].m_Bone.position + destination );
            }

            // if the system already in a transtion
            if ( Mathf.Abs( m_BlendSpeed ) > Mathf.Epsilon && m_IsValid )
            {
                m_Weight += m_BlendSpeed * Time.deltaTime;

                if ( m_BlendSpeed > 0.0f )
                {
                    if ( m_Weight > 1.0f )
                    {
                        m_Weight = 1.0f;
                        m_BlendSpeed = 0.0f;
                    }
                }
                else if ( m_Weight < 0.0f )
                {
                    m_Weight = 0.0f;
                    m_BlendSpeed = 0.0f;
                }
            }
            else
            {
                m_BlendSpeed = 0.0f;
            }
        }
    }

    /*************************************************************************/
    void LateUpdate ()
    {
        if ( m_TargetObject == null )
        {
            Debug.LogWarning( "No target object set for the component. Component won't work without a target object", this );
            return;
        }

        if ( !m_IsValid )
        {
            Debug.LogWarning( "Missing bones in PerfectlookAt component. Component won't work unless all bones are set", this );
            return;
        }

        if( m_Weight < Mathf.Epsilon )
        {
            //updating last frame rotation even if the weight is zero
            for ( int i = 0; i < m_LookAtBones.Length; i++ )
            {
                PerfectLookAtData lookAtBoneData = m_LookAtBones[ i ];
                m_LastFrameRotations[ i ] = lookAtBoneData.m_Bone.localRotation;

                for ( int j = 0; j < lookAtBoneData.m_LinkedBones.Length; j++ )
                {
                    lookAtBoneData.m_LinkedBones[ j ].SetLastFrameRotation( lookAtBoneData.m_LinkedBones[ j ].m_Bone.rotation );
                }

            }
            return;
        }

        //Updating the bones rotations
        if ( m_LookAtBones.Length > 0 )
        {
            CheckForForceDefaultRotation();

            Dictionary< int, Quaternion > linkedBonesRotations = new Dictionary< int, Quaternion >();
            Vector3 rotatedInitFwdVec = GetForwardVector( ref m_LookAtBones[ 0 ].m_Bone, m_LookAtBones[ 0 ].m_ForwardAxis );
            Vector3 currentFwdVec;
            Vector3 parentFwdVec;
            Vector3 targetVector = m_TargetObject.transform.position - m_LookAtBones[ 0 ].m_Bone.position; //current vector is being updated in UpdateCurrentTargetVector
            Vector3 firstBoneRotatedInitFwdVec = rotatedInitFwdVec;
            byte numBonesToRotate = 1;

            for ( int i = 0; i < m_LookAtBones.Length; i++ )
            {
                Transform currentBone = m_LookAtBones[ i ].m_Bone;
                Transform parentBone = currentBone.parent; ;
                
                float rotationLimit = m_LookAtBones[ i ].m_RotationLimit;
                bool parentExists = true;
                
                currentFwdVec = GetForwardVector( ref currentBone, m_LookAtBones[ i ].m_ForwardAxis );
                parentFwdVec = GetForwardVector( ref parentBone, m_LookAtBones[ i ].m_ParentBoneForwardAxis );

                if ( parentBone != null )
                {
                    currentFwdVec = GetForwardVector( ref currentBone, m_LookAtBones[ i ].m_ForwardAxis );
                    parentFwdVec = GetForwardVector( ref parentBone, m_LookAtBones[ i ].m_ParentBoneForwardAxis );
                    float diffAngleFromAnim = Vector3.Angle( currentFwdVec, parentFwdVec );

                    //in case animation already has a bone with relative rotation higher than the limit specified by user
                    if ( diffAngleFromAnim > rotationLimit )
                    {
                        rotationLimit = diffAngleFromAnim; 
                    }
                }
                else
                {
                    parentExists = false;
                }

                Quaternion lookAtRot = GetWorldLookAtRotation( targetVector, rotatedInitFwdVec );

                if ( m_LookAtBones[ i ].m_RotateAroundUpVectorWeight > 0.0f )
                {
                    Vector3 currentRotationAxis;
                    currentRotationAxis.x = lookAtRot.x;
                    currentRotationAxis.y = lookAtRot.y;
                    currentRotationAxis.z = lookAtRot.z;

                    //checking the up vector direction for rotation
                    float rotationSign = Mathf.Sign( Vector3.Cross( firstBoneRotatedInitFwdVec, targetVector ).y );
                    Vector3 finalUpVector = rotationSign * m_UpVector;
                    finalUpVector = Vector3.Lerp( currentRotationAxis, finalUpVector, m_LookAtBones[ i ].m_RotateAroundUpVectorWeight );


                    lookAtRot = QuaternionFromAngleAxis( ref finalUpVector, GetAngleFromQuaternionRad( lookAtRot ) );

                }

                Quaternion childRotation = lookAtRot * currentBone.rotation;

                if ( parentExists )
                {
                    //checking the angle difference 
                    float diffAngle = Mathf.Abs( Vector3.Angle( parentFwdVec, lookAtRot * currentFwdVec ) ) - rotationLimit;
                    if ( diffAngle > 0 )
                    {
                        {
                            Vector3 rotationAxis = new Vector3( lookAtRot.x, lookAtRot.y, lookAtRot.z );
                            float limitAngleRad = GetAngleFromQuaternionRad( ref lookAtRot ) + Mathf.Deg2Rad * ( -diffAngle );

                            lookAtRot = QuaternionFromAngleAxis( ref rotationAxis, limitAngleRad );
                            childRotation = lookAtRot * currentBone.rotation;

                            // blending with the rotation before assigning it to bone so it can have no effect on rotaiton limit checking
                            // and we can have updated bone position for calculating the rotated fwd vec and target vector
                            Quaternion currentBoneRotationLS = currentBone.localRotation;
                            m_LookAtBones[ i ].m_Bone.rotation = childRotation;
                            m_BlendedRotations[ i ] = PerfectLookAtSlerp( currentBoneRotationLS, m_LookAtBones[ i ].m_Bone.localRotation, m_Weight );

                            if ( m_LookAtBones[ i ].m_LinkedBones.Length > 0 )
                            {
                                linkedBonesRotations.Add( i, PerfectLookAtSlerp( Quaternion.identity, lookAtRot, m_Weight ) );
                            }
                        }

                        //
                        if ( i != m_LookAtBones.Length - 1 )
                        {
                            Vector3 currentBoneToParentPosDiff = m_LookAtBones[ 0 ].m_Bone.position - m_LookAtBones[ i + 1 ].m_Bone.position;
                            rotatedInitFwdVec = GetForwardVector( ref m_LookAtBones[ 0 ].m_Bone, m_LookAtBones[ 0 ].m_ForwardAxis );
                            rotatedInitFwdVec.Normalize();
                            rotatedInitFwdVec = currentBoneToParentPosDiff + rotatedInitFwdVec;

                            targetVector = currentBoneToParentPosDiff + ( m_TargetObject.transform.position - m_LookAtBones[ 0 ].m_Bone.position ).normalized;

                            numBonesToRotate++;

                            if ( m_DrawDebugLookAtLines )
                            {
                                Debug.DrawLine( m_LookAtBones[ i + 1 ].m_Bone.position, m_LookAtBones[ i + 1 ].m_Bone.position + rotatedInitFwdVec, Color.green );
                                Debug.DrawLine( m_LookAtBones[ i + 1 ].m_Bone.position, m_LookAtBones[ i + 1 ].m_Bone.position + targetVector, Color.red );
                            }
                        }
                    }
                    else
                    {
                        Quaternion currentBoneRotationLS = currentBone.localRotation;
                        m_LookAtBones[ i ].m_Bone.rotation = childRotation;
                        m_BlendedRotations[ i ] = PerfectLookAtSlerp( currentBoneRotationLS, m_LookAtBones[ i ].m_Bone.localRotation, m_Weight );

                        if ( m_LookAtBones[ i ].m_LinkedBones.Length > 0 )
                        {
                            linkedBonesRotations.Add( i, PerfectLookAtSlerp( Quaternion.identity, lookAtRot, m_Weight ) );
                        }

                        break;
                    }
                }
                else
                {
                    Quaternion currentBoneRotationLS = currentBone.localRotation;
                    m_LookAtBones[ i ].m_Bone.rotation = childRotation;
                    m_BlendedRotations[ i ] = PerfectLookAtSlerp( currentBoneRotationLS, m_LookAtBones[ i ].m_Bone.localRotation, m_Weight );

                    if ( m_LookAtBones[ i ].m_LinkedBones.Length > 0 )
                    {
                        linkedBonesRotations.Add( i, PerfectLookAtSlerp( Quaternion.identity, lookAtRot, m_Weight ) );
                    }

                    if( i < m_LookAtBones.Length - 1 )
                    {
                        Debug.LogWarning( "Warning Bone name doesn't have a parent. The rest of the PerfectLookAt bone chain won't work after this bone!", this );
                        break;
                    }
                }
            }


            //Setting bones rotations final pass. The initial bone rotations are set during the last loop.
            //These two loops are used for blending and smooth movement.

            //Updating last frame rotations. Two loops are separated to have less jumps in memory and be more cache friendly.
            bool isBlending = Mathf.Abs( m_Weight - 1.0f ) > Mathf.Epsilon;

            // Calculating the blend weight to blend between the current frame and last frame to achieve smooth rotations in dynamic animations with large range of movements.
            float smoothingWeight = Mathf.Clamp( m_LookAtBlendSpeed * Time.deltaTime, 0.0f, 1.0f );

            //Linearly blend the smoothingWeight to zero and its current frame value to be sure when perfect look at blend weight is zero-
            //the rotation is exactly the same as animation and no last frame is getting blended in.
            smoothingWeight = ( smoothingWeight - 1.0f ) * m_Weight + 1.0f;

            for ( int k = 0; k < m_LookAtBones.Length; k++ )
            {
                PerfectLookAtData lookAtBoneData = m_LookAtBones[ k ];
                Quaternion boneLocalRotation;
                if ( isBlending && k < numBonesToRotate )
                {
                   boneLocalRotation = m_BlendedRotations[ k ];
                }
                else
                {
                    boneLocalRotation = lookAtBoneData.m_Bone.localRotation;
                }

                Quaternion localRotationFromAnim = lookAtBoneData.m_Bone.localRotation;

                lookAtBoneData.m_Bone.localRotation = PerfectLookAtSlerp( m_LastFrameRotations[ k ], boneLocalRotation, smoothingWeight );

                m_LastFrameRotations[ k ] = lookAtBoneData.m_Bone.localRotation;
            }

            // updating linked bones
            for ( int m = 0; m < m_LookAtBones.Length; m++ )
            {
                PerfectLookAtData lookAtBoneData = m_LookAtBones[ m ];

                if ( lookAtBoneData.m_LinkedBones.Length > 0 )
                {
                    if( m < numBonesToRotate )
                    {
                        Quaternion linkedBoneLookAtQuat = linkedBonesRotations[ m ];
                        for ( int n = 0; n < lookAtBoneData.m_LinkedBones.Length; n++ )
                        {
                            Transform linkedBone = lookAtBoneData.m_LinkedBones[ n ].m_Bone;

                            linkedBone.rotation = PerfectLookAtSlerp( lookAtBoneData.m_LinkedBones[ n ].GetLastFrameRotation(),
                                linkedBoneLookAtQuat * linkedBone.rotation, smoothingWeight );
                            lookAtBoneData.m_LinkedBones[ n ].SetLastFrameRotation( linkedBone.rotation );

                        }
                    }
                    else
                    {
                        for ( int n = 0; n < lookAtBoneData.m_LinkedBones.Length; n++ )
                        {
                            lookAtBoneData.m_LinkedBones[ n ].SetLastFrameRotation( lookAtBoneData.m_LinkedBones[ n ].m_Bone.rotation );
                        }
                    }
                }
            }
        }
    }
}
