using UnityEngine;

public class LineConector : MonoBehaviour
{

    [SerializeReference] private GameObject[] _objs;
    private LineRenderer line;

    void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _objs.Length; i++)
        {
            line.SetPosition(i, _objs[i].transform.position);
        }
    }
}
