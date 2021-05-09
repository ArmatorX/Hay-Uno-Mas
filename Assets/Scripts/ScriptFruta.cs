using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptFruta : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private GameController gameController;
    private bool isNewObject;

    public GameController GameController { get => gameController; set => gameController = value; }
    public bool IsNewObject { get => isNewObject; set => isNewObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        //ChangePositionToRandom();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSprite(Sprite sprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }

    //public void ChangePositionToRandom()
    //{
    //    Vector3 position = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f));

    //    while (IsPositionOccupied(position))
    //    {
    //        position = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f));
    //    }

    //    this.transform.position = position;
    //}

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsNewObject)
            {
                GameController.NextLevel();
            } else
            {
                GameController.IncrementErrors();
            }
        }
    }

    
}
