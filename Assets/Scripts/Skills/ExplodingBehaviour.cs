using System.Collections;
using UnityEngine;

public class ExplodingBehaviour : Skill
{
    ParticleSystem explosionParticles;
    LayerMask blockerLayer;
    float pulsateDuration;

    // Called by ExplodeSkillDefinition.AttachTo() after AddComponent
    public void Setup(ParticleSystem particles, LayerMask layer, float duration, float skillCooldown)
    {
        explosionParticles = particles;
        blockerLayer       = layer;
        pulsateDuration    = duration;
        cooldown           = skillCooldown;
    }

    protected override void Activate()
    {
        StartCoroutine(Pulsate());
    }

    IEnumerator Pulsate()
    {
        IsActive = true;
        OnSkillStart?.Invoke();

        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsedTime < pulsateDuration)
        {
            float scaleFactor = Mathf.PingPong(elapsedTime * 3, 0.2f) + 0.8f;
            transform.localScale = originalScale * scaleFactor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        Explode();

        IsActive = false;
        OnSkillEnd?.Invoke();
    }

    void Explode()
    {
        if (explosionParticles == null) return;

        ParticleSystem instance = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        instance.Play();

        Collider2D[] blockerList = Physics2D.OverlapCircleAll(transform.position, 3, blockerLayer);
        foreach (var blocker in blockerList)
            Destroy(blocker.gameObject);
    }
}
