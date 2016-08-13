using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupManager : Singleton<GroupManager>
{
    private List<Bibbit_LineSpawner> m_Spawners = new List<Bibbit_LineSpawner>();
    private Dictionary<Transform, Bibbit_LineSpawner> m_BibbitsToSpawners = new Dictionary<Transform, Bibbit_LineSpawner>();

    public void RegisterSpawner(Bibbit_LineSpawner spawner)
    {
        Debug.Assert(!m_Spawners.Contains(spawner));
        m_Spawners.Add(spawner);
        spawner.OnBibbitSpawned += OnBibbitSpawned;
        spawner.OnBibbitUnspawned += OnBibbitUnspawned;
    }

    public void UnregisterSpawner(Bibbit_LineSpawner spawner)
    {
        spawner.OnBibbitSpawned -= OnBibbitSpawned;
        spawner.OnBibbitUnspawned -= OnBibbitUnspawned;

        Debug.Assert(m_Spawners.Contains(spawner));
        m_Spawners.Remove(spawner);
    }

    void OnBibbitSpawned(Bibbit_LineSpawner spawner, Transform bibbit)
    {
        VRTK_InteractableObject bibbitInteractableObject = bibbit.GetComponent<VRTK_InteractableObject>();
        bibbitInteractableObject.InteractableObjectGrabbed += DoInteractGrab;
        bibbitInteractableObject.InteractableObjectUngrabbed += DoInteractUngrab;

        m_BibbitsToSpawners.Add(bibbit, spawner);
    }

    void OnBibbitUnspawned(Bibbit_LineSpawner spawner, Transform bibbit)
    {
        VRTK_InteractableObject bibbitInteractableObject = bibbit.GetComponent<VRTK_InteractableObject>();
        bibbitInteractableObject.InteractableObjectGrabbed -= DoInteractGrab;
        bibbitInteractableObject.InteractableObjectUngrabbed -= DoInteractUngrab;

        m_BibbitsToSpawners.Remove(bibbit);
    }

    void DoInteractGrab(object sender, InteractableObjectEventArgs e)
    {
        GameObject bibbit = e.interactingObject;
        Transform bibbitTransform = e.interactingObject.transform;
        Debug.Assert(m_BibbitsToSpawners.ContainsKey(bibbitTransform));
        Bibbit_LineSpawner spawner = m_BibbitsToSpawners[bibbitTransform];
        spawner.RemoveBibbit(bibbit);
		m_BibbitsToSpawners.Remove(bibbitTransform);
    }

    void DoInteractUngrab(object sender, InteractableObjectEventArgs e)
    {
        // Find closest spawner and add bibbit to it.
        // TODO: Use something else than distance to the spawner as it could be in some weird locations (pipe above, etc.). clinel 2016-08-13.

        GameObject bibbit = e.interactingObject;
        Transform bibbitTransform = e.interactingObject.transform;

        Bibbit_LineSpawner closestSpawner = null;
        float closestDistance = float.MaxValue;

        int nbSpawners = m_Spawners.Count;
        for (int i = 0; i < nbSpawners; ++i)
        {
            Bibbit_LineSpawner currentSpawner = m_Spawners[i];
            float distance = Vector3.Distance(bibbitTransform.position, currentSpawner.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpawner = currentSpawner;
            }
        }

        Debug.Assert(closestSpawner != null);
        Debug.Assert(!m_BibbitsToSpawners.ContainsKey(bibbitTransform));
        m_BibbitsToSpawners[bibbitTransform] = closestSpawner;
        closestSpawner.AddBibbit(bibbit);
    }
}
