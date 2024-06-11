using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager thePlayer;
    List<MovingObject> characters;

    private void Awake()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void PreLoadCharacter()
    {
        characters = ToList();
    }

    public List<MovingObject> ToList()
    {
        List<MovingObject> tempList = new List<MovingObject>();
        MovingObject[] temp = FindObjectsOfType<MovingObject>();
        for (int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }
        return tempList;
    }

    public void Move(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName == _name)
            {
                characters[i].Move(_dir);
            }
        }
    }

    public void EventMove(string _name, string _dir, int _frequency)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName == _name)
            {
                characters[i].Move(_dir, _frequency);
            }
        }
    }

    public void SetTransparent(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName == _name)
            {
                Renderer renderer = characters[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
    }

    public void SetThorounght(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName == _name)
            {
                characters[i].boxCollider2D.enabled = false;
            }
        }
    }

    public void SetUnThorounght(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName == _name)
            {
                characters[i].boxCollider2D.enabled = true;
            }
        }
    }

    public void SetUnTransparent(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName == _name)
            {
                Renderer renderer = characters[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
        }
    }

    public void NotMove()
    {
        thePlayer.notMove = true;
    }

    public void CanMove()
    {
        thePlayer.notMove = false;
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterName == _name)
            {
                characters[i].animor.SetFloat("DirX", 0);
                characters[i].animor.SetFloat("DirY", 0);
                switch (_dir)
                {
                    case "UP":
                        characters[i].animor.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animor.SetFloat("DirY", -1f);
                        break;
                    case "LEFT":
                        characters[i].animor.SetFloat("DirX", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animor.SetFloat("DirX", 1f);
                        break;
                }
            }
        }
    }
}
