using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; set; }


    [SerializeField] Cell prefab;

    [SerializeField] GameObject BG;

    public int Width { get; set; }
    public int Height { get; set; }

    public float TickRate { get; set; }
    public float AliveChance { get; set; }
    float tickCount;

    public bool Pause { get; set; }

    Cell[] cells;
    List<Cell> pool;
    //bool[] oldCells;
    bool[] newCells;

    Camera mainCamera;
    UIManager uIManager;
    Vector2 prevMousePosition;

    bool isTicking;
    bool poolIsActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        mainCamera = Camera.main;
        pool = new();
    }

    public void Generate(int width, int height)
    {

        if (width == Width && height == Height)
        {
            RegenerateField();
            return;
        }

        Width = width;
        Height = height;

        if(cells != null && cells.Length != 0)
        {
            foreach(var item in cells)
            {
                item.gameObject.SetActive(false);
            }
        }

        cells = new Cell[Width * Height];
        newCells = new bool[Width * Height];
        //oldCells = new bool[width * height];

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = GetObjectFromPool(i);
            cells[i].transform.position = IndexToPosition(i);
            cells[i].SetState(AliveChance > Random.Range(0,1f));
            //oldCells[i] = aliveChance > Random.Range(0, 1f);
            //cells[i].SetState(oldCells[i]);
        }
        poolIsActive = false;
        isTicking = true;
        mainCamera.transform.position = new Vector3((Width-1) / 2f, (Height-1) / 2f, -10);
        mainCamera.orthographicSize = Height/2f;
        BG.transform.position = new Vector3((Width - 1) / 2f, (Height - 1) / 2f);
        BG.transform.localScale = new Vector3(Width, Height, 1);
    }

    public void RegenerateField()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].SetState(AliveChance > Random.Range(0, 1f));
        }
    }

    private void Update()
    {
        if(!isTicking)
        {
            return;
        }


        if (!Pause)
        {
            tickCount += Time.deltaTime;
            if (tickCount >= TickRate)
            {
                tickCount = 0;
                Tick();
            }
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));
        if (mousePosition.x >= 0 && mousePosition.x <= Width - 1 && mousePosition.y >= 0 && mousePosition.y <= Height - 1)
        {
            if (!uIManager.MouseIsOverUI && ((Input.GetMouseButton(0) && mousePosition != prevMousePosition) || Input.GetMouseButtonDown(0)))
            {
                int i = PositionToIndex(mousePosition);
                cells[i].SetState(!cells[i].State);
                newCells[i] = cells[i].State;
                prevMousePosition = mousePosition;
            }
        }
    }

    public void Tick()
    {

        for (int i = 0; i < newCells.Length; i++)
        {
            int aliveNeighbours = 0;
            Vector2 position = IndexToPosition(i);

            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if(!(position.x + (1-j) < 0 || position.x + (1 - j) > (Width-1) || position.y + (1 - k) < 0 || position.y + (1 - k) > (Height-1)) && !(j == 1 && k == 1))
                    {
                        if(cells[PositionToIndex(position.x + (1-j), position.y + (1-k))].State)
                        {
                            aliveNeighbours++;
                        }
                        /*if(oldCells[PositionToIndex(new Vector2(position.x + (1-j), position.y + (1-k)))])
                        {
                            aliveNeighbours++;
                        }*/
                    }
                }
            }

            switch (aliveNeighbours)
            {
                case < 2:
                    newCells[i] = false;
                    break;
                case > 3:
                    newCells[i] = false;
                    break;
                default:
                    if (!cells[i].State && aliveNeighbours == 3)
                    {
                        newCells[i] = true;
                    }
                    else if (cells[i].State)
                    {
                        newCells[i] = true;
                    }
                    break;
            }
        }

        for (int i = 0; i < newCells.Length; i++)
        {
            cells[i].SetState(newCells[i]);
            //oldCells[i] = newCells[i];
        }
    }

    Vector2 IndexToPosition(int index)
    {
        return new Vector2Int(index % Width, index / Width);
    }

    int PositionToIndex(Vector2 position)
    {
        return Mathf.RoundToInt(position.x + position.y * Width);
    }

    int PositionToIndex(float x, float y)
    {
        return Mathf.RoundToInt(x + y * Width);
    }

    Cell GetObjectFromPool(int index)
    {
        if(!poolIsActive && index < pool.Count)
        {
            pool[index].gameObject.SetActive(true);
            return pool[index];

        }

        poolIsActive = true;
        Cell cell = Instantiate(prefab, transform.position, Quaternion.identity);
        pool.Add(cell);
        return cell;
    }
}
