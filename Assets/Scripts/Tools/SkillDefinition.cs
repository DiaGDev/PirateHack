using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill")]
public class SkillDefinition : ScriptableObject
{
    public enum SkillType { None, Explode }

    public SkillType skillType;
    public int tokens = -1; // -1 = no limit

    // Explode fields
    public ParticleSystem explosionParticles;
    public LayerMask blockerLayer;
    public float pulsateDuration = 3f;
    public float cooldown = 9f;

    public Skill AttachTo(GameObject target)
    {
        switch (skillType)
        {
            case SkillType.Explode:
                ExplodingBehaviour b = target.AddComponent<ExplodingBehaviour>();
                b.Setup(explosionParticles, blockerLayer, pulsateDuration, cooldown);
                return b;
            default:
                return null;
        }
    }
}
