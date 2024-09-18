using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool State { get; set; }

    public int Stability { get; set; }

    [SerializeField] int maxStability = 5;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetState(bool newState)
    {
        if(newState) 
        {
            if(Stability < maxStability)
            {
                Stability += 2;
                if(Stability > maxStability)
                {
                    Stability= maxStability;
                }
            }
        }
        else
        {
            if(Stability > 0)
            {
                Stability--;
            }
        }

        State = newState;
        if(State)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = new Color(1-((float)(Stability-1)/maxStability), 1, 1-((float)(Stability - 1) / maxStability));
        }
        else
        {
            //spriteRenderer.color = Color.black;
            spriteRenderer.enabled = false;
        }
    }
}
