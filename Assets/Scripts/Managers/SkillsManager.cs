using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SkillsManager : MonoBehaviour
{
    [SerializeField] List<SkillDefinition> skills = new List<SkillDefinition>();

    IStatProvider owner;

    // Runtime token counts — copied from assets so each instance is independent
    Dictionary<SkillDefinition, int> runtimeTokens = new Dictionary<SkillDefinition, int>();

    void OnValidate()
    {
        // Remove duplicates immediately when the list is changed in the Inspector
        HashSet<SkillDefinition> seen = new HashSet<SkillDefinition>();
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            if (skills[i] == null) continue;
            if (!seen.Add(skills[i]))
                skills.RemoveAt(i);
        }
    }

    void Awake()
    {
        owner = GetComponent<IStatProvider>();

        foreach (SkillDefinition def in skills)
        {
            if (def == null) continue;

            // Copy tokens from the asset so enemies don't share state
            runtimeTokens[def] = def.tokens;

            Skill skill = def.AttachTo(gameObject);
            SkillDefinition captured = def; // capture for lambda

            skill.OnSkillStart += () =>
            {
                owner?.OnSkillActivated(captured.skillType);
                ConsumeToken(captured, skill);
            };
            skill.OnSkillEnd += () => owner?.OnSkillDeactivated(captured.skillType);
        }
    }

    public bool HasSkill(SkillDefinition.SkillType type)
    {
        foreach (SkillDefinition def in skills)
            if (def != null && def.skillType == type) return true;
        return false;
    }

    void ConsumeToken(SkillDefinition def, Skill skill)
    {
        if (runtimeTokens[def] < 0) return; // unlimited

        runtimeTokens[def]--;

        if (runtimeTokens[def] == 0)
            skill.enabled = false;
    }
}
