using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    private static EffectsManager _instance;
    public static EffectsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EffectsManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private ParticleSystem burstParticle;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void Burst(Vector2 position)
    {
        ParticleSystem burst = Instantiate(burstParticle, position, Quaternion.identity, null);
        Destroy(burst.gameObject,  burst.main.duration + burst.main.startLifetime.constant);
    }
}
