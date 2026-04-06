using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    PlayerMovement movement;
    Rigidbody2D rb;

    Animator humanAnim;
    Animator catAnim;

    void Awake()
    {
        movement  = GetComponent<PlayerMovement>();
        rb        = GetComponent<Rigidbody2D>();
        humanAnim = movement.humanVisual.GetComponent<Animator>();
        catAnim   = movement.catVisual.GetComponent<Animator>();
    }

    void Update()
    {
        // Only drive the active form's animator
        if (movement.currentForm == PlayerMovement.PlayerForm.Cat)  //determine cat form anim
        {
            bool isWalking = movement.isGrounded && movement.inputX != 0f;
            catAnim.SetBool("isWalking", isWalking);
        }
        else if(movement.currentForm == PlayerMovement.PlayerForm.Human)  //determine human form anim
        {
            bool isWalkingH = movement.isGrounded && movement.inputX != 0f;
            humanAnim.SetBool("isWalkingH", isWalkingH);
        }
        // Human animations not implemented yet — will be added later
    }
}
