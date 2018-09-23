using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTileController : MonoBehaviour {
	[Header("to link")]
	public SelectedTile SelectedTilePrefab;
	public RectTransform buttonsRectTransform;//to check if user didn't click buttons
											  //public RectTransform TopHalfRectTransform;//to block putting selected tile onto top half
	public RadialMenuController RadialMenuC;

	[Header("other")]
	public SelectedTile DisplayedSelectedTile;
	public Sprite DisplayedSelectedTileSprite;
	public Camera MainCamera;
	public Sprite BackgroundSprite;
	public BaseArrayController BaseArrayC;
	//public float SelectedTileWidth;//the bigger the selected size bigger

	void Start () {
		DisplayedSelectedTile = null;
		MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;
		BaseArrayC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BaseArrayController>();
		//SelectedTileWidth = 128.0f;

		DisplayedSelectedTile = Instantiate(SelectedTilePrefab.gameObject).GetComponent<SelectedTile>();
		DisplayedSelectedTileSprite = DisplayedSelectedTile.GetComponent<SpriteRenderer>().sprite;
		DisplayedSelectedTile.gameObject.SetActive(false);//hide
	}

	public void DisplayTile(Vector2 clickPosition)
	{
		Vector2 worldIndexedPosition;
		Vector2Int Indexes = BaseArrayC.GetIndexesFromWorldPosition(clickPosition, out worldIndexedPosition);

		if (Indexes.y > (BaseArrayC.GlobalBaseArrayToView_X[0].row_Y.Count / 2) - 1)
		{
			UIController.DisplayInfoForPlayer0("build only on bottom half!");
			GameObject.FindWithTag("TopRightHalf").GetComponent<TopRedSprite>().ShowRedHalf();
			HideSelectionTile();
		}
		else if(BaseArrayController.GetBase(Indexes) == BaseArrayController.NoBase)
		{
			//user clicked already occupied tile, do nothing, maybe later display upgrade option
			DisplayedSelectedTile.MyIndexes = Indexes;
			DisplayedSelectedTile.transform.position = worldIndexedPosition;
			DisplayedSelectedTile.gameObject.SetActive(true);//show

			RadialMenuC.ShowMenuOnClick(Input.mousePosition);
		}
		else
		{

			HideSelectionTile();

		}
	}

	public void Input_MouseLeftUp()
	{
		if (!RectTransformUtility.RectangleContainsScreenPoint(buttonsRectTransform, Input.mousePosition))//if user didn't click buttons
		{
			DisplayTile(MainCamera.ScreenToWorldPoint(Input.mousePosition));

            SFXController.PlaySound(SOUNDS.SELECT_BASE);
		}
		else
			HideSelectionTile();    
	}

	public void HideSelectionTile()
	{
		DisplayedSelectedTile.gameObject.SetActive(false);
		RadialMenuC.gameObject.SetActive(false);
	}
}
