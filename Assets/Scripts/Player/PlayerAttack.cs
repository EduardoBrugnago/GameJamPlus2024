using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject melee;
    bool isAttacking = false;
    float atkDuration = 0.4f;
    float atkTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        CheckMelleTimer();

        if (Input.GetButtonDown("Fire1"))
        {
            OnAttack();
        }
    }

    void OnAttack()
    {
        if (!isAttacking)
        {
            melee.SetActive(true);
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
