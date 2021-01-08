using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritSideControl : Entity
{

    private SpiritFunction spiritFunction;
    public SpiritSideControl flip;

    public bool awake;

    public void Start()
    {
        spiritFunction = gameObject.GetComponentInParent<SpiritFunction>();
    }

    public SpiritSideControl GetFlip()
    {
        return flip;
    }

    public void SetFlip(SpiritSideControl flip)
    {
        this.flip = flip;
    }

    public bool IsAwake()
    {
        return awake;
    }

    public void SetAwake(bool awake)
    {
        this.awake = awake;
    }

    public void Awaken()
    {
        if (!IsAwake() && flip.IsAwake())
        {
            SetAwake(true);
            flip.SetAwake(false);

            spiritFunction.GetHand().GetComponent<HandAligner>().UpdateAlignment();
        }
    }

    public void Tire()
    {
        if (IsAwake() && !flip.IsAwake())
        {
            SetAwake(false);
            flip.SetAwake(true);

            spiritFunction.GetHand().GetComponent<HandAligner>().UpdateAlignment();
        }
    }

}
