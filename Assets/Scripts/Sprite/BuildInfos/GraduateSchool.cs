using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraduateSchool : BuildControl
{

    public string explain;
    public override void OnAvailable()
    {
        AddTecSpeed();
    }

    public override void OnBuild()
    {
        AddTecSpeed();
       
    }

    public override void OnDismantle()
    {
        ReduceTecSpeed();
    }

    public override void OnNotAvailable()
    {
        ReduceTecSpeed();
    }

    public override void Left()
    {
       
    }

    public override void Right()
    {
        Debug.LogWarning("right");
        DisplayBoard.Show(this,gameObject.transform.position, explain);
    }

    private void AddTecSpeed()
    {
        GraduateSchool[] grs = GameObject.FindObjectsOfType<GraduateSchool>();
        if (grs.Length == 1)
        {
            GameObject.FindObjectOfType<TechnologyManager>().ChangeEfficiency(1f);
        }
        if (grs.Length > 1 && grs.Length < 5)
        {
            GameObject.FindObjectOfType<TechnologyManager>().ChangeEfficiency(0.25f);
        }
        if (grs.Length >= 5)
        {
            GameObject.FindObjectOfType<TechnologyManager>().ChangeEfficiency(0.1f);
        }
    }

    private void ReduceTecSpeed()
    {
        GraduateSchool[] grs = GameObject.FindObjectsOfType<GraduateSchool>();
        if (grs.Length == 1)
        {
            GameObject.FindObjectOfType<TechnologyManager>().ChangeEfficiency(-1f);
        }
        if (grs.Length > 1 && grs.Length < 5)
        {
            GameObject.FindObjectOfType<TechnologyManager>().ChangeEfficiency(-0.25f);
        }
        if (grs.Length >= 5)
        {
            GameObject.FindObjectOfType<TechnologyManager>().ChangeEfficiency(-0.1f);
        }
    }
}
