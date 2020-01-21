﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MoleGame : MonoBehaviour, IVirtualButtonEventHandler
{
  const float low = -0.1f, high = 0.03f, difficulty = 3.5f;  // difficulty should vary from 1 to 5
  float timeElapsed = 0f, speed = 20f;  // speed is speed of mole up and mole down
  const int size = 3;
  int index, score = 0;
  enum dir { up, down }
  dir direction = dir.up;
  System.Random random = new System.Random();
  List<GameObject> moles = new List<GameObject>() { null, null, null };  // length should be same as size, all null
  GameObject temp;
  TextMesh scoreText;

  public void OnButtonPressed(VirtualButtonBehaviour vb)
  {
    if (vb.VirtualButtonName[7].ToString() == (index + 1).ToString())
    {
      timeElapsed = 0f;
      direction = dir.down;
      score += 10;
      scoreText.text = "Score: " + score.ToString();
    }
  }

  public void OnButtonReleased(VirtualButtonBehaviour vb) { }

  void Start()
  {
    scoreText = FindObjectOfType<TextMesh>();

    for (int i = 1; i < size + 1; i++)
      moles[i - 1] = GameObject.Find("mole " + i.ToString());

    index = random.Next(size);

    for (int i = 1; i < size + 1; i++)
    {
      temp = GameObject.Find("button " + i.ToString());
      temp.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }
  }

  void Update()
  {
    timeElapsed += Time.deltaTime;

    if (timeElapsed >= 6 - difficulty)
    {
      timeElapsed = 0f;
      direction = dir.down;
    }

    if (moles[index].transform.localPosition.y < high && direction == dir.up)
      moles[index].transform.Translate(Vector3.up * speed * Time.deltaTime);
    else if (moles[index].transform.localPosition.y > low && direction == dir.down)
      moles[index].transform.Translate(Vector3.down * speed * Time.deltaTime);
    else if (moles[index].transform.localPosition.y <= low)
    {
      direction = dir.up;
      index = random.Next(size);
    }
  }
}
