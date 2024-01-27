using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public List<Transform> markers;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var character = hit.transform;

                var freeMarker = markers.FirstOrDefault();

                if (freeMarker != null) {
                    character.parent = freeMarker;
                }
            }
        }
    }
}
