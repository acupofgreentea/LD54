using UnityEngine;
using System.Collections;

public class Testy_AEC : MonoBehaviour, IAquariumEvent
{
    #region Interfaced
    public float Duration { get { return _duration; } set { _duration = value; } }
    #endregion
    #region Serialized
    [SerializeField] private float _duration;
    #endregion

    struct ThrowAction
    {
        public Vector3 ThrowFromPosition;
        public Vector3 ThrowAtPosition;
        public float WaitAmongUs;

        public ThrowAction(Vector3 from, Vector3 at, float wait)
        {
            ThrowFromPosition = from;
            ThrowAtPosition = at;
            WaitAmongUs = wait;
        }
    }

    [Header("Variables")]
    public float ThrowDelay;
    [Tooltip("Þu an göstermelik")]  public int ArrowCount = 3;

    [Header("References")]
    public Animator[] Arrows = new Animator[3];
    public Transform[] ArrowTs = new Transform[3];

    [Space(16)]
    public LayerMask FishLayers;


    //************************


    public void DependentItemPool(IAquariumEvent reference)
    {
        
    }


    const string float_arrowSpeed = "ArrowSpeed";
    const string trig_be = "be";
    float currentDuration, delay;
    ThrowAction[] actionList = new ThrowAction[3];
    int happenLoop;
    public void Happen()
    {
        //her okun fýrlatýlma çizgisini ve ayrý fýrlatýlma zamanlarýný belirle
        delay = ThrowDelay;
        for (happenLoop = 0; happenLoop < 3; happenLoop++)
        {
            actionList[happenLoop] = new ThrowAction(   new Vector3(0, 6), new Vector3(Random.Range(-5, 5), -3.5f), delay);

            ArrowTs[happenLoop].position = actionList[happenLoop].ThrowFromPosition;

            dir = actionList[happenLoop].ThrowAtPosition - actionList[happenLoop].ThrowFromPosition;
            dir.z = 0;

            ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            ArrowTs[happenLoop].rotation = Quaternion.Euler((ang + 90) * Vector3.forward);

            Arrows[happenLoop].SetFloat(float_arrowSpeed, 1 / delay);
            Arrows[happenLoop].SetTrigger(trig_be);

            delay += Random.Range(0.3f, 2);
        }

        //oklarýn geçeceði yolu göster ve biraz bekle
        StopAllCoroutines();
        StartCoroutine(ThrowWait());

        thrown = 0;
        CollectiveWait = 0;
        currentDuration = delay * 2;
    }


    float CollectiveWait;
    int thrown, waitloop;
    IEnumerator ThrowWait()
    {
        while (true)
        {
            CollectiveWait += Time.deltaTime;

            for (waitloop = thrown; waitloop < 3; waitloop++)
            {
                if (actionList[waitloop].WaitAmongUs <= CollectiveWait)
                {
                    //oku at
                    ThrowArrow();
                    thrown++;
                }
            }

            if (thrown == 3) break;

            yield return 1;
        }
    }


    RaycastHit[] results = new RaycastHit[128];
    Vector3 dir;
    float ang;
    void ThrowArrow()
    {
        //oku çevir
        dir = actionList[thrown].ThrowAtPosition - actionList[thrown].ThrowFromPosition;
        dir.z = 0;

        ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        ArrowTs[thrown].rotation = Quaternion.Euler((ang + 90) * Vector3.forward);

        //oku baþlangýç konumuna býrakýp hedef konuma fýrlat
        StartCoroutine(HitScanShot(thrown));

        //okun gittiði çizgiyi raycast ile kontrol edip bütün çarptýklarýný öldür
        for (int i = 0; i < 128; i++) results[i] = new RaycastHit();
        Physics.RaycastNonAlloc(actionList[thrown].ThrowFromPosition, actionList[thrown].ThrowAtPosition, results, 10, FishLayers);

        foreach (RaycastHit c in results)
        {
            if (c.collider == null) continue;

            //Lean.Pool.LeanPool.Despawn(c.collider.GetComponent<Fish?>());
        }
    }


    const string trig_shake = "shake";
    IEnumerator HitScanShot(int index)
    {
        //lerple
        float lerpy = 0;

        while (true)
        {
            lerpy += Time.deltaTime * 2;
            if (lerpy > 1) lerpy = 1;

            ArrowTs[index].position = Vector3.Lerp(actionList[index].ThrowFromPosition, actionList[index].ThrowAtPosition, lerpy);

            if (lerpy == 1)
            {
                // okun animasyonunu çalýþtýr ki yok olsun
                Arrows[index].SetTrigger(trig_shake);

                break;
            }

            yield return new WaitForSeconds(0.001f);
        }
    }


    private void Update()
    {
        //duration bitince yok ol
        if (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0)
            {
                currentDuration = 0;

                //ardýndan despawn ol
                Lean.Pool.LeanPool.Despawn(this);

            }
        }
    }

}
