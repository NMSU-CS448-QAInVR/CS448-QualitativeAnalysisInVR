using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

/*
    Check SaveFormat for more info.

    The savings of 2D drawings of a notecard will be stored externally as a PNG file. The name of the file is texture_file_name, the folder where the file is saved_folder. The saved_folder is currently the name of the folder of the session where this will be stored. 
    The name will be "board" + index i from the static variable board_no. The board_no will be assigned to each board in the scene.
*/
[Serializable]
public class BoardSaveFormat : SaveFormat
{

    // texture for drawings. 
    public string texture_file_name;
    public string saved_folder;

    public int board_no = 0;

    public BoardSaveFormat() : base(FormatType.BOARD) {
    } // end NotecardSaveFormat

    public override async Task UpdateData(GameObject board, string save_des_folder) {
        BoardSavable boardSavable = board.GetComponent<BoardSavable>();
        board_no = boardSavable.id; 
        
        // texture for drawing
        Drawable dr = board.GetComponent<Drawable>();
        if (dr.isModified()) {
            byte[] texture_data = dr.GetTextureColor();
            texture_file_name = "board" + (board_no) + ".png";
            saved_folder = save_des_folder;
            string path = Path.Combine(saved_folder, texture_file_name);
            await FileManager.WriteBytesToAsync(path, texture_data);
        } else {
            texture_file_name = "";
        } // end else
    } // end UpdateData

    public override async Task<bool> LoadObjectInto(GameObject board) {
        // // load texture
        Drawable dr = board.GetComponent<Drawable>();
        if (texture_file_name != "") {
            string path = Path.Combine(saved_folder, texture_file_name);
            Debug.Log("I'm here in loading texture: " + path);
            byte[] data = FileManager.ReadBytesFrom(path);
            if (data != null) {
                Debug.Log("Load texture here");
                dr.UpdateTexture(data);
                dr.SetModified();
            }
        } //end if
    
        return true;
    } // end LoadObject
}
