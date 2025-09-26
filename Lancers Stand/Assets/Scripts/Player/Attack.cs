using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack Animation")]
    public Sprite attackSprite1;  // Image 2 - First attack frame
    public Sprite attackSprite2; // Image 3 - Second attack frame
    private Sprite idleSprite; // Image 1 - Initial/moving sprite
    public float attackAnimationDuration = 1f; // Total seconds for the animation sequence

    public GameObject spriteHolder;
    private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = spriteHolder.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Make sure they arent already attacking
        if (Input.GetKeyDown(GlobalVariables.attackKey) && !GlobalVariables.isAttacking)
        {
            idleSprite = spriteRenderer.sprite;
            StartAttackAnimation();
        }
    }
    
    public void StartAttackAnimation()
    {   
        GlobalVariables.isAttacking = true;
        StartCoroutine(AttackAnimationSequence());
    }
    
    private System.Collections.IEnumerator AttackAnimationSequence()
    {
        float frameDuration = attackAnimationDuration / 4f; // 4 transitions (1->2->3->2->1)

        GlobalVariables.isDamaging = true;
        // 1 -> 2
        spriteRenderer.sprite = attackSprite1;
        yield return new WaitForSeconds(frameDuration);
        
        // 2 -> 3
        spriteRenderer.sprite = attackSprite2;
        yield return new WaitForSeconds(frameDuration);
        
        GlobalVariables.isDamaging = false;

        // 3 -> 2
        spriteRenderer.sprite = attackSprite1;
        yield return new WaitForSeconds(frameDuration);
        
        // 2 -> 1
        spriteRenderer.sprite = idleSprite;
        yield return new WaitForSeconds(frameDuration);
        
        GlobalVariables.isAttacking = false;
    }
}
