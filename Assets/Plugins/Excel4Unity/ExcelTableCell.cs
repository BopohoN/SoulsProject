using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ExcelTableCellType
{
    None = 0,
    TextField = 1,
    Label = 2,
    Popup = 3,
}

public class ExcelTableCell
{
    public int ColumnIndex;
    public int RowIndex;


    public ExcelTableCellType Type = ExcelTableCellType.TextField;

    public string Value;
    public List<string> ValueSelected = new List<string>();


    public float width = 50f;

    public ExcelTableCell(int row, int column, string value)
    {
        RowIndex = row;
        ColumnIndex = column;
        Value = value;
    }
}