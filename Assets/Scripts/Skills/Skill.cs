using UnityEngine;

public interface IStatProvider
{
    float GetStat(string stat);
    void OnSkillActivated(SkillDefinition.SkillType skill);
    void OnSkillDeactivated(SkillDefinition.SkillType skill);
}

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown = 9f;

    float timer = 0f;

    public bool IsActive { get; protected set; }

    public System.Action OnSkillStart;
    public System.Action OnSkillEnd;

    void Update()
    {
        timer += Time.deltaTime;

        if (!IsActive && timer >= cooldown)
        {
            timer = 0f;
            Activate();
        }
    }

    protected abstract void Activate();
}
