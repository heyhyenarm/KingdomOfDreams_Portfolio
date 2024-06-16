using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPieces : MonoBehaviour
{
    public Text txtMagicAmount;
    public Text txtSpeedAmount;
    public Text txtWisdomAmount;
    public Text txtDetoxAmount;

    public Text txtMagicRequire;
    public Text txtSpeedRequire;
    public Text txtWisdomRequire;
    public Text txtDetoxRequire;

    public Text txtLevel;
    public Image deem;

    public void Start()
    {
       
    }
    public void Init()
    {
        this.Refresh();

        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.REFRESH_UI_MAGICTOOL, new EventHandler((type) =>
        {
            this.Refresh();

        }));
    }

    public void Refresh()
    {
        for (int i = 0; i < InfoManager.instance.DreamPieceInfo.Count; i++)
        {
            var infoPiece = InfoManager.instance.DreamPieceInfo[i];
            //var dataPiece = LHMDataManager.instance.GetTDreamPieceData(infoPiece.id);
            var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;
            var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == myLevel + 1);
            switch (infoPiece.id)
            {
                case 600:
                    this.txtMagicAmount.text = infoPiece.amount.ToString();
                    break;

                case 601:
                    this.txtSpeedAmount.text = infoPiece.amount.ToString();
                    break;

                case 602:
                    this.txtDetoxAmount.text = infoPiece.amount.ToString();
                    break;

                case 603:
                    this.txtWisdomAmount.text = infoPiece.amount.ToString();
                    break;
            }

            if(data != null)
            {
                this.txtMagicRequire.text = data.magic_piece_require.ToString();
                this.txtSpeedRequire.text = data.speed_piece_require.ToString();
                this.txtDetoxRequire.text = data.detox_piece_require.ToString();
                this.txtWisdomRequire.text = data.wisdom_piece_require.ToString();
            }
            else
            {
                this.txtMagicRequire.text = "0";
                this.txtSpeedRequire.text = "0";
                this.txtDetoxRequire.text = "0";
                this.txtWisdomRequire.text = "0";
            }

        }
    }

    public void ResetUIPiece()
    {

        for (int i = 0; i < InfoManager.instance.DreamPieceInfo.Count; i++)
        {
            var infoPiece = InfoManager.instance.DreamPieceInfo[i];
            //var dataPiece = LHMDataManager.instance.GetTDreamPieceData(infoPiece.id);
            var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;
            var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == myLevel + 1);
            switch (infoPiece.id)
            {
                case 600:
                    infoPiece.amount = infoPiece.amount - data.magic_piece_require;
                    this.txtMagicAmount.text = infoPiece.amount.ToString();
                    break;

                case 601:
                    infoPiece.amount = infoPiece.amount - data.speed_piece_require;
                    this.txtSpeedAmount.text = infoPiece.amount.ToString();
                    break;

                case 602:
                    infoPiece.amount = infoPiece.amount - data.detox_piece_require;
                    this.txtDetoxAmount.text = infoPiece.amount.ToString();
                    break;

                case 603:
                    infoPiece.amount = infoPiece.amount - data.wisdom_piece_require;
                    this.txtWisdomAmount.text = infoPiece.amount.ToString();
                    break;
            }

            if (data != null)
            {
                this.txtMagicRequire.text = data.magic_piece_require.ToString();
                this.txtSpeedRequire.text = data.speed_piece_require.ToString();
                this.txtDetoxRequire.text = data.detox_piece_require.ToString();
                this.txtWisdomRequire.text = data.wisdom_piece_require.ToString();
            }
            else
            {
                this.txtMagicRequire.text = "0";
                this.txtSpeedRequire.text = "0";
                this.txtDetoxRequire.text = "0";
                this.txtWisdomRequire.text = "0";
            }

        }
    }

}
