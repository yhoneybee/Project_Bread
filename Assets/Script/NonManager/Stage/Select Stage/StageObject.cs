using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    public SpriteRenderer[] stars;
    public SpriteRenderer theme_number_image;
    public SpriteRenderer[] stage_number_image;

    private int _star_count;
    public int star_count
    {
        get
        {
            return _star_count;
        }
        set
        {
            _star_count = value;
            SetStarsColor();
        }
    }
    public bool is_startable { get; set; }

    void SetStarsColor()
    {
        for (int i = 0; i < star_count; i++)
        {
            stars[i].color = Color.white;
        }
    }
}
