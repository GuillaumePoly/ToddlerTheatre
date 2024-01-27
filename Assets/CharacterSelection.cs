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
        if (!Input.GetMouseButtonDown(0)) return;

            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(point, Vector2.zero);

            if (hit.collider == null) return;

                var isCharacter = hit.collider.CompareTag("Player");
                if (!isCharacter) return;

                var character = hit.transform;

                var freeMarker = markers.Where(m => m.childCount == 0).FirstOrDefault();

                if (freeMarker != null)
                {
                    character.parent = freeMarker;
                    character.position = freeMarker.position;
                }
        }
}
