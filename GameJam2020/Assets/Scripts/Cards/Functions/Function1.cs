﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function1 : Function
{
  public override void Play(GameObject cardSide, GameObject target)
  {
      Debug.Log("yeeted: " + target.name);
      Discard(cardSide);
  }

    public override bool IsTargetable(GameObject target)
    {
        return true;
    }

}
