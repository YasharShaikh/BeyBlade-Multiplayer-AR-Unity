using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleScript : MonoBehaviourPun
{
    [Header("Spin Speed Settings")]
    [SerializeField] float startSpinSpeed;
    [SerializeField] float currentSpinSpeed;

    [Header("UI")]
    [SerializeField] Image img_SpinSpeedbar;
    [SerializeField] TextMeshProUGUI text_SpinSpeedRatio;
    [SerializeField] GameObject ui_3d;
    [SerializeField] GameObject ui_DeathPanel;
    GameObject deathPanel;

    [Header("Damage Coefficients")]
    [SerializeField] float common_dmg_coefficiant = 0.04f;
    [SerializeField] float doDmg_coefficient_attacker = 10.0f;
    [SerializeField] float getDmg_coefficient_attacker = 10.0f;
    [SerializeField] float doDmg_coefficient_defender = 0.75f;
    [SerializeField] float getDmg_coefficient_defender = 0.2f;

    [Header("Gameplay Balancing")]
    [SerializeField] float maxDamageCap = 400f;
    [SerializeField] float minSpinSpeedToLive = 100f;
    [SerializeField] float respawnDelay = 8f;

    [Header("Player Type")]
    public bool isAttacker;
    public bool isDefender;

    [Header("Spin Burst Settings")]
    [SerializeField] float spinBurstThreshold = 0.1f;   // 10% of startSpinSpeed
    [SerializeField] float spinBurstMultiplier = 1.5f;  // How much speed is restored
    [SerializeField] float spinBurstChance = 0.3f;      // 30% chance
    bool hasTriggeredSpinBurst;

    [Header("Momentum Steal Settings")]
    [SerializeField] float momentumStealRatio = 0.25f;  // % of damage attacker gains

    bool isDead;
    Rigidbody rb;
    Spinner spinner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spinner = GetComponent<Spinner>();
        startSpinSpeed = spinner.spinSpeed;
        currentSpinSpeed = startSpinSpeed;

        UpdateSpinUI();
    }

    void Start()
    {
        CheckPlayerType();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody enemyRb = collision.collider.GetComponent<Rigidbody>();
            if (!enemyRb) return;

            float speed = rb.linearVelocity.magnitude;
            float enemySpeed = enemyRb.linearVelocity.magnitude;

            if (speed > enemySpeed)
            {
                float default_DmgAmt = rb.linearVelocity.magnitude * 3600.0f * common_dmg_coefficiant;

                if (isAttacker)
                {
                    default_DmgAmt *= doDmg_coefficient_attacker;
                }
                else if (isDefender)
                {
                    default_DmgAmt *= doDmg_coefficient_defender;
                }

                PhotonView enemyView = collision.collider.GetComponent<PhotonView>();
                if (enemyView != null && enemyView.IsMine)
                {
                    PhotonView attackerView = GetComponent<PhotonView>();
                    int attackerId = attackerView != null ? attackerView.ViewID : -1;

                    enemyView.RPC("DoDamage", RpcTarget.AllBuffered, default_DmgAmt, attackerId);
                }
            }
        }
    }

    void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else
        {
            isAttacker = false;
            isDefender = true;

            spinner.spinSpeed = 4400.0f;
            startSpinSpeed = spinner.spinSpeed;
            currentSpinSpeed = spinner.spinSpeed;

            UpdateSpinUI();
        }
    }

    [PunRPC]
    void DoDamage(float _damage, int attackerViewId = -1)
    {
        if (isDead) return;

        // Apply defender coefficients
        if (isAttacker)
        {
            _damage *= getDmg_coefficient_attacker;
            if (_damage > 1000.0f) _damage = maxDamageCap;
        }
        else if (isDefender)
        {
            _damage *= getDmg_coefficient_defender;
        }

        // Subtract damage
        spinner.spinSpeed -= _damage;
        currentSpinSpeed = spinner.spinSpeed;

        //  Spin Burst check
        TrySpinBurst();

        UpdateSpinUI();

        // If attacker exists → give them some stolen momentum
        if (attackerViewId != -1)
        {
            PhotonView attackerView = PhotonView.Find(attackerViewId);
            if (attackerView != null)
            {
                attackerView.RPC("GainMomentum", RpcTarget.AllBuffered, _damage * momentumStealRatio);
            }
        }

        // Death check
        if (currentSpinSpeed < minSpinSpeedToLive)
        {
            Die();
        }
    }

    void TrySpinBurst()
    {
        if (hasTriggeredSpinBurst) return; // Only once per life
        if (currentSpinSpeed > startSpinSpeed * spinBurstThreshold) return;

        if (Random.value <= spinBurstChance)
        {
            float boost = startSpinSpeed * spinBurstMultiplier * 0.1f; // 10% base restored with multiplier
            spinner.spinSpeed += boost;
            currentSpinSpeed = spinner.spinSpeed;

            Debug.Log($"{gameObject.name} activated SPIN BURST comeback!");
            hasTriggeredSpinBurst = true;
            UpdateSpinUI();
        }
    }

    [PunRPC]
    void GainMomentum(float amount)
    {
        if (isDead) return;

        spinner.spinSpeed += amount;
        currentSpinSpeed = spinner.spinSpeed;
        UpdateSpinUI();
    }

    void Die()
    {
        isDead = true;
        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinner.spinSpeed = 0.0f;

        ui_3d.gameObject.SetActive(false);

        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (deathPanel == null)
        {
            deathPanel = Instantiate(ui_DeathPanel, canvas.transform);
        }
        else
        {
            deathPanel.SetActive(true);
        }

        Text text_respawnTime = deathPanel.transform.Find("RespawnTimeText").GetComponent<Text>();
        float respawnTime = respawnDelay;

        while (respawnTime > 0.0f)
        {
            text_respawnTime.text = respawnTime.ToString("F0");
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;

            GetComponent<MovementController>().enabled = false;
        }

        deathPanel?.SetActive(false);
        GetComponent<MovementController>().enabled = true;

        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void Reborn()
    {
        spinner.spinSpeed = startSpinSpeed;
        currentSpinSpeed = spinner.spinSpeed;

        UpdateSpinUI();

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        ui_3d.SetActive(true);
        isDead = false;
    }

    void UpdateSpinUI()
    {
        img_SpinSpeedbar.fillAmount = currentSpinSpeed / startSpinSpeed;
        text_SpinSpeedRatio.text = $"{currentSpinSpeed:F0}/{startSpinSpeed}";
    }
}
