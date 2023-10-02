using UnityEngine;

public class BombDrop_AEC : MonoBehaviour, IAquariumEvent
{
    #region Interfaced
    public float Duration { get { return _duration; } set { _duration = value; } }
    #endregion
    #region Serialized
    [SerializeField] private float _duration;
    #endregion

    [Header("References")]
    public Animator Bomb;
    public Transform BombT;

    [Space(16)]
    public LayerMask FishLayers;


    //************************


    public void DependentItemPool(IAquariumEvent reference)
    {
        
    }


    float currentDuration;
    float horizontalSelection;
    const string trig_Ignited = "Ignited";
    public void Happen()
    {
        //rasgele bir konum se�ip oraya bombay� yukar�dan b�rak
        horizontalSelection = Random.Range(-5,5);
        BombT.position = new Vector3(horizontalSelection, 5, -3);

        //bomba fitil animasyonu sergilesin
            Bomb.SetTrigger(trig_Ignited);

        currentDuration = Duration;
    }


    Collider[] results = new Collider[128];
    const string trig_boom = "boom";
    private void Update()
    {
        //duration bitince bombay� patlatma animasyonu oynat
        if (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0)
            {
                currentDuration = -3;

                Bomb.SetTrigger(trig_boom);

                //ve bomban�n etraf�nda yar� �ap�na g�re bir overlapspherenonalloc kullan ve dokundu�u bal�k bulursa �ld�rs�n
                for (int i = 0; i < 128; i++) results[i] = null;
                Physics.OverlapSphereNonAlloc(BombT.position, 4, results, FishLayers);

                foreach (Collider c in results)
                {
                    if (c == null) continue;

                    //Lean.Pool.LeanPool.Despawn(c.GetComponent<Fish?>());
                    c.GetComponent<FishHealth>().Die();
                }

            }
        }
        else if (currentDuration < 0)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= 0)
            {
                currentDuration = 0;

                //ard�ndan despawn ol
                Lean.Pool.LeanPool.Despawn(this);

            }
        }
    }

}
