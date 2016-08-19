using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupManager : Singleton<GroupManager>
{
    private List<Bibbit_LineSpawner> m_Spawners = new List<Bibbit_LineSpawner>();
    private Dictionary<Transform, Bibbit_LineSpawner> m_BibbitsToSpawners = new Dictionary<Transform, Bibbit_LineSpawner>();

    private List<Transform> m_AdditionalGrabbedBibbits = new List<Transform>();
    private Coroutine m_WarpToHandAndParentCoroutine = null;

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

        // TODO: Don't differentiate the grabbed bibbit from the additional ones. clinel 2016-08-19.
        // Note: We could grab a bibbit that is warping toward the hand.
        if (!m_AdditionalGrabbedBibbits.Contains(bibbitTransform))
        {
            Debug.Assert(m_BibbitsToSpawners.ContainsKey(bibbitTransform));
            Bibbit_LineSpawner spawner = m_BibbitsToSpawners[bibbitTransform];

            List<Transform> neighbours = new List<Transform>();
            spawner.GetNeighbouringBibbits(bibbitTransform, ref neighbours, maxNbNeighbours: 5);

            Debug.Assert(!neighbours.Contains(bibbitTransform));

            spawner.RemoveBibbit(bibbit);
            m_BibbitsToSpawners.Remove(bibbitTransform);

            int nbNeighbours = neighbours.Count;
            for (int i = 0; i < nbNeighbours; ++i)
            {
                Transform neighbourBibbit = neighbours[i];
                spawner.RemoveBibbit(neighbourBibbit.gameObject);
                m_BibbitsToSpawners.Remove(neighbourBibbit);

                m_AdditionalGrabbedBibbits.Add(neighbourBibbit);
            }

            m_WarpToHandAndParentCoroutine = StartCoroutine(WarpGrabbedBibbitsToHandAndParent(m_AdditionalGrabbedBibbits, GameObject.Find("AdditionalBibbitHolder").transform));
        }
    }

    void DoInteractUngrab(object sender, InteractableObjectEventArgs e)
    {
        // Find closest spawner and add bibbit to it.
        // TODO: Use something else than distance to the spawner as it could be in some weird locations (pipe above, etc.). clinel 2016-08-13.

        Debug.Assert(m_WarpToHandAndParentCoroutine != null);
        // Note: Stop the coroutine just in case we ungrabbed them before they reached the hand.
        StopCoroutine(m_WarpToHandAndParentCoroutine);

        GameObject bibbit = e.interactingObject;
        Transform bibbitTransform = e.interactingObject.transform;

        // Find closest spawner
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

        // Release ungrabbed and additional bibbits
        Debug.Assert(!m_BibbitsToSpawners.ContainsKey(bibbitTransform));
        m_BibbitsToSpawners[bibbitTransform] = closestSpawner;
        closestSpawner.AddBibbit(bibbit);

        int nbGrabbedBibbits = m_AdditionalGrabbedBibbits.Count;
        for (int i = 0; i < nbGrabbedBibbits; ++i)
        {
            Transform grabbedBibbit = m_AdditionalGrabbedBibbits[i];

            grabbedBibbit.SetParent(null);
            Debug.Assert(!m_BibbitsToSpawners.ContainsKey(grabbedBibbit));
            m_BibbitsToSpawners[grabbedBibbit] = closestSpawner;
            closestSpawner.AddBibbit(grabbedBibbit.gameObject);
        }
        m_AdditionalGrabbedBibbits.Clear();
    }

    IEnumerator WarpGrabbedBibbitsToHandAndParent(List<Transform> bibbitsToWarp, Transform hand)
    {
        float GrabWarpSpeed = 15f;

        Dictionary<Transform, Vector3> bibbitsToInitialPosition = new Dictionary<Transform, Vector3>();

        int nbBibbitsToWarp = bibbitsToWarp.Count;
        for (int i = 0; i < nbBibbitsToWarp; ++i)
        {
            Transform bibbitToWarp = bibbitsToWarp[i];
            bibbitsToInitialPosition.Add(bibbitToWarp, bibbitToWarp.position);
        }

        while (bibbitsToInitialPosition.Count > 0)
        {
            foreach (Transform bibbit in bibbitsToWarp)
            {
                Vector3 initialPosition;

                if (bibbitsToInitialPosition.TryGetValue(bibbit, out initialPosition))
                {
                    float totalDistance = Vector3.Distance(initialPosition, hand.position);
                    float ratio = Vector3.Distance(initialPosition, bibbit.position) / totalDistance;
                    float duration = totalDistance / GrabWarpSpeed;
                    ratio += Time.deltaTime / duration;
                    bibbit.transform.position = Vector3.Lerp(initialPosition, hand.position, ratio);

                    if (ratio >= 1f)
                    {
                        bibbit.SetParent(hand);
                        bibbitsToInitialPosition.Remove(bibbit);
                    }
                }
            }

            yield return null;
        }
    }
}
