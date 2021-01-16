using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardRenderer : MonoBehaviour
{
    private GlobalVariables globalVariables;
    private GameObject cardPrefab;
    private GameObject cardRender;
    private CardData cardData;
    void Start()
    {
        this.globalVariables = GameObject.Find("EventSystem").GetComponent<GlobalVariables>();
        this.cardPrefab = globalVariables.cardPrefab;

        //This grabs the data for this individual card from the data of all the cards in GlobalVariables.
        cardData = globalVariables.GetCardData("Test Card");
        RenderCard();
    }

    //This creates the image, backing, and text for a card.
    private void RenderCard()
    {
      cardRender = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
      /*
      Although the transform hierarchy is not a good organizational structure in general, in this case it is necessary in order to render the Image components.
      (Images only render when they are children of Canvases.)
      Additionally, for now, it's convenient to have the card prefab's position be relative to the card gameObject.
      */
      cardRender.GetComponent<RectTransform>().transform.SetParent(this.transform);
      EditCardArt(cardData.cardArt);
      EditCardText(cardData.cardText);
    }

    //TODO: When we have distinct card backings, this is where we're going to use them.
    private void EditBacking()
    {

    }

    private void EditCardArt(string spriteName)
    {
      Sprite sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
      cardRender.transform.Find("CardImage").GetComponent<Image>().sprite = sprite;
    }


    private void EditCardText(string text)
    {
      cardRender.transform.Find("CardText").GetComponent<TextMeshProUGUI>().text = text;
    }

}
