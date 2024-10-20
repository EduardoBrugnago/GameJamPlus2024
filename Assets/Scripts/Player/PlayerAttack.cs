using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject melee;
    bool isAttacking = false;
    float atkDuration = 0.4f;
    float atkTimer = 0f;
    PlayerController controlerPlayer;
    private void Start()
    {
        controlerPlayer = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (controlerPlayer != null)
        {
            if (!controlerPlayer.onAction) {
                CheckMelleTimer();

                /*if (Input.GetButtonDown("Fire1"))
                {
                    OnAttack();
                }*/
            }
          
        }

    }

    void OnAttack()
    {
        if (!isAttacking)
        {
            FindFirstObjectByType<PlayerSounds>().PlaySfx(PlayerSounds.SfxState.Dmg);
            melee.SetActive(true);
            GetComponent<PlayerController>().animator.Play("PAttack");
            isAttacking = true;
        }
    }

    void CheckMelleTimer()
    {
        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking = false;
                melee.SetActive(false);
            }
        }
    }
}
