using Unity.Netcode;
using UnityEngine;

public class Break_Ghost : NetworkBehaviour
{
    public GameObject Ghost_Normal;
    public GameObject Ghost_Parts;
    public GameObject Ghost_Part_Top;
    public GameObject Ghost_Part_Mid;
    public GameObject Ghost_Part_Bottom;
    public Animator Ghost_Anim;

    [SerializeField] private float explosion_Force = 1.0f;
    [SerializeField, Range(0.0f, 60.0f)] private float respawn_Time = 5.0f;

    private NetworkVariable<bool> is_Broken = new NetworkVariable<bool>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    private bool was_broken;
    private Vector3 part_pos_top;
    private Vector3 part_pos_mid;
    private Vector3 part_pos_bottom;
    private float time_remaining;

    private void Start()
    {
        Ghost_Normal.SetActive(true);
        Ghost_Parts.SetActive(false);

        this.part_pos_top = Ghost_Part_Top.transform.position;
        this.part_pos_mid = Ghost_Part_Mid.transform.position;
        this.part_pos_bottom = Ghost_Part_Bottom.transform.position;

        this.was_broken = false;
    }

    private void Update()
    {
        if(this.is_Broken.Value != this.was_broken)
        {
            // Either freshly broken or freshly respawned
            Ghost_Parts.SetActive(this.is_Broken.Value);
            Ghost_Normal.SetActive(!this.is_Broken.Value);

            this.was_broken = is_Broken.Value;
            this.time_remaining = this.respawn_Time;

            if (this.is_Broken.Value)
            {
                Ghost_Part_Top.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Ghost_Part_Mid.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Ghost_Part_Bottom.GetComponent<Rigidbody>().velocity = Vector3.zero;

                Ghost_Part_Top.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * this.explosion_Force, ForceMode.Impulse);
                Ghost_Part_Mid.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * this.explosion_Force, ForceMode.Impulse);
                Ghost_Part_Bottom.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * this.explosion_Force, ForceMode.Impulse);
            }
            else
            {
                Ghost_Part_Top.transform.position = this.part_pos_top;
                Ghost_Part_Mid.transform.position = this.part_pos_mid;
                Ghost_Part_Bottom.transform.position = this.part_pos_bottom;
            }
        }
        else
        {
            if (this.is_Broken.Value && IsServer)
            {
                this.time_remaining -= Time.deltaTime;

                if (this.time_remaining <= 0)
                {
                    // Respawn the ghost!
                    this.is_Broken.Value = false;
                    this.is_Broken.SetDirty(true);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void BreakGhostServerRpc()
    {
        if (IsOwner)
        {
            this.is_Broken.Value = true;
            this.is_Broken.SetDirty(true);
        }
        
    }
}
