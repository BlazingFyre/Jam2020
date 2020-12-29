using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function1 : Function
{
  public override void Play(GameObject target)
  {
      Debug.Log("yeet");
      Discard();
  }
  public void Play()
  {
    Play(null);
  }
}
