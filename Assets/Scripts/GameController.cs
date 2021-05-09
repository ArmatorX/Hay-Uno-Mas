using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
    private List<Sprite> spritesLeft;
    private List<GameObject> objectsShowing;
    private List<ScriptFruta> fruitsShowing;
    private int level;
    private bool isFirstLevel;

    private int errors;

    public List<Sprite> SpriteList;
    public GameObject ElementoFrutaPrefab;
    public List<Sprite> SpritesLeft { get => spritesLeft; set => spritesLeft = value; }
    public List<GameObject> ObjectsShowing { get => objectsShowing; set => objectsShowing = value; }
    public List<ScriptFruta> FruitsShowing { get => fruitsShowing; set => fruitsShowing = value; }
    public int Level { get => level; set => level = value; }
    public bool IsFirstLevel { get => isFirstLevel; set => isFirstLevel = value; }
    public int Errors { get => errors; set => errors = value; }

    // Start is called before the first frame update
    void Start()
    {
        Errors = 0;
        Level = 3;
        IsFirstLevel = true;
        FruitsShowing = new List<ScriptFruta>();
        ObjectsShowing = new List<GameObject>();

        ShuffleSpriteList();
        SpritesLeft = new List<Sprite>(SpriteList);

        StartLevel();
    }

    void StartLevel()
    {
        if (Level > 4)
        { 
            StartCoroutine(Upload());
            Debug.Log("El tiempo total de juego fue " + Time.time + " s.");
            Debug.Log("El paciente cometió " + Errors + " errores.");
        } else
        {
            if (!IsFirstLevel)
            {
                ResetSpritesLeft();
                DestroyAllFruits();
            }
            SpawnNFruits(Level);
        }
    }

    void ResetSpritesLeft()
    {
        SpritesLeft = new List<Sprite>(SpriteList);
    }

    void DestroyAllFruits()
    {
        ObjectsShowing.ForEach((fruitObject) => Destroy(fruitObject));
        ObjectsShowing.Clear();
        FruitsShowing.Clear();
    }

    void SpawnNFruits(int n)
    {
        for (int i = 0; i < n; i ++)
        {
            SpawnFruit();
        }

        FruitsShowing[n-1].IsNewObject = true;
        Debug.Log(FruitsShowing[n-1].spriteRenderer.sprite.name);
        Debug.Log(FruitsShowing.Count);
        Debug.Log(FruitsShowing[n-1].transform.position);
    }

    // Instancia un objeto nuevo
    void SpawnFruit()
    {
        Vector3 position = GetUnoccupiedPosition();
        GameObject fruta = Instantiate(ElementoFrutaPrefab, position, Quaternion.identity, GetComponent<Transform>());
        ScriptFruta sFruta = fruta.GetComponent<ScriptFruta>();
        sFruta.GameController = this;
        FruitsShowing.Add(sFruta);
        ObjectsShowing.Add(fruta);
        sFruta.setSprite(SpritesLeft[0]);
        sFruta.IsNewObject = false;
        SpritesLeft.RemoveAt(0);
    }

    public void NextLevel()
    {
        if (IsFirstLevel)
        {
            IsFirstLevel = false;
        }
        IncrementLevel();
        StartLevel();
    }

    public void ShuffleSpriteList()
    {
        List<Sprite> sprites = new List<Sprite>();
        List<Sprite> spriteListAux = new List<Sprite>(SpriteList);

        for (int i = 0; i < SpriteList.Count; i++)
        {
            int rand = Random.Range(0, spriteListAux.Count);
            sprites.Add(spriteListAux[rand]);
            spriteListAux.RemoveAt(rand);
        }

        SpriteList = sprites;
    }

    public void IncrementLevel()
    {
        Level++;
    }

    Vector3 GetUnoccupiedPosition()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(-8, 8), Random.Range(-3.5f, 3.5f));
        } while (IsPositionOccupied(position));

        return position;
    }

    bool IsPositionOccupied(Vector2 position)
    {
        Vector2 p1 = position - new Vector2(1f, 1f);
        Vector2 p2 = position + new Vector2(1f, 1f);
        Collider2D collider = Physics2D.OverlapArea(p1, p2);

        if (collider != null)
        {
            return true;
        }

        return false;
    }

    public void IncrementErrors()
    {
        Errors++;
    }

    public IEnumerator Upload()
    {
        yield return new WaitForEndOfFrame();

        Juego juego = new Juego(Time.time, Errors, "HayUnoMas");
        string postData = JsonUtility.ToJson(juego);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(postData);

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("Content-Type", "application/json");
        parameters.Add("Content-Length", postData.Length.ToString());

        WWW www = new WWW("http://localhost:8080/juegos", bytes, parameters);

        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
