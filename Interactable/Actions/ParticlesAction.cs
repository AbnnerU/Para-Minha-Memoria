
using UnityEngine;

public class ParticlesAction : InteractionAction
{
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private bool play;

    public override void ExecuteAction()
    {
        if (particles == null)
            return;

        if (play)
            particles.Play();
        else
            particles.Stop();
    }
}
