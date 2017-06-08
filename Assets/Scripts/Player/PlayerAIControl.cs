using UnityEngine;
using System.Collections;
using Pathfinding.RVO;

namespace Pathfinding
{
    /** AI controller specifically made for the spider robot.
	 * The spider robot (or mine-bot) which is got from the Unity Example Project
	 * can have this script attached to be able to pathfind around with animations working properly.\n
	 * This script should be attached to a parent GameObject however since the original bot has Z+ as up.
	 * This component requires Z+ to be forward and Y+ to be up.\n
	 *
	 * It overrides the AIPath class, see that class's documentation for more information on most variables.\n
	 * Animation is handled by this component. The Animation component refered to in #anim should have animations named "awake" and "forward".
	 * The forward animation will have it's speed modified by the velocity and scaled by #animationSpeed to adjust it to look good.
	 * The awake animation will only be sampled at the end frame and will not play.\n
	 * When the end of path is reached, if the #endOfPathEffect is not null, it will be instantiated at the current position. However a check will be
	 * done so that it won't spawn effects too close to the previous spawn-point.
	 * \shadowimage{mine-bot.png}
	 *
	 * \note This script assumes Y is up and that character movement is mostly on the XZ plane.
	 */
    [RequireComponent(typeof(Seeker))]
    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_mine_bot_a_i.php")]

    public class PlayerAIControl : AIPath
    {
        /** Animation component.
                 * Should hold animations "awake" and "forward"
                 */

        public float StopDist;
        public Animator _Anim;
        public Animation anim;

        /** Minimum velocity for moving */
        public float sleepVelocity = 0.4F;

        /** Speed relative to velocity with which to play animations */
        public float animationSpeed = 0.2F;

        /** Effect which will be instantiated when end of path is reached.
		 * \see OnTargetReached */
        public GameObject endOfPathEffect;

        public float verticalAmount;
        public float horizontalAmount;

        public bool TargetAnim;

        [Header("Новая анимация")]
        public Transform cam;
        public Vector3 camForward;
        public Vector3 move;
        public Vector3 moveInput;

        public float RotSpeed;

        public float horizontal;
        public float vertical;

        [Header("Скорости")]
        public float sprintSpeed;
        public float sprintTime;
        public bool sprint;

        Vector3 localMove;
        public Transform targetAim;
        
    
        public new void Start()
        {
            targetAim = GameObject.Find("Aim").transform;
            //Prioritize the walking animation
            // anim["forward"].layer = 10;

            //Play all animations
            // anim.Play("awake");
            // anim.Play("forward");

            //Setup awake animations properties
            // anim["awake"].wrapMode = WrapMode.Clamp;
            // anim["awake"].speed = 0;
            // anim["awake"].normalizedTime = 1F;

            //Call Start in base script (AIPath)
            base.Start();
        }

        /** Point for the last spawn of #endOfPathEffect */
        protected Vector3 lastTarget;

        /**
		 * Called when the end of path has been reached.
		 * An effect (#endOfPathEffect) is spawned when this function is called
		 * However, since paths are recalculated quite often, we only spawn the effect
		 * when the current position is some distance away from the previous spawn-point
		 */
        public override void OnTargetReached()
        {
            if (endOfPathEffect != null && Vector3.Distance(tr.position, lastTarget) > 1)
            {
                Instantiate(endOfPathEffect, tr.position, tr.rotation);
                lastTarget = tr.position;
            }
        }

        public override Vector3 GetFeetPosition()
        {
            return tr.position;
        }
        //Vector3 ray;
        public override void Update()
        {
            //FunnelModifier FN = GetComponent<FunnelModifier>();

       

            if (Input.GetMouseButtonDown(0))
            {
                //ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //endOfPathEffect.transform.position = ray;
            }

            //Get velocity in world-space
            Vector3 velocity;

            if (canMove)
            {
                //Calculate desired velocity
                Vector3 dir = CalculateVelocity(GetFeetPosition());

                //Rotate towards targetDirection (filled in by CalculateVelocity)
               /* RotateTowards(targetDirection);

                dir.y = 0;
                if (dir.sqrMagnitude > sleepVelocity * sleepVelocity)
                {
                    //If the velocity is large enough, move
                }
                else
                {
                    //Otherwise, just stand still (this ensures gravity is applied)
                    dir = Vector3.zero;
                }*/

                if (controller != null)
                {
                    controller.SimpleMove(dir);
                    velocity = controller.velocity;
                }
                else
                {
                    //Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
                    velocity = Vector3.zero;
                }
            }
            else
            {
                velocity = Vector3.zero;
            }


            //Animation

            //Calculate the velocity relative to this transform's orientation
            Vector3 relVelocity = tr.InverseTransformDirection(velocity);
            relVelocity.y = 0;

            float dist = Vector3.Distance(transform.position, target.position);
            if (dist > StopDist)
            {
                if (velocity.sqrMagnitude <= sleepVelocity * sleepVelocity)
                {
                    //Fade out walking animation
                    //  anim.Blend("forward", 0, 0.2F);
                   // _Anim.SetFloat("Speed", 2);
                   // print(1);
                }
            }

            else
            {
                //Fade in walking animation
                //  anim.Blend("forward", 1, 0.2F);
                // _Anim.SetFloat("Speed", 1);

                //Modify animation speed to match velocity

                float speed = relVelocity.z;
                speed = speed * animationSpeed;

              //  _Anim.SetFloat("Speed", 0);
               // print(0);

            }
        }


    }


}
